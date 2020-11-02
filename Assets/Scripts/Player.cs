//using GameDevHQ.Filebase.DataModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    /// <summary>
    /// - 54 blocks
    /// - block = length is 3x width, thickness is 1/5 length
    /// - randomize dimmensions for difficulty
    /// </summary>
    
    // target obj
    [SerializeField]
    private GameObject _target;
    private CoffinBehavior _targetBehavior;
    private Rigidbody _targetRB;
    private Material _targetMaterial;
    private Renderer[] _targetChildRenderers;
    private GameObject _targetParent;
    private GameObject _currentParent;

    // inputs
    private float _horizontalInput;
    private float _verticalInput;
    private float _sideInput;
    [SerializeField]
    private float _inputSpeed = 5f;
    private Vector3 _inputVector;
    private bool _isMoving = false;
    private bool _hasMoved = false;
    private bool _canMove = false;
    private bool _canDrop = false;

    private string _baseColor = "_BaseColor";
    private Color _defaultColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    private Color _selectedColor = Color.red;

    private void Start()
    {
        _target = null;
    }

    private void Update()
    {
        if (_target == null || _hasMoved == false)
        {
            CalculateTarget();
        }

        if (_target != null)
        {
            CalculateMovement();
            CalculateDrop();
        }

        if (GameManager.Instance._gameOver)
        {
            _canMove = false;
        }
    }

    private void FixedUpdate()
    {
        if (_canMove && _target != null)
        {
            _targetRB.velocity = _inputVector;
        }
    }

    private void CalculateTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!GameManager.Instance._starting)
            {
                UIManager.Instance.ToggleGameIntroPanel();
                GameManager.Instance._starting = true;
                AudioManager.Instance.PlayGameBackgroundAudio();
                AudioManager.Instance.StartZombieGroanRoutine();
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray.origin, ray.direction * 10, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Coffin"))
                {
                    AssignTarget(hitInfo);
                }
            }
        }
    }

    private void AssignTarget(RaycastHit target)
    {
        if (_target != null)
        {
            ResetTarget();

            _target = null;
        }

        _target = target.collider.gameObject;
        Debug.Log(_target.name + " selected!");

        _targetParent = _target.transform.parent.gameObject;
        Debug.Log(_target.transform.parent);

        if (_currentParent == _targetParent)
        {
            _target = null;

            return;
        }
        else
        {
            _targetBehavior = _target.GetComponent<CoffinBehavior>();
            _targetBehavior._isSelected = true;

            _targetRB = _target.GetComponent<Rigidbody>();
            _targetRB.useGravity = false;

            _targetMaterial = _target.GetComponent<Renderer>().material;
            _targetMaterial.SetColor(_baseColor, _selectedColor);

            _targetChildRenderers = _target.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in _targetChildRenderers)
            {
                renderer.material.SetColor(_baseColor, _selectedColor);
            }
            
            
        }
    }

    private void ResetTarget()
    {
        _canMove = false;
        _canDrop = false;

        _targetRB.useGravity = true;
        _targetMaterial.SetColor(_baseColor, _defaultColor);
        foreach (Renderer renderer in _targetChildRenderers)
        {
            renderer.material.SetColor(_baseColor, _defaultColor);
        }
    }

    private void CalculateMovement()
    {
        _isMoving = false;

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        // horizontal input
        if (Input.GetKey(KeyCode.A)) 
        {
            _isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D)) 
        {
            _isMoving = true;
        }

        // vertical input
        if (Input.GetKey(KeyCode.W)) 
        {
            _isMoving = true;
        }
        else if (Input.GetKey(KeyCode.S)) 
        {
            _isMoving = true;
        }

        // side input
        if (Input.GetKey(KeyCode.Q)) 
        {
            _isMoving = true;
            _sideInput = -1;
        }
        else if (Input.GetKey(KeyCode.E)) 
        {
            _isMoving = true;
            _sideInput = 1f;
        }
        else
        {
            _sideInput = 0;
        }

        if (_isMoving)
        {
            if (_hasMoved == false)
            {
                _hasMoved = true;
            }

            _inputVector = new Vector3(_horizontalInput * _inputSpeed, _verticalInput * _inputSpeed, _sideInput * _inputSpeed);
        }
        else
        {
            _inputVector = Vector3.zero;
            _targetRB.velocity = Vector3.zero;
            _targetRB.angularVelocity = Vector3.zero;
        }

        if (!_canMove)
        {
            _canMove = true;
        }
    }

    private void CalculateDrop()
    {
        if (!GameManager.Instance._gameOver && _targetBehavior._isOut)
        {
            if (!_canDrop)
            {
                _canDrop = true;

                UIManager.Instance.ToggleFeedNotification();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                ResetTarget();

                StartCoroutine(_targetBehavior.DestroyRoutine());

                _currentParent = _targetParent;

                _target = null;

                _hasMoved = false;

                UIManager.Instance.ToggleFeedNotification();
                UIManager.Instance.UpdateSoulsRemaining();
            }
        }

        if (!_targetBehavior._isOut)
        {
            if (_canDrop)
            {
                _canDrop = false;

                UIManager.Instance.ToggleFeedNotification();
            }
        }
    }
}

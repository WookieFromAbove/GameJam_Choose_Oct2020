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
    /// Jenga
    /// - 54 blocks
    /// - block = length is 3x width, thickness is 1/5 length
    /// - randomize dimmensions for difficulty
    /// </summary>
   
    // click on block
    // move block with wasdqe

    [SerializeField]
    private GameObject _target;
    private CoffinBehavior _targetBehavior;
    private Rigidbody _targetRB;
    private Vector3 _targetPos;

    private float _horizontalInput;
    private float _verticalInput;
    private float _sideInput;
    [SerializeField]
    private float _inputSpeed = 5f;
    private bool _isMoving = false;

    [SerializeField]
    private GameObject _coffinContainer;
    private List<GameObject> _coffinList = new List<GameObject>();

    private Vector3 _inputVector;
    private bool _canMove = false;
    private bool _canDrop = false;

    private void Start()
    {
        VaryCoffinSize();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CalculateTarget();
        }

        if (_target != null)
        {
            CalculateMovement();
            CalculateDrop();
        }

        if (GameManager.Instance._towerFailure)
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

    private void VaryCoffinSize()
    {
        var coffins = GameObject.FindGameObjectsWithTag("Coffin");

        foreach (var coffin in coffins)
        {
            float randomVariance = Random.Range(0.99f, 1.01f);

            var coffinScale = coffin.transform.localScale;
            coffinScale.y *= randomVariance;

            coffin.transform.localScale = coffinScale;

            _coffinList.Add(coffin);
        }
    }

    private void CalculateTarget()
    {
        _target = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray.origin, ray.direction * 10, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("Coffin"))
            {
                _target = hitInfo.collider.gameObject;
                Debug.Log(_target.name + " selected!");

                _targetBehavior = _target.GetComponent<CoffinBehavior>();
                _targetBehavior._isSelected = true;
                
                _targetRB = _target.GetComponent<Rigidbody>();
                _targetRB.useGravity = false;
            }
        }
    }

    private void CalculateMovement()
    {
        _isMoving = false;

        // horizontal input
        if (Input.GetKey(KeyCode.A)) // forward
        {
            _isMoving = true;
            _horizontalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.D)) // back
        {
            _isMoving = true;
            _horizontalInput = -1f;
        }
        else
        {
            _horizontalInput = 0f;
        }

        // vertical input
        if (Input.GetKey(KeyCode.W)) // up
        {
            _isMoving = true;
            _verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S)) // down
        {
            _isMoving = true;
            _verticalInput = -1f;
        }
        else
        {
            _verticalInput = 0f;
        }

        // side input
        if (Input.GetKey(KeyCode.Q)) // left
        {
            _isMoving = true;
            _sideInput = -1;
        }
        else if (Input.GetKey(KeyCode.E)) // right
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
        if (!GameManager.Instance._towerFailure && _targetBehavior._isOut)
        {
            if (!_canDrop)
            {
                _canDrop = true;

                UIManager.Instance.ToggleFeedNotification();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _canMove = false;
                _canDrop = false;

                _targetRB.useGravity = true;

                _target = null;

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

        // wait to see if tower failure & over feeding area
        // if tower failure

        
    }
}

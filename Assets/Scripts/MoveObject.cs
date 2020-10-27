using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveObject : MonoBehaviour
{
    /// <summary>
    /// Jenga
    /// - 54 blocks
    /// - block = length is 3x width, thickness is 1/5 length
    /// - random variations from dimmensions for difficulty
    /// </summary>
   
    // click on block
    // move block with wasd

    [SerializeField]
    private GameObject _target;
    private Rigidbody _targetRB;

    private float _horizontalInput;
    private float _verticalInput;
    private float _sideInput;
    [SerializeField]
    private float _inputSpeed = 10f;
    private bool _isMoving = false;

    [SerializeField]
    private GameObject _coffinContainer;
    private List<GameObject> _coffinList = new List<GameObject>();

    private Vector3 _inputVector;
    private bool _canMove = false;

    private void Start()
    {
        VaryCoffinSize();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _target = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray.origin, ray.direction * 10, out hitInfo))
            {
                _target = hitInfo.collider.gameObject;

                _targetRB = _target.GetComponent<Rigidbody>();
                _targetRB.useGravity = false;
            }
        }

        if (_target != null)
        {
            CalculateMovementInput();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _canMove = false;

            _targetRB.useGravity = true;

            _target = null;
        }
    }

    private void FixedUpdate()
    {
        if (_canMove)
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

    private void CalculateMovementInput()
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
            _inputVector = new Vector3(_horizontalInput * 10f, _verticalInput * 10f, _sideInput * 10f);
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
}

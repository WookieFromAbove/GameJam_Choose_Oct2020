using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    /// <summary>
    /// Jenga
    /// - 54 blocks
    /// - block = length is 3x width, thickness is 1/5 length
    /// - random variations from dimmensions for difficulty
    /// </summary>
    /// 

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
                //_targetRB.isKinematic = true;
            }
        }

        if (_target != null)
        {
            //MoveBlock();
            AddForce();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _targetRB.useGravity = true;
            //_targetRB.isKinematic = false;

            _target = null;
        }
    }

    private void AddForce()
    {
        _isMoving = false;

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
            var moveDirection = new Vector3(_horizontalInput, _verticalInput, _sideInput);
            var velocity = moveDirection * _inputSpeed;

            _targetRB.AddForce(velocity);

            //ReachVelocity(_targetRB, velocity, 10);
        }
        else
        {
            _targetRB.velocity = Vector3.zero;
            _targetRB.angularVelocity = Vector3.zero;
        }
    }

    private void ReachVelocity(Rigidbody rb, Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
    {
        if (force == 0 || velocity.magnitude == 0)
        {
            return;
        }

        velocity = velocity + velocity.normalized * 0.2f * rb.drag;
        force = Mathf.Clamp(force, -rb.mass / Time.fixedDeltaTime, rb.mass / Time.fixedDeltaTime);

        if (rb.velocity.magnitude == 0)
        {
            rb.AddForce(velocity * force, mode);
        }
        else
        {
            var velocityToTarget = (velocity.normalized * Vector3.Dot(velocity, rb.velocity) / velocity.magnitude);
            rb.AddForce((velocity - velocityToTarget) * force, mode);
        }
    }

    private void MoveBlock()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _sideInput = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            _sideInput = -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            _sideInput = 1f;
        }

        Vector3 direction = new Vector3(_horizontalInput, _verticalInput, _sideInput);
        transform.Translate(direction * _inputSpeed * Time.deltaTime);
    }
}

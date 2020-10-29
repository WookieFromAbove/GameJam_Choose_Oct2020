using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinBehavior : MonoBehaviour
{
    public bool _isSelected = false;
    public bool _isOut = false;

    private Vector3 _startPos;
    private Vector3 _currentPos;

    private float _maxDistancce = 2.5f;
    private float _currentDistance;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        CheckIfFailed();
    }

    private void CheckIfFailed()
    {
        if (!_isSelected)
        {
            _currentPos = transform.position;

            _currentDistance = Vector3.Distance(_startPos, _currentPos);
            //Debug.Log("Distance: " + _currentDistance);

            if (_currentDistance > _maxDistancce)
            {
                GameManager.Instance.GameOver();
                Debug.Log("Tower Failure!");
            }
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isOut == true && other.CompareTag("Plane"))
        {
            _isOut = false;
            Debug.Log("Coffin is NOT out!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Plane"))
        {
            _isOut = true;
            Debug.Log("Coffin is out!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CoffinBehavior : MonoBehaviour
{
    public bool _isSelected = false;
    public bool _isOut = false;

    private Vector3 _startPos;
    private Vector3 _currentPos;

    private float _maxDistancce = 3.5f;
    private float _currentDistance;

    private WaitForSeconds _destroyWait = new WaitForSeconds(2.5f);

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

            if (_currentDistance > _maxDistancce)
            {
                GameManager.Instance.GameOver();
            }
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isOut == true && other.CompareTag("Plane"))
        {
            _isOut = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Plane"))
        {
            _isOut = true;
        }
    }

    public IEnumerator DestroyRoutine()
    {
        CoffinContainer.Instance.RemoveFromList(this.gameObject);

        yield return _destroyWait;

        AudioManager.Instance.PlayZombieClip(0);

        Destroy(this.gameObject);
    }
}

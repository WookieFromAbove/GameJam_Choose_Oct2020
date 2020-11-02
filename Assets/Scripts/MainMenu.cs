using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // camera
    private bool _moveCamera = false;
    private Vector3 _mainCameraStartPos = new Vector3(0, 1f, -10f);
    private Vector3 _mainCameraTargetPos = new Vector3(0, -0.1f, 5f);
    private float _moveSpeed = 0.5f;

    // gate
    private bool _canOpen = false;
    private bool _isOpen = false;

    private Vector3 _defaultRotation = Vector3.zero;
    private float _minRotation = 5f;

    [SerializeField]
    private GameObject _gate1;
    private Vector3 _gate1CurrentRotation;
    private Vector3 _gate1TargetRotation = new Vector3(0, 75f, 0);
    private float _check1;

    [SerializeField]
    private GameObject _gate2;
    private Vector3 _gate2CurrentRotation;
    private Vector3 _gate2TargetRotation = new Vector3(0, -75f, 0);
    private float _check2;

    // game text
    [SerializeField]
    private GameObject _startText;

    private void Start()
    {
        if (!_startText.gameObject.activeInHierarchy)
        {
            _startText.gameObject.SetActive(true);
        }

        Camera.main.transform.position = _mainCameraStartPos;

        _gate1.transform.localEulerAngles = _defaultRotation;
        _gate2.transform.localEulerAngles = _defaultRotation;

        _gate1CurrentRotation = _gate1.transform.localEulerAngles;
        _gate2CurrentRotation = _gate2.transform.localEulerAngles;

        _canOpen = true;
    }

    private void Update()
    {
        if (_canOpen)
        {
            OpenGate();
        }        

        if (_isOpen)
        {
            RotateGatePos();
        }

        if (_moveCamera)
        {
            MoveCamera();
        }
    }

    private void OpenGate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _startText.gameObject.SetActive(false);

            AudioManager.Instance.PlayGateOpening();

            _isOpen = true;
            _canOpen = false;
            _moveCamera = true;
        }
    }

    private void MoveCamera()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, _mainCameraTargetPos, _moveSpeed * Time.deltaTime);
    }

    private void RotateGatePos()
    {
        _gate1CurrentRotation.y = Mathf.LerpAngle(_gate1CurrentRotation.y, _gate1TargetRotation.y, Time.deltaTime);
        _gate1.transform.localEulerAngles = _gate1CurrentRotation;

        _gate2CurrentRotation.y = Mathf.LerpAngle(_gate2CurrentRotation.y, _gate2TargetRotation.y, Time.deltaTime);
        _gate2.transform.localEulerAngles = _gate2CurrentRotation;

        _check1 = _gate1TargetRotation.y - _gate1.transform.localEulerAngles.y;
        _check2 = _gate2TargetRotation.y - _gate2.transform.localEulerAngles.y;

        if (_check1 <= _minRotation && _check2 <= _minRotation)
        {
            _isOpen = false;

            GameManager.Instance.LoadScene(1);
        }
    }
}

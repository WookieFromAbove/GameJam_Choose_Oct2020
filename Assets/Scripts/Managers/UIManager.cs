using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    private Image _gameIntroPanel;

    [SerializeField]
    private TextMeshProUGUI _soulsCountTMP;
    [SerializeField]
    private TextMeshProUGUI _gameTimerTMP;
    [SerializeField]
    private TextMeshProUGUI _feedNotificationTMP;
    [SerializeField]
    private TextMeshProUGUI _gameWonTMP;
    [SerializeField]
    private TextMeshProUGUI _gameLostTMP;

    private bool _canFeed = false;

    [SerializeField]
    private Image _statusImg;

    private int _totalSouls = 54;
    private int _currentSouls;

    private float _gameTime = 0f;
    private float _gameMinutes = 0f;
    private float _gameSeconds = 0f;

    public override void Init()
    {
        base.Init();

        _gameIntroPanel.gameObject.SetActive(true);

        _soulsCountTMP.text = _totalSouls.ToString();
    }

    private void Start()
    {
        _currentSouls = _totalSouls;
    }

    private void Update()
    {
        if (GameManager.Instance._starting && !GameManager.Instance._gameOver)
        {
            Timer();
        }
    }

    public void ToggleGameIntroPanel()
    {
        if (_gameIntroPanel.gameObject.activeInHierarchy)
        {
            _gameIntroPanel.gameObject.SetActive(false);
        }
    }

    public void UpdateSoulsRemaining()
    {
        _currentSouls--;

        _soulsCountTMP.text = _currentSouls.ToString();
    }

    public void ToggleFeedNotification()
    {
        _canFeed = !_canFeed;

        _feedNotificationTMP.gameObject.SetActive(_canFeed);
    }

    public void ShowGameWonText()
    {
        if (!_gameWonTMP.gameObject.activeInHierarchy)
        {
            _gameWonTMP.gameObject.SetActive(true);
        }
    }

    public void ShowGameOverText()
    {
        if (!_gameLostTMP.gameObject.activeInHierarchy)
        {
            _feedNotificationTMP.gameObject.SetActive(false);
            _gameLostTMP.gameObject.SetActive(true);
        }
    }

    public void Timer()
    {
        _gameTime += Time.deltaTime;

        _gameMinutes = Mathf.FloorToInt(_gameTime / 60);
        _gameSeconds = Mathf.FloorToInt(_gameTime % 60);

        _gameTimerTMP.text = _gameMinutes.ToString("0") + ":" + _gameSeconds.ToString("00");
    }
}

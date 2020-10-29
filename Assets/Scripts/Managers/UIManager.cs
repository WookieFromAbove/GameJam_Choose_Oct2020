using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    // score
    // increasing time counter 
    // image with color changing reflecting failure %?

    [SerializeField]
    private TextMeshProUGUI _soulsCountTMP;
    [SerializeField]
    private TextMeshProUGUI _hungerLevelTMP;
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

    public override void Init()
    {
        base.Init();

        _soulsCountTMP.text = _totalSouls.ToString();
        _hungerLevelTMP.text = "Hunger Level";
    }

    private void Start()
    {
        _currentSouls = _totalSouls;
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

    public void ShowGameOverText()
    {
        if (!_gameLostTMP.gameObject.activeInHierarchy)
        {
            _gameLostTMP.gameObject.SetActive(true);
        }
    }
}

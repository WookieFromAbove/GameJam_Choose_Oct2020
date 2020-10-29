using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public bool _towerFailure = false;

    public void GameOver()
    {
        if (!_towerFailure)
        {
            _towerFailure = true;
            UIManager.Instance.ShowGameOverText();
        }
    }
}

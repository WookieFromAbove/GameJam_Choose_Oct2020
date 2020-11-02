using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public bool _starting = false;
    public bool _gameOver = false;

    private void Update()
    {
        if (_gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                LoadScene(0);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LoadScene(int sceneID) // 0 = main menu, 1 = game
    {
        SceneManager.LoadScene(sceneID);
    }

    public void GameOver()
    {
        if (!_gameOver)
        {
            _gameOver = true;
            UIManager.Instance.ShowGameOverText();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinContainer : MonoSingleton<CoffinContainer>
{
    [SerializeField]
    private GameObject _topStatueObj;

    [SerializeField]
    private List<GameObject> _coffinList = new List<GameObject>();

    private void Start()
    {
        VaryCoffinSize();
    }

    private void Update()
    {
        CheckIfTopDropped();
    }

    private void VaryCoffinSize()
    {
        var coffins = GameObject.FindGameObjectsWithTag("Coffin");

        _coffinList.Clear();

        foreach (var coffin in coffins)
        {
            _coffinList.Add(coffin);

            float randomVariance = Random.Range(0.98f, 1.01f);

            var coffinScale = coffin.transform.localScale;
            coffinScale.y *= randomVariance;

            coffin.transform.localScale = coffinScale;
        }
    }

    private void CheckIfTopDropped()
    {
        if (_topStatueObj.transform.position.y < 1f && !GameManager.Instance._gameOver)
        {
            if (_coffinList.Count <= 1)
            {
                UIManager.Instance.ShowGameWonText();
            }
            else
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    public void RemoveFromList(GameObject obj)
    {
        _coffinList.Remove(obj);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField]
    private AudioSource _audioManagerAS;

    [SerializeField]
    private AudioSource _gameBackgroundAS;

    [SerializeField]
    private AudioClip _gateOpeningClip;

    [SerializeField]
    private List<AudioClip> _zombieClipList = new List<AudioClip>();

    private WaitForSeconds _zombieGroanWaitShort;
    private WaitForSeconds _zombieGroanWaitMedium;
    private WaitForSeconds _zombieGroanWaitLong;
    private int[] _randomWaitIndex = new int[] { 0, 1, 2 };
    private int _randomWait;

    public override void Init()
    {
        base.Init();

        _audioManagerAS = GetComponent<AudioSource>();
        if (_audioManagerAS == null)
        {
            Debug.Log("AudioManagerAS is NULL.");
        }

        _zombieGroanWaitShort = new WaitForSeconds(7f);
        _zombieGroanWaitMedium = new WaitForSeconds(10f);
        _zombieGroanWaitLong = new WaitForSeconds(13f);
    }

    public void PlayGateOpening()
    {
        StartCoroutine(GateOpeningRoutine());
    }

    private IEnumerator GateOpeningRoutine()
    {
        _audioManagerAS.clip = _gateOpeningClip;
        _audioManagerAS.Play();

        yield return new WaitForSeconds(3f);

        _audioManagerAS.Stop();
    }

    public void PlayGameBackgroundAudio()
    {
        if (!_gameBackgroundAS.gameObject.activeInHierarchy)
        {
            _gameBackgroundAS.gameObject.SetActive(true);
        }
    }

    public void PlayZombieClip(int clipID) // 0 = eating, 1 = groan
    {
        switch (clipID)
        {
            case 0:
                _audioManagerAS.clip = _zombieClipList[clipID];
                _audioManagerAS.Play();
                break;

            case 1:
                AudioSource.PlayClipAtPoint(_zombieClipList[clipID], Camera.main.transform.position);
                break;

            default:
                break;
        }
    }

    public void StartZombieGroanRoutine()
    {
        StartCoroutine(ZombieGroanRoutine());
    }

    private IEnumerator ZombieGroanRoutine()
    {
        while (!GameManager.Instance._gameOver)
        {
            _randomWait = _randomWaitIndex[Random.Range(0, _randomWaitIndex.Length)];

            switch (_randomWait)
            {
                case 0:
                    yield return _zombieGroanWaitShort;
                    break;

                case 1:
                    yield return _zombieGroanWaitMedium;
                    break;

                case 2:
                    yield return _zombieGroanWaitLong;
                    break;

                default:
                    break;
            }

            PlayZombieClip(1);
        }
    }
}

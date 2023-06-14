using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardedAnnouncement : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private float _timeRepeat;
    [SerializeField] private float _timeShow;
    [SerializeField] private int _repeats = 3;

    private int _countRepeat = 0;

    private void OnEnable()
    {
        _game.LevelUp += ResetRepeats;
    }

    private void OnDisable()
    {
        _game.LevelUp -= ResetRepeats;
    }

    private void Start()
    {
        StartCoroutine(Announcement());
    }

    private IEnumerator Announcement()
    {
        if(_countRepeat < _repeats)
        {
            _countRepeat++;
            _gameObject.SetActive(true);

            yield return new WaitForSeconds(_timeShow);

            _gameObject.SetActive(false);

            yield return new WaitForSeconds(_timeRepeat);

            StartCoroutine(Announcement());
        }
    }

    private void ResetRepeats(int value)
    {
        _countRepeat = 0;
        _gameObject.SetActive(false);
        StartCoroutine(Announcement());
    }
}

using System;
using System.Collections;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private Stat _totalScoreText;
    [SerializeField] private Stat _score;
    [SerializeField] private Stat _bounces;
    [SerializeField] private Stat _money;
    [SerializeField] private Stat _ScoreTime;

    private int _totalScore = 0;

    private void OnEnable()
    {
        StartCoroutine(ShowStatsTime(_totalScore, _playerInfo.TotalScore, _totalScoreText));
        StartCoroutine(ShowStatsTime(0, _playerInfo.Score, _score));
        StartCoroutine(ShowStatsTime(0, _playerInfo.Bounces, _bounces));
        StartCoroutine(ShowStatsTime(0, _playerInfo.TotalMoneyLevel, _money));
        _ScoreTime.ChangeText((int)_levelLoader.ScoreTime);
        _totalScore = _playerInfo.TotalScore;
    }

    private IEnumerator ShowStatsTime(int startValue, int endValue, Stat text)
    {
        int value;
        int highDifference = 1000;
        int lowDifference = 300;
        int highVariable = 100;
        int midVariable = 20;
        int minVariable = 1;

        for (int i = startValue; i <= endValue; i += value)
        {
            int variable = endValue - i;

            if (variable > highDifference)
                value = highVariable;
            else if (variable < lowDifference)
                value = minVariable;
            else
                value = midVariable;

            text.ChangeText(i);

            yield return null;
        }

        text.ChangeText(endValue);
    }
}

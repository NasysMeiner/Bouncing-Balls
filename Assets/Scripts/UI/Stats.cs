using System.Collections;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Stat _totalScoreText;
    [SerializeField] private Stat _score;
    [SerializeField] private Stat _bounces;
    [SerializeField] private Stat _money;
    [SerializeField] private Stat _ScoreTime;

    private int _totalScore = 0;

    private void OnEnable()
    {
        StartCoroutine(ShowStatsTime(_totalScore, _game.TotalScore, _totalScoreText));
        StartCoroutine(ShowStatsTime(0, (int)_game.Score, _score));
        StartCoroutine(ShowStatsTime(0, _game.Bounces, _bounces));
        StartCoroutine(ShowStatsTime(0, (int)_game.TotalMoneyLevel, _money));
        _ScoreTime.ChangeText((int)_game.ScoreTime);
        _totalScore = _game.TotalScore;
    }

    private IEnumerator ShowStatsTime(int startValue,int endValue , Stat text)
    {
        int value;

        for (int i = startValue; i <= endValue; i += value)
        {
            int variable = endValue - i;

            if (variable > 1000)
                value = 100;
            else if (variable < 300)
                value = 1;
            else
                value = 20;

            text.ChangeText(i);

            yield return null;
        }

        text.ChangeText(endValue);
    }
}

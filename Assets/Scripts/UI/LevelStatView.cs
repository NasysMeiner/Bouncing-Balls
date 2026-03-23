using System.Collections;
using TMPro;
using UnityEngine;

public class LevelStatView : MonoBehaviour
{
    [SerializeField] private TMP_Text _totalScore;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private TMP_Text _bounceCount;
    [SerializeField] private TMP_Text _totalMoney;
    [SerializeField] private TMP_Text _timeInGame;

    public void SetLevelData(LevelData levelData)
    {
        StartCoroutine(ShowStatsTime(0, levelData.TotalScore, _totalScore));
        StartCoroutine(ShowStatsTime(0, levelData.Score, _score));
        StartCoroutine(ShowStatsTime(0, levelData.BounceCount, _bounceCount));
        StartCoroutine(ShowStatsTime(0, levelData.TotalMoney, _totalMoney));
        StartCoroutine(ShowStatsTime(0, levelData.TimeInLevel, _timeInGame));
    }

    private IEnumerator ShowStatsTime(int startValue, int endValue, TMP_Text text)
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

            text.text = i.ToString();

            yield return null;
        }

        text.text = endValue.ToString();
    }
}
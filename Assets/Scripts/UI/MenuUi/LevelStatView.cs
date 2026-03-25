using System.Collections;
using TMPro;
using UnityEngine;

namespace BouncingBalls
{
    public class LevelStatView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _totalScore;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private TMP_Text _bounceCount;
        [SerializeField] private TMP_Text _totalMoney;
        [SerializeField] private TMP_Text _timeInGame;

        private int _highDifference = 1000;
        private int _lowDifference = 300;
        private int _highVariable = 100;
        private int _midVariable = 20;
        private int _minVariable = 1;

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

            for (int i = startValue; i <= endValue; i += value)
            {
                int variable = endValue - i;

                if (variable > _highDifference)
                    value = _highVariable;
                else if (variable < _lowDifference)
                    value = _minVariable;
                else
                    value = _midVariable;

                text.text = i.ToString();

                yield return null;
            }

            text.text = endValue.ToString();
        }
    }
}
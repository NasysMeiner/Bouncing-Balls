using BouncingBalls.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BouncingBalls.View
{
    public class LevelStatView : MonoBehaviour
    {
        [SerializeField] private List<StatProfile> _statList = new();

        private int _highDifference = 1000;
        private int _lowDifference = 300;
        private int _highVariable = 100;
        private int _midVariable = 20;
        private int _minVariable = 1;

        public void SetLevelData(LevelData levelData)
        {
            foreach(StatProfile profile in _statList)
                StartCoroutine(ShowStatsTime(0, levelData.GetStat(profile.StatType), profile.Text));
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
using BouncingBalls.GameSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BouncingBalls.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private Image _scoreBar;
        [SerializeField] private List<SubLevelView> _subLevelViews;

        private ScoreController _scoreController;

        private int _maxScore;

        private void OnDestroy()
        {
            if (_scoreController != null)
            {
                _scoreController.OnChangeScore -= OnChangeScore;
                _scoreController.OnSubLevelUp -= OnSubLevelUp;
                _scoreController.OnPostInitialize -= OnPostInitialize;
            }
        }

        public void Initialize(ScoreController scoreController)
        {
            _scoreController = scoreController;

            _scoreController.OnChangeScore += OnChangeScore;
            _scoreController.OnSubLevelUp += OnSubLevelUp;
            _scoreController.OnPostInitialize += OnPostInitialize;
        }

        public void OnChangeScore(int currentScore)
        {
            _scoreBar.fillAmount = (float)currentScore / _maxScore;
        }

        public void OnSubLevelUp(int subLevel)
        {
            if (subLevel < _subLevelViews.Count)
                _subLevelViews[subLevel].ActiveView();
        }

        private void OnPostInitialize(int maxScore)
        {
            _maxScore = maxScore;

            foreach (var view in _subLevelViews)
                view.InactiveView();

            OnChangeScore(0);
        }
    }
}
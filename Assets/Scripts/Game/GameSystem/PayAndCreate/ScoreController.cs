using BouncingBalls.Block;
using BouncingBalls.Data;
using BouncingBalls.LevelSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls.GameSystem
{
    public class ScoreController : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField] private List<float> _subLevelUpBorder = new();

        private GameManager _gameManager;
        private BlockManager _blockManager;
        private Bank _bank;

        private int _currentLevelScore = 0;
        private int _maxScore = 0;
        private int _bounces;
        private int _bonusTime = 1000;

        private int _currentSubLevel = 0;
        private float _currentBorder;
        private bool _inGame = false;

        public event Action<LevelData> FullScore;
        public event Action<int> OnPostInitialize;
        public event Action<int> OnSubLevelUp;
        public event Action<int> OnChangeScore;

        private void OnDestroy()
        {
            if (_blockManager != null)
            {
                _blockManager.BlockAdded -= OnBlockAdded;
                _blockManager.BlockRemoved -= OnBlockRemoved;
            }
        }

        public void Initialize(BlockManager blockManager, Bank bank, GameManager gameManager)
        {
            _gameManager = gameManager;
            _blockManager = blockManager;
            _bank = bank;

            _blockManager.BlockAdded += OnBlockAdded;
            _blockManager.BlockRemoved += OnBlockRemoved;
        }

        public void PostInitialize(int maxScore)
        {
            _maxScore = maxScore;

            _currentBorder = _subLevelUpBorder[0];

            _inGame = true;
            _currentSubLevel = 0;
            _currentLevelScore = 0;

            OnPostInitialize?.Invoke(_maxScore);
        }

        private void AddScore(int value)
        {
            if (!_inGame)
                return;

            _currentLevelScore += value;

            _bounces++;

            if (_currentLevelScore >= _currentBorder * _maxScore)
            {
                if (_currentBorder != 1)
                {
                    OnSubLevelUp?.Invoke(_currentSubLevel++);
                    SetNewBorderScore();
                }
                else
                {
                    CalculeteTotalScore();
                }
            }

            OnChangeScore?.Invoke(_currentLevelScore);
        }

        private void CalculeteTotalScore()
        {
            _inGame = false;

            LevelData levelData = new();

            int totalMoney = _bank.TotalMoney;
            float timeInLevel = _gameManager.TimeInLevel;

            int totalScore = _bounces + _currentLevelScore + totalMoney;
            float bonus = totalScore * timeInLevel / _bonusTime;
            totalScore -= (int)bonus;

            levelData.TotalScore = totalScore;
            levelData.Score = _currentLevelScore;
            levelData.BounceCount = _bounces;
            levelData.TotalMoney = totalMoney;
            levelData.TimeInLevel = (int)timeInLevel;

            FullScore?.Invoke(levelData);
        }

        private void SetNewBorderScore()
        {
            foreach (var border in _subLevelUpBorder)
            {
                if (border > _currentBorder)
                {
                    _currentBorder = border;
                    return;
                }
            }

            _currentBorder = 1;
        }

        private void OnBlockAdded(Block.Block block)
        {
            block.ScoreEarned += OnScoreEarned;
        }

        private void OnBlockRemoved(Block.Block block)
        {
            block.ScoreEarned -= OnScoreEarned;
        }

        private void OnScoreEarned(BounceScoreData bounceScoreData)
        {
            if (bounceScoreData.Score > 0)
                AddScore(bounceScoreData.Score);
        }
    }
}
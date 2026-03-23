using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private List<float> _subLevelUpBorder = new();

    private GameManager _gameManager;
    private BlockManager _blockManager;
    private Bank _bank;

    private int _score = 0;
    private int _maxScore = 0;
    private int _bounces;
    private int _bonusTime = 1000;

    private int _currentSubLevel = 0;
    private float _currentBorder;
    private bool _inGame = false;

    public int MaxScore => _maxScore;

    public event Action<LevelData> OnFullScore;
    public event Action<int> OnPostInitialize;
    public event Action<int> OnSubLevelUp;
    public event Action<int> OnChangeScore;

    private void OnDestroy()
    {
        if (_blockManager != null)
        {
            _blockManager.OnBlockAdded -= OnBlockAdded;
            _blockManager.OnBlockRemoved -= OnBlockRemoved;
        }
    }

    public void Initialize(BlockManager blockManager, Bank bank, GameManager gameManager)
    {
        _gameManager = gameManager;
        _blockManager = blockManager;
        _bank = bank;

        _blockManager.OnBlockAdded += OnBlockAdded;
        _blockManager.OnBlockRemoved += OnBlockRemoved;
    }

    public void PostInitialize(int maxScore)
    {
        _maxScore = maxScore;

        _currentBorder = _subLevelUpBorder[0];

        _inGame = true;
        _currentSubLevel = 0;
        _score = 0;

        OnPostInitialize?.Invoke(_maxScore);
    }

    private void AddScore(int value)
    {
        if (!_inGame)
            return;

        _score += value;

        _bounces++;

        if(_score >= _currentBorder * _maxScore)
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

        OnChangeScore?.Invoke(_score);
    }

    private void CalculeteTotalScore()
    {
        _inGame = false;

        LevelData levelData = new();

        int totalMoney = _bank.TotalMoney;
        float timeInLevel = _gameManager.TimeInLevel;

        int totalScore = _bounces + _score + totalMoney;
        float bonus = totalScore * timeInLevel / _bonusTime;
        totalScore -= (int)bonus;

        levelData.TotalScore = totalScore;
        levelData.Score = _score;
        levelData.BounceCount = _bounces;
        levelData.TotalMoney = totalMoney;
        levelData.TimeInLevel = (int)timeInLevel;

        OnFullScore?.Invoke(levelData);
    }

    private void SetNewBorderScore()
    {
        foreach(var border in _subLevelUpBorder)
        {
            if(border > _currentBorder)
            {
                _currentBorder = border;
                return;
            }
        }

        _currentBorder = 1;
    }

    private void OnBlockAdded(Block block)
    {
        block.OnScoreEarned += OnScoreEarned;
    }

    private void OnBlockRemoved(Block block)
    {
        block.OnScoreEarned -= OnScoreEarned;    
    }

    private void OnScoreEarned(BounceScoreData bounceScoreData)
    {
        if (bounceScoreData.Score > 0)
            AddScore(bounceScoreData.Score);
    }
}

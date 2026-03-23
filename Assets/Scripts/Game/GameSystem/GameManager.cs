using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _startLevel = 0;
    [SerializeField] private List<int> _scoreLevels = new();

    private LevelManager _levelManager;
    private ScoreController _scoreController;

    private UIManager _uiManager;

    private int _currentLevel;
    private float _timeInLevel = 0;

    private bool _isGameStarted = false;

    public int CurrentLevel => _currentLevel;
    public float TimeInLevel => _timeInLevel;

    public event Action<int> OnSetLevel;

    private void OnDestroy()
    {
        if(_scoreController != null)
        {
            _scoreController.OnFullScore -= OnFullScore;
        }
    }

    public void Update()
    {
        if(_isGameStarted)
            _timeInLevel += Time.deltaTime;
    }

    public void InitManager(LevelManager levelManager, UIManager uIManager, ScoreController scoreController)
    {
        _levelManager = levelManager;
        _uiManager = uIManager;
        _scoreController = scoreController;

        _currentLevel = _startLevel;

        _scoreController.OnFullScore += OnFullScore;
    }

    public void StartGame()
    {
        CreateLevel();
    }

    public void CreateLevel()
    {
        _levelManager.GenerateLevel(_currentLevel);
        _scoreController.PostInitialize(_scoreLevels[_currentLevel]);

        _isGameStarted = true;

        OnSetLevel?.Invoke(_currentLevel);
    }

    public void OnFullScore(LevelData levelData)
    {
        _levelManager.StopGame();
        _isGameStarted = false;
        _uiManager.ViewEndLevelPanel(levelData);
    }

    public void StartNextLevel()
    {
        _currentLevel++;
        FullReset();
        CreateLevel();
    }

    private void FullReset()
    {
        _levelManager.FullReset();
    }
}

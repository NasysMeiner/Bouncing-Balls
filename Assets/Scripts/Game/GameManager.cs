using BouncingBalls.Data;
using BouncingBalls.GameSystem;
using BouncingBalls.View;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls.LevelSystem
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private bool _isUnity = true;
        [Space]
        [SerializeField] private int _startLevel = 0;
        [SerializeField] private int _maxLevel = 5;
        [SerializeField] private List<int> _scoreLevels = new();

        private LevelManager _levelManager;
        private ScoreController _scoreController;

        private UIManager _uiManager;

        private int _currentLevel;
        private float _timeInLevel = 0;

        private bool _isGameStarted = false;
        private bool _isPause = false;

        public event Action StartLevel;
        public event Action EndLevel;

        public bool IsUnity => _isUnity;
        public int CurrentLevel => _currentLevel;
        public int MaxLevel => _maxLevel;
        public int MaxLevelInBorder => _scoreLevels.Count;
        public float TimeInLevel => _timeInLevel;

        private void OnDestroy()
        {
            if (_scoreController != null)
            {
                _scoreController.FullScore -= OnFullScore;
            }
        }

        public void Update()
        {
            if (_isGameStarted && !_isPause)
                _timeInLevel += Time.deltaTime;
        }

        public void Initialize(LevelManager levelManager, UIManager uIManager, ScoreController scoreController)
        {
            _levelManager = levelManager;
            _uiManager = uIManager;
            _scoreController = scoreController;

            _currentLevel = _startLevel;

            _scoreController.FullScore += OnFullScore;
        }

        public void SetLevel(int level)
        {
            _currentLevel = level;
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

            StartLevel?.Invoke();
        }

        public void OnFullScore(LevelData levelData)
        {
            _levelManager.StopGame();
            _isGameStarted = false;
            _uiManager.ViewEndLevelPanel(levelData);

            EndLevel?.Invoke();
        }

        public void RestartGame()
        {
            _currentLevel = 0;
            FullReset();
            CreateLevel();
        }

        public void StartNextLevel()
        {
            _currentLevel++;
            FullReset();
            CreateLevel();
        }

        public void PauseOn()
        {
            _isPause = true;
            Time.timeScale = 0;
        }

        public void PauseOff()
        {
            _isPause = false;
            Time.timeScale = 1;
        }

        private void FullReset()
        {
            _levelManager.FullReset();
        }
    }
}
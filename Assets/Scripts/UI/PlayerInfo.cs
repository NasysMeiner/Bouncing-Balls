using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private ScoreView _scoreBar;
    [SerializeField] private Leaderboard _leaderboard;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private BankView _moneyBar;
    [SerializeField] private BankView _cristallBar;
    //[SerializeField] private ShopDistributor _shopDistributor;
    [SerializeField] private int _completeLevelReward = 20;

    private string _name;
    private int _level = 1;
    private int _icon = 0;
    private bool _isShowGuide = false;
    private bool _isUnlockBlockDeleter = false;
    private int _BlockDeleterLevel = 0;

    private int _cristall = 10000;
    private int _money = 10000;
    private int _totalMoneyLevel;

    private int _bounces;
    private int _score = 0;
    private int _totalScore;

    private int _allTime = 1000;
    private float _partTime = 0.99f;

    public int Score { get { return _score; } private set { } }
    public int TotalScore { get { return _totalScore; } private set { } }
    public int Level { get { return _level; } private set { } }
    public string Name { get { return _name; } private set { } }
    public int Cristall { get { return _cristall; } private set { } }
    public bool IsUnlockBascket { get { return _isUnlockBlockDeleter; } private set { } }
    public int LevelUp { get { return _BlockDeleterLevel; } private set { } }
    public int Icon { get { return _icon; } private set { } }
    public int Money { get { return _money; } private set { } }
    public int TotalMoneyLevel { get { return _totalMoneyLevel; } private set { } }
    public bool IsShowGuide { get { return _isShowGuide; } private set { } }
    public int Bounces { get { return _bounces; } private set { } }

    public event UnityAction<int> MoneyChanged;
    public event UnityAction<int> CristallChanged;

    private void OnEnable()
    {
        //_scoreBar.ScoreChange += OnScoreChange;
        //_scoreBar.EndLevel += OnEndLevel;
        _levelLoader.GameStarting += OnStartGame;
        _levelLoader.GenerationStarting += OnGenerationStart;
    }

    private void OnDisable()
    {
        //_scoreBar.ScoreChange -= OnScoreChange;
        //_scoreBar.EndLevel -= OnEndLevel;
        _levelLoader.GameStarting -= OnStartGame;
        _levelLoader.GenerationStarting -= OnGenerationStart;
    }

    public void OnScoreChange(int value)
    {
        _score = value;
    }

    public void ChangeMoney(int value)
    {
        if (!_levelLoader.IsFreezChangeMoney)
        {
            _money += value;
            //_moneyBar.ChangeValue(_money);
            MoneyChanged?.Invoke(_money);

            if (value > 0)
            {
                _totalMoneyLevel += value;
            }
        }
    }

    public void ChangeCristall(int value)
    {
        _cristall += value;
        //_cristallBar.ChangeValue(_cristall);
        CristallChanged?.Invoke(_cristall);
    }

    public void SetName(string name)
    {
        _name = name;
        _levelLoader.SetName(name);
    }

    public void ShowGuideOn()
    {
        _isShowGuide = true;
    }

    public void UnlockBascet()
    {
        _isUnlockBlockDeleter = true;
    }

    public void SetIcon(int value)
    {
        _icon = value;
    }

    public void LevelUpBascet()
    {
        _BlockDeleterLevel++;
    }

    public void ChangeBounces()
    {
        _bounces++;
    }

    //public void LoadLevelData(PlayerData playerData)
    //{
    //    _totalScore = playerData.score;
    //    _level = playerData.level;
    //    _BlockDeleterLevel = playerData.levelUp;
    //    _cristall = playerData.cristall;
    //    _isUnlockBlockDeleter = playerData.isUnlockBascet;
    //    _isShowGuide = playerData.isShowGuide;
    //    SetIcon(playerData.icon);
    //    SetName(playerData.name);
    //    _money = playerData.money;
    //    _shopDistributor.LoadUp(_BlockDeleterLevel, _isUnlockBlockDeleter);
    //    _levelLoader.LoadLevel(_level, _icon);

    //    for (int i = 0; i < _level - 1; i++)
    //    {
    //        //_scoreBar.LoadLevelScoreMax();
    //    }
    //}

    public void UpdateData()
    {
        _level = _levelLoader.Level;
    }

    private void OnStartGame()
    {
        ChangeCristall(0);
        ChangeMoney(0);
    }

    private void OnEndLevel()
    {
        bool isEndGame;

        if(!_levelLoader.isUnity)
            _leaderboard.OnGetLeaderboardEntries(CalculeitTotalScore());

        if (_level + 1 <= _levelLoader.MaxLevel)
        {
            _level++;
            isEndGame = false;
            ChangeCristall(_completeLevelReward);
            _levelLoader.EndLevelPanelOn();
        }
        else
        {
            _level = 0;
            isEndGame = true;
            _levelLoader.EndGamePanelOn();
        }

        _playerData.WriteDataPlayer(TotalScore, Level, Cristall, IsUnlockBascket, LevelUp, Icon, Money, IsShowGuide, Name, isEndGame);
    }

    private void OnGenerationStart()
    {
        _score = 0;
        _totalMoneyLevel = 0;
        _bounces = 0;
    }

    private int CalculeitTotalScore()
    {
        float bonus;
        _totalScore += _bounces + _score + _totalMoneyLevel;

        if (_levelLoader.TimeLevel >= _allTime)
            bonus = _totalScore * _partTime;
        else
            bonus = _totalScore * _levelLoader.TimeLevel / _allTime;

        _totalScore -= (int)bonus;

        return _totalScore;
    }
}

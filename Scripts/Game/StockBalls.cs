using System;
using System.Collections.Generic;
using UnityEngine;

public class StockBalls : MonoBehaviour
{
    [SerializeField] private ShopDistributor _shopDistributor;
    [SerializeField] private List<Color> _colorsLevel;
    [SerializeField] private List<int> _minBallLevel;
    [SerializeField] private List<int> _startPriceLevel;
    [SerializeField] private List<int> _priceUpLevel;
    [SerializeField] private int _numberStartBalls;
    [SerializeField] private BallMover _prefabBalloon;
    [SerializeField] private Gun _gun;
    [SerializeField] private Camera _camera;
    [SerializeField] private ShopCreate _shop;
    [SerializeField] private Buffer _buffer;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private int _startPriceBall = 40;
    [SerializeField] private TextPriceBlock _priceBallText;
    [SerializeField] private AudioCounter _audioCounter;
    [SerializeField] private AudioBar _audioBar;

    private int _currentLevel;
    private int _currentBalls = 0;
    private int _bufferBalls = 4;
    private int _priceUp = 10;
    private int _currentPriceBall;
    private List<BallMover> _balls = new List<BallMover>();
    private List<BallMover> _allBalls = new List<BallMover>();
    private int _idNumber = 0;
    private int _currentNumberBalls;

    public int CurrentPriceBall => _currentPriceBall;

    private void OnEnable()
    {
        _levelLoader.DeleteAll += OnDeleteAll;
        _levelLoader.GenerationStart += OnGenerationStart;
        _levelLoader.LevelUp += OnLevelUp;
        _levelLoader.SubLevelUp += OnSubLevel;
        _gun.StartGame += CreateBufferBalls;
        _gun.StartGame += CreateStartBalls;
        _gun.StartGame += OnGenerationStart;
    }

    private void OnDisable()
    {
        _levelLoader.DeleteAll -= OnDeleteAll;
        _levelLoader.GenerationStart -= OnGenerationStart;
        _levelLoader.LevelUp -= OnLevelUp;
        _levelLoader.SubLevelUp -= OnSubLevel;
        _gun.StartGame -= CreateBufferBalls;
        _gun.StartGame -= CreateStartBalls;
        _gun.StartGame -= OnGenerationStart;
    }

    private void Start()
    {
        _currentNumberBalls = _currentBalls;
        _currentLevel = _playerInfo.Level;
        _currentPriceBall = _startPriceBall;
        _priceBallText.ChangeText(_currentPriceBall);
    }

    public void BuyBalls()
    {
        if(_playerInfo.Money >= _currentPriceBall)
        {
            BallMover newBall = _shop.CreateBalloon(_prefabBalloon, transform, _currentLevel, false, out int profitability);
            newBall.Init(_gun, _camera, profitability, _colorsLevel[profitability - 1], _buffer, _idNumber, _audioCounter, _audioBar);
            _audioCounter.Subscribe(newBall.BallAudio);
            ChangeNextId();
            _balls.Add(newBall);
            _allBalls.Add(newBall);
            _playerInfo.ChangeMoney(-_currentPriceBall);
            _currentPriceBall += _priceUp;
            _priceBallText.ChangeText(_currentPriceBall);
            _gun.AddBalls(newBall);
        }
    }

    public void ReplaceBall(BallMover ball, BallMover newBall)
    {
        for (int i = 0; i < _balls.Count; i++)
        {
            if (ball.Id == _balls[i].Id)
            {
                _balls[i] = newBall;

                break;
            }
        }
    }

    private void OnLevelUp(int level)
    {
        _currentPriceBall = _startPriceLevel[_playerInfo.Level - 1];
        _priceUp = _priceUpLevel[_playerInfo.Level - 1];
        _currentLevel = _minBallLevel[level - 1];
        _priceBallText.ChangeText(_currentPriceBall);
        _currentBalls = 0;
    }

    private void OnSubLevel()
    {
        _currentLevel++;
    }

    private void OnGenerationStart()
    {
        _currentNumberBalls = _numberStartBalls;
        ChangePriceBall();
        CreateStartBalls();
        CreateBufferBalls();
    }

    private void CreateBufferBalls()
    {
        _currentLevel = _playerInfo.Level;
        ChangePriceBall();

        for (int i = 0; i < 5; i++)
        {
            for(int x = 0; x < _bufferBalls; x++)
            {
                BallMover newBall = _shop.CreateBalloon(_prefabBalloon, _buffer.transform, _currentLevel + i, true, out int profitability);
                newBall.Init(_gun, _camera, profitability, _colorsLevel[profitability - 1], _buffer, _idNumber, _audioCounter, _audioBar);
                _audioCounter.Subscribe(newBall.BallAudio);
                ChangeNextId();
                _allBalls.Add(newBall);
                _buffer.AddBufferBalls(newBall);
            }
        }
    }

    private void CreateStartBalls()
    {
        ChangePriceBall();

        if(_currentBalls < _currentNumberBalls)
        {
            for (int i = 0; i < _currentNumberBalls; i++)
            {
                BallMover newBall = _shop.CreateBalloon(_prefabBalloon, transform, _currentLevel, false, out int profitability);
                newBall.Init(_gun, _camera, profitability, _colorsLevel[profitability - 1], _buffer, _idNumber, _audioCounter, _audioBar);
                _audioCounter.Subscribe(newBall.BallAudio);
                ChangeNextId();
                _currentBalls++;
                _allBalls.Add(newBall);
                _balls.Add(newBall);
                _gun.AddBalls(newBall);
            }
        }
    }

    private void ChangePriceBall()
    {
        _currentLevel = _playerInfo.Level;
        _currentPriceBall = _startPriceLevel[_playerInfo.Level - 1];
        _priceUp = _priceUpLevel[_playerInfo.Level - 1];
        _priceBallText.ChangeText(_currentPriceBall);
    }

    private void OnDeleteAll()
    {
        foreach(BallMover ball in _allBalls)
        {
            if(ball != null)
            {
                _audioCounter.Unsubscribe(ball.BallAudio);
                ball.BallAudio.Unsubscribe();
                ball.BallAudio.StopAudioPlay();
                Destroy(ball.gameObject);
            }
        }

        _balls.Clear();
        _allBalls.Clear();
    }

    private void ChangeNextId()
    {
        _idNumber += 1;
    }
}
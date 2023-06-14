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
    [SerializeField] private Ball _prefabBalloon;
    [SerializeField] private Gun _gun;
    [SerializeField] private Camera _camera;
    [SerializeField] private Constructor _shop;
    [SerializeField] private Buffer _buffer;
    [SerializeField] private Game _game;
    [SerializeField] private int _startPriceBall = 40;
    [SerializeField] private TextPriceBlock _priceBallText;
    [SerializeField] private AudioCounter _audioCounter;
    [SerializeField] private AudioBar _audioBar;

    private int _currentLevel;
    private int _currantStartBalls = 0;
    private int _bufferBalls = 4;
    private int _priceUp = 10;
    private int _currentPriceBall;
    private List<Ball> _balls = new List<Ball>();
    private List<Ball> _allBalls = new List<Ball>();
    private int _idNumber = 0;

    public int CurrentPriceBall => _currentPriceBall;

    private void OnEnable()
    {
        _game.DeleteAll += OnDeleteAll;
        _game.GenerationStart += OnGenerationStart;
        _game.LevelUp += OnLevelUp;
        _game.SubLevelUp += OnSubLevel;
        _gun.StartGame += CreateBufferBalls;
        _gun.StartGame += CreateStartBalls;
        _gun.StartGame += OnGenerationStart;
    }

    private void OnDisable()
    {
        _game.DeleteAll -= OnDeleteAll;
        _game.GenerationStart -= OnGenerationStart;
        _game.LevelUp -= OnLevelUp;
        _game.SubLevelUp -= OnSubLevel;
        _gun.StartGame -= CreateBufferBalls;
        _gun.StartGame -= CreateStartBalls;
        _gun.StartGame -= OnGenerationStart;
    }

    private void Start()
    {
        _numberStartBalls = 4;
        _currentLevel = _game.Level;
        _currentPriceBall = _startPriceBall;
        _priceBallText.ChangeText(_currentPriceBall);
    }

    public void BuyBalls()
    {
        if(_game.Money >= _currentPriceBall)
        {
            Ball newBalloon = _shop.CreateBalloon(_prefabBalloon, transform, _currentLevel, false, out int profitability);
            newBalloon.Init(_gun, _camera, profitability, _colorsLevel[profitability - 1], _buffer, _idNumber, _audioCounter, _audioBar);
            _audioCounter.Subscribe(newBalloon);
            ChangeNextId();
            _balls.Add(newBalloon);
            _allBalls.Add(newBalloon);
            _game.ChangeMoney(-_currentPriceBall);
            _currentPriceBall += _priceUp;
            _priceBallText.ChangeText(_currentPriceBall);
            _gun.AddBalls(newBalloon);
        }
    }

    public void ReplaceBall(Ball ball, Ball newBall)
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
        _currentPriceBall = _startPriceLevel[_game.Level - 1];
        _priceUp = _priceUpLevel[_game.Level - 1];
        _currentLevel = _minBallLevel[level - 1];
        _priceBallText.ChangeText(_currentPriceBall);
        _currantStartBalls = 0;
    }

    private void OnSubLevel(int subLevel)
    {
        _currentLevel += subLevel;
    }

    private void OnGenerationStart()
    {
        _numberStartBalls = 4;
        ChangePriceBall();
        CreateStartBalls();
        CreateBufferBalls();
    }

    private void CreateBufferBalls()
    {
        _currentLevel = _game.Level;
        ChangePriceBall();

        for (int i = 0; i < 5; i++)
        {
            for(int x = 0; x < _bufferBalls; x++)
            {
                Ball newBalloon = _shop.CreateBalloon(_prefabBalloon, _buffer.transform, _currentLevel + i, true, out int profitability);
                newBalloon.Init(_gun, _camera, profitability, _colorsLevel[profitability - 1], _buffer, _idNumber, _audioCounter, _audioBar);
                _audioCounter.Subscribe(newBalloon);
                ChangeNextId();
                _allBalls.Add(newBalloon);
                _buffer.AddBufferBalls(newBalloon);
            }
        }
    }

    private void CreateStartBalls()
    {
        ChangePriceBall();

        if(_currantStartBalls < _numberStartBalls)
        {
            for (int i = 0; i < _numberStartBalls; i++)
            {
                Ball newBalloon = _shop.CreateBalloon(_prefabBalloon, transform, _currentLevel, false, out int profitability);
                newBalloon.Init(_gun, _camera, profitability, _colorsLevel[profitability - 1], _buffer, _idNumber, _audioCounter, _audioBar);
                _audioCounter.Subscribe(newBalloon);
                ChangeNextId();
                _currantStartBalls++;
                _allBalls.Add(newBalloon);
                _balls.Add(newBalloon);
                _gun.AddBalls(newBalloon);
            }
        }
    }

    private void ChangePriceBall()
    {
        _currentLevel = _game.Level;
        _currentPriceBall = _startPriceLevel[_game.Level - 1];
        _priceUp = _priceUpLevel[_game.Level - 1];
        _priceBallText.ChangeText(_currentPriceBall);
    }

    private void OnDeleteAll()
    {
        foreach(Ball ball in _allBalls)
        {
            if(ball != null)
            {
                _audioCounter.Unsubscribe(ball);
                ball.Unsubscribe();
                ball.StopPlay();
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
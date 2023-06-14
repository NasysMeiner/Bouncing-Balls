using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioCounter : MonoBehaviour
{
    [SerializeField] private int _playMusic = 5;
    [SerializeField] private Game _game;

    private int _currentMusic = 0;
    private bool _isStop = false;

    public bool IsStop => _isStop;

    private void OnEnable()
    {
        _game.LevelUp += OnLevelUp;
    }

    private void OnDisable()
    {
        _game.LevelUp -= OnLevelUp;
    }

    private void Update()
    {
        if (_currentMusic < _playMusic && _isStop)
            _isStop = false;
    }

    public void Subscribe(Ball ball)
    {
        ball.startMusic += PlusMusic;
        ball.endMusic += MinusMusic;
    }

    public void Unsubscribe(Ball ball)
    {
        ball.startMusic -= PlusMusic;
        ball.endMusic -= MinusMusic;
    }

    private void PlusMusic()
    {
        _currentMusic++;

        if(_currentMusic >= _playMusic)
            _isStop = true;
    }

    private void MinusMusic()
    {
        _currentMusic--;
    }

    private void OnLevelUp(int value)
    {
        _currentMusic = 0;
    }
}

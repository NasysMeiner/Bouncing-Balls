using System.Collections.Generic;
using UnityEngine;

public class Buffer : MonoBehaviour
{
    [SerializeField] private StockBalls _stockBalls;
    [SerializeField] private Game _game;

    private Queue<Ball> _ballsCurrentLevel = new();
    private Queue<Ball> _ballsNextLevel = new();
    private Queue<Ball> _ballsHighLevel1 = new();
    private Queue<Ball> _ballsHighLevel2 = new();
    private Queue<Ball> _ballsHighLevel3 = new();
    private int _currentLevel;

    private void OnEnable()
    {
        _game.DeleteAll += OnDeleteAll;
    }

    private void OnDisable()
    {
        _game.DeleteAll -= OnDeleteAll;
    }

    public Ball GetBall(Ball ball)
    {
        _currentLevel = _game.Level;
        Ball newBall;
        ChangePosition(ball);

        if (ball.Profitability == _currentLevel)
        {
            _ballsCurrentLevel.Enqueue(ball);
            newBall = _ballsCurrentLevel.Dequeue();
        }
        else if(ball.Profitability == _currentLevel + 1)
        {
            _ballsNextLevel.Enqueue(ball);
            newBall = _ballsNextLevel.Dequeue();
        }
        else if (ball.Profitability == _currentLevel + 2)
        {
            _ballsHighLevel1.Enqueue(ball);
            newBall = _ballsHighLevel1.Dequeue();
        }
        else if (ball.Profitability == _currentLevel + 3)
        {
            _ballsHighLevel2.Enqueue(ball);
            newBall = _ballsHighLevel2.Dequeue();
        }
        else if (ball.Profitability == _currentLevel + 4)
        {
            _ballsHighLevel3.Enqueue(ball);
            newBall = _ballsHighLevel3.Dequeue();
        }
        else
        {
            newBall = null;
        }

        _stockBalls.ReplaceBall(ball, newBall);

        return newBall;
    }

    public void AddBufferBalls(Ball ball)
    {
        _currentLevel = _game.Level;
        ChangePosition(ball);

        if (ball.Profitability == _currentLevel)
            _ballsCurrentLevel.Enqueue(ball);
        else if (ball.Profitability == _currentLevel + 1)
            _ballsNextLevel.Enqueue(ball);
        else if (ball.Profitability == _currentLevel + 2)
            _ballsHighLevel1.Enqueue(ball);
        else if(ball.Profitability == _currentLevel + 3)
            _ballsHighLevel2.Enqueue(ball);
        else if(ball.Profitability == _currentLevel + 4)
            _ballsHighLevel3.Enqueue(ball);
    }

    private void ChangePosition(Ball ball)
    {
        ball.transform.position = transform.position;
    }

    private void OnDeleteAll()
    {
        _ballsCurrentLevel.Clear();
        _ballsNextLevel.Clear();
        _ballsHighLevel1.Clear();
        _ballsHighLevel2.Clear();
        _ballsHighLevel3.Clear();
    }
}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    [SerializeField] private StockBalls _stock;
    [SerializeField] private float _timeToShot;
    [SerializeField] private float _force;
    [SerializeField] private float _rotation = 75;
    [SerializeField] private StockBalls _stockBalls;
    [SerializeField] private Buffer _buffer;
    [SerializeField] private float _forceFactor = 1;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private ScreenPosition _screenPosition;

    private Queue<BallMover> _balloons = new Queue<BallMover>();
    private Vector3 _direction;
    private Vector3 _currentPositionCell;
    private float _time = 0;
    private bool _isStop = true;
    private bool _isStartGame = false;
    private Cell _currentCell;

    public event UnityAction StartGame;

    private void OnEnable()
    {
        _levelLoader.LevelUp += OnLevelUp;
        _levelLoader.GenerationStart += OnGenerationStart;
        _levelLoader.DeleteAll += OnDeleteAll;
        _levelLoader.StartGame += OnStartGame;
        _screenPosition.ChangeScreenOrientation += OnChangeScrenOrientation;
    }

    private void OnDisable()
    {
        _levelLoader.LevelUp -= OnLevelUp;
        _levelLoader.GenerationStart -= OnGenerationStart;
        _levelLoader.DeleteAll -= OnDeleteAll;
        _levelLoader.StartGame -= OnStartGame;
        _screenPosition.ChangeScreenOrientation -= OnChangeScrenOrientation;
    }

    private void Update()
    {
        if (_isStop == false)
        {
            if (_time >= _timeToShot && _balloons.Count > 0 && _isStop == false)
            {
                Shoot();
            }

            _time += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out BallMover ball))
        {
            if (!ball.IsDestroyed())
                ball.BallAudio.StartPlayAudio();

            Rigidbody rbBall = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 normal = -collision.contacts[0].normal;
            Vector3 reflection = Vector3.Reflect(collision.relativeVelocity.normalized, normal);
            rbBall.velocity = reflection.normalized * _forceFactor;
        }
    }

    public void AddBalls(BallMover balloon)
    {
        _balloons.Enqueue(balloon);
        Vector3 newPosition = new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y, transform.GetChild(0).position.z);

        if (balloon != null)
            balloon.transform.position = newPosition;
    }

    public void ChangePosition(Cell cell, int leftOrRight)
    {
        int halfCircle = 180;
        _currentCell = cell;
        _currentPositionCell = new Vector3(cell.transform.position.x, cell.transform.position.y, transform.position.z);
        transform.position = _currentPositionCell;
        float finalRotation = Random.Range(-_rotation, _rotation);

        if (leftOrRight == 0)
            finalRotation = finalRotation + halfCircle;

        transform.eulerAngles = new Vector3(0, 0, finalRotation);
        _stockBalls.transform.position = transform.position;
        _buffer.transform.position = transform.position;

        if (_isStartGame == false)
        {
            _isStartGame = true;
            StartGame?.Invoke();
        }
    }

    private void OnChangeScrenOrientation()
    {
        if (_currentCell != null)
        {
            _currentPositionCell = new Vector3(_currentCell.transform.position.x, _currentCell.transform.position.y, transform.position.z);
            transform.position = _currentPositionCell;
            _buffer.transform.position = transform.position;
            _stockBalls.transform.position = transform.position;
        }
    }

    private void OnStartGame()
    {
        _isStop = false;
    }

    private void OnDeleteAll()
    {
        _balloons.Clear();
    }

    private void OnLevelUp(int level)
    {
        _isStop = true;
    }

    private void OnGenerationStart()
    {
        _isStop = false;
    }

    private void Shoot()
    {
        BallMover CurrentBalloon = _balloons.Dequeue();

        if (CurrentBalloon != null)
        {
            CurrentBalloon.transform.position = transform.GetChild(1).position;
            CurrentBalloon.Rigidbody.isKinematic = false;
            CurrentBalloon.ChangeStateOff();
            CurrentBalloon.Train.enabled = true;
            _direction = CalculeitDirection();
            CurrentBalloon.AddForceBalls(_direction * _force);
            _time = 0;
        }
    }

    private Vector3 CalculeitDirection()
    {
        return (transform.GetChild(1).position - transform.GetChild(0).position).normalized;
    }
}

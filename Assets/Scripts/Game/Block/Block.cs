using BouncingBalls;
using System;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
public class Block : MonoBehaviour, IInitializable
{
    [SerializeField] private ObjectType _objectType;
    [SerializeField] private float _forceFactor = 1;
    [SerializeField] private ScorePopupManager _textBlock;

    private Collider _collider;

    private Cell _currentCell;
    private PlayField _playField;
    private float _price;
    private int _currentFactor = 1;
    private float _currentForceFactore;
    private int _factor;
    private int _crisstalChance = 0;
    private ScreenPosition _screenPosition;
    private int _profitabilityCristall = 1;

    public Cell CurrentCell => _currentCell;
    public PlayField PlayField => _playField;
    public ObjectType ObjectType => _objectType;
    public float Price => _price;

    public event Action<int> OnInitialize;
    public event Action<BounceScoreData> OnScoreEarned;
    public event Action<Block> Deleted;
    public event Action Bounced;
    public event Action OnPostInitialize;

    private void OnDisable()
    {
        ClicabilityOn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        int maxChance = 100;

        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            if (!CurrentCell.IsStock)
            {
                BounceScoreData scoreData = new
                (
                    _currentFactor * ball.Profitability,
                    UnityEngine.Random.Range(0, maxChance) <= _crisstalChance ? _profitabilityCristall : 0,
                    collision.contacts[0].point
                );

                OnScoreEarned?.Invoke(scoreData);
            }

            Rigidbody rigidbodyBall = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 normalInSurface = -collision.contacts[0].normal;
            Vector3 reflection = Vector3.Reflect(collision.relativeVelocity.normalized, normalInSurface);
            rigidbodyBall.velocity = reflection.normalized * _forceFactor;

            Bounced?.Invoke();
        }
    }

    public void Initialize(int factor)
    {
        if (gameObject.TryGetComponent(out BoxCollider boxCollider))
            _collider = boxCollider;
        else
            _collider = GetComponent<MeshCollider>();

        _factor = factor;
        _currentFactor = factor;

        OnInitialize?.Invoke(_factor);
    }

    public void PostInitialize(PlayField playField, ScorePopupManager textBlock)
    {
        _playField = playField;
        _textBlock = textBlock;

        OnPostInitialize?.Invoke();
    }

    public void ChangeCell(Cell newCell)
    {
        if (_currentCell != null)
            _currentCell.ReleaseCell();

        _currentCell = newCell;
        _currentCell.TakeCell();

        ChangePosition(_currentCell.GetPointPosition());
    }

    public void DeleteBlock()
    {
        _currentCell.ReleaseCell();
        _currentCell = null;
        _screenPosition.ChangeScreenOrientation -= OnChangeScreenOrientation;

        Deleted?.Invoke(this);
    }

    public void ClicabilityOn()
    {
        if (_collider != null)
            _collider.enabled = true;
    }

    public void ClicabilityOff()
    {
        if (_collider != null)
            _collider.enabled = false;
    }

    private void ChangePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    private void StartBuffsRebound(int id, float value)
    {
        const int forceBuffId = 0;
        const int profitabilityBuffId = 1;
        const int cristallChanceBuffId = 3;

        switch (id)
        {
            case forceBuffId:
                if (value > 0)
                    _forceFactor = _forceFactor * value;
                else
                    _forceFactor = _currentForceFactore;
                break;

            case profitabilityBuffId:
                if (value > 0)
                    _currentFactor = _currentFactor * (int)value;
                else
                    _currentFactor = _factor;
                break;

            case cristallChanceBuffId:
                if (value > 0)
                    _crisstalChance += (int)value;
                else
                    _crisstalChance = 0;
                break;
        }

        if (_currentFactor <= 0)
            _currentFactor = _factor;
    }

    private void OnChangeScreenOrientation()
    {
        transform.position = new Vector3(_currentCell.transform.position.x, _currentCell.transform.position.y, transform.position.z);
    }
}

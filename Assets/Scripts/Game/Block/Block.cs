using BouncingBalls;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(Draggable))]
public class Block : MonoBehaviour
{
    [SerializeField] private ObjectType _objectType;
    [SerializeField] private float _forceFactor = 1;
    [SerializeField] private TMP_Text _text = null;
    [SerializeField] private TextBlock _textBlock;

    private Collider _collider;

    private Cell _currentCell;
    private Cells _cells;
    //private ScoreBar _scoreBar;
    private StockBlocks _stockBlocks;
    private Shop _items;
    private ShopDistributor _shopDistributor;
    private float _price;
    private int _currentFactor = 1;
    private float _currentForceFactore;
    private int _factor;
    private int _crisstalChance = 0;
    private bool _isPremium = false;
    private ScreenPosition _screenPosition;
    private int _profitabilityCristall = 1;

    //private PlayerInfo _playerInfo;

    public Cell CurrentCell => _currentCell;
    public Cells Cells => _cells;
    public ObjectType ObjectType => _objectType;
    public float Price => _price;

    public event Action<int> ScoreChanged;
    public event Action<int> CristallChanged;
    public event Action<Vector3> BouncedPosition;
    public event Action<Block> Deleted;
    public event Action Bounced;

    private void OnCollisionEnter(Collision collision)
    {
        int maxChance = 100;

        if (collision.gameObject.TryGetComponent(out BallMover ball))
        {
            if (!CurrentCell.IsStock)
            {
                int score = _currentFactor * ball.Profitability;

                Vector3 positionText = Camera.main.WorldToScreenPoint(collision.contacts[0].point);
                int randomNumber = UnityEngine.Random.Range(0, maxChance);

                _textBlock.ShowMoneyTextBlock(ball.Profitability, _currentFactor, positionText);

                if (randomNumber <= _crisstalChance)
                {
                    //_textBlock.ShowCristallTextBlock(positionText);
                    //_playerInfo.ChangeCristall(_profitabilityCristall);

                    CristallChanged?.Invoke(_profitabilityCristall);
                }

                //_playerInfo.ChangeBounces();
                //_scoreBar.ChangeScore(score);
                //_playerInfo.ChangeMoney(score);

                ScoreChanged?.Invoke(_currentFactor * ball.Profitability);
                BouncedPosition?.Invoke(collision.contacts[0].point);
            }

            if (!ball.IsDestroyed())
                ball.BallAudio.StartPlayAudio();

            Rigidbody rigidbodyBall = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 normalInSurface = -collision.contacts[0].normal;
            Vector3 reflection = Vector3.Reflect(collision.relativeVelocity.normalized, normalInSurface);
            rigidbodyBall.velocity = reflection.normalized * _forceFactor;

            Bounced?.Invoke();
        }
    }

    private void Start()
    {
        if (gameObject.TryGetComponent(out BoxCollider boxCollider))
            _collider = boxCollider;
        else
            _collider = GetComponent<MeshCollider>();
    }

    public void Initialize(int factor, float price)
    {
        _factor = factor;
        _currentFactor = factor;
        _price = price;
    }

    public void SetPremium()
    {
        _isPremium = true;

        //if (_isPremium == false)
        //    _levelLoader.AllFieldDeleting += DeleteBlock;
    }

    public void PostInitialize(Cells cells, Shop items, ShopDistributor shopDistributor, TextBlock textBlock, PlayerInfo playerInfo, ScreenPosition screenPosition)
    {
        _cells = cells;

        //_playerInfo = playerInfo;
        //_scoreBar = scoreBar;
        _textBlock = textBlock;

        _shopDistributor = shopDistributor;
        _shopDistributor.ChangeBuffs += StartBuffsRebound;

        _text.text = $"X{_currentFactor}";

        //_currentCell = newCell;

        _items = items;
        _items.Open += ClicabilityOff;
        _items.Close += ClicabilityOn;


        _isPremium = false;

        _screenPosition = screenPosition;
        _screenPosition.ChangeScreenOrientation += OnChangeScreenOrientation;

        //ChangePosition(newPosition);

        //transform.GetChild(0).GetComponent<Canvas>().worldCamera = camera;
    }

    public void ChangeCell(Cell newCell)
    {
        if(_currentCell != null)
            _currentCell.ReleaseCell();

        _currentCell = newCell;
        _currentCell.TakeCell();

        ChangePosition(_currentCell.GetPointPosition());
    }

    public void DeleteBlock()
    {
        _currentCell.ReleaseCell();

        _shopDistributor.ChangeBuffs -= StartBuffsRebound;
        _items.Open -= ClicabilityOn;
        _items.Close -= ClicabilityOff;
        _screenPosition.ChangeScreenOrientation -= OnChangeScreenOrientation;

        if (_isPremium == false)
        {
            //_levelLoader.AllFieldDeleting -= DeleteBlock;
            //_deleateField.CellBlock(_price);
        }

        Deleted?.Invoke(this);
    }

    private void ChangePosition(Vector3 newPosition)
    {
        transform.position = newPosition;

        if (_items.IsPlay)
            ClicabilityOff();
    }

    private void ClicabilityOn()
    {
        if (_currentCell.IsStock)
            _collider.enabled = true;
    }

    private void ClicabilityOff()
    {
        if (_currentCell.IsStock)
            _collider.enabled = false;
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

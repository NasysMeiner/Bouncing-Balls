using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
public class Block : MonoBehaviour
{
    [SerializeField] private float _forceFactor = 1;
    [SerializeField] private TMP_Text _text = null;
    [SerializeField] private TextBlock _textBlock;

    private Cell _currentCell;
    private Cells _cells;
    private LevelLoader _levelLoader;
    private ScoreBar _scoreBar;
    private DeleteField _deleateField;
    private StockBlocks _stockBlocks;
    private Shop _items;
    private ShopDistributor _shopDistributor;
    private float _price;
    private int _currentFactor = 1;
    private float _currentForceFactore;
    private int _factor;
    private int _crisstalChance = 0;
    private bool _isPremium;
    private bool _isStock;
    private ScreenPosition _screenPosition;
    private int _profitabilityCristall = 1;

    private PlayerInfo _playerInfo;

    public Cell CurrentCell => _currentCell;
    public Cells Cells => _cells;
    public DeleteField Delete => _deleateField;
    public LevelLoader Game => _levelLoader;
    public StockBlocks StockBlocks => _stockBlocks;
    public bool IsStock => _isStock;
    public ScreenPosition ScreenPosition => _screenPosition;

    private void OnCollisionEnter(Collision collision)
    {
        int maxChance = 100;

        if (collision.gameObject.TryGetComponent(out BallMover ball))
        {
            if (_isStock == false)
            {
                Vector3 positionText = Camera.main.WorldToScreenPoint(collision.contacts[0].point);
                int randomNuber = UnityEngine.Random.Range(0, maxChance);

                _textBlock.ShowMoneyTextBlock(ball.Profitability, _currentFactor, positionText);

                if (randomNuber <= _crisstalChance)
                {
                    _textBlock.ShowCristallTextBlock(positionText);
                    _playerInfo.ChangeCristall(_profitabilityCristall);
                }

                _playerInfo.ChangeBounces();
                _scoreBar.ChangeScore(_currentFactor * ball.Profitability);
                _playerInfo.ChangeMoney(_currentFactor * ball.Profitability);
            }

            if (!ball.IsDestroyed())
                ball.BallAudio.StartPlayAudio();

            Rigidbody rbBall = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 normal = -collision.contacts[0].normal;
            Vector3 reflection = Vector3.Reflect(collision.relativeVelocity.normalized, normal);
            rbBall.velocity = reflection.normalized * _forceFactor;
        }
    }

    public void Init(int factor, Vector3 newPosition, Cell newCell, Cells cells, Color color, Camera camera, LevelLoader game, DeleteField deleateField, StockBlocks stockBlocks, Shop items, float price, ShopDistributor shopDistributor, TextBlock textBlock, ScoreBar scoreBar, PlayerInfo playerInfo, ScreenPosition screenPosition, bool isPremium = false, int crisstalChance = 0)
    {
        _currentFactor = factor;
        _playerInfo = playerInfo;
        _scoreBar = scoreBar;
        _textBlock = textBlock;
        _crisstalChance = crisstalChance;
        _factor = factor;
        _shopDistributor = shopDistributor;
        _shopDistributor.ChangeBuffs += StartBuffsRebound;
        _price = price;
        _text.text = $"X{_currentFactor}";
        _cells = cells;
        _stockBlocks = stockBlocks;
        _deleateField = deleateField;
        _currentCell = newCell;
        _items = items;
        _items.Open += ClicabilityOff;
        _items.Close += ClicabilityOn;
        _currentCell.TakeCell(this);
        _isPremium = isPremium;
        _isStock = true;
        _levelLoader = game;
        _screenPosition = screenPosition;
        _screenPosition.ChangeScreenOrientation += OnChangeScreenOrientation;
        _currentForceFactore = _forceFactor;

        if (isPremium == false)
            _levelLoader.DeleteAll += DeleteBlock;

        ChangePosition(newPosition);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = color;
        transform.GetChild(0).GetComponent<Canvas>().worldCamera = camera;
    }

    public void ChangeCell(Cell newCell, Vector3 newPosition)
    {
        _currentCell = newCell;
        _currentCell.TakeCell(this);
        ChangePosition(newPosition);

        if (_currentCell.IsStock)
            _isStock = true;
        else
            _isStock = false;
    }

    public void DeleteBlock()
    {
        _stockBlocks.DeleteBlock(this);
        _currentCell.ReleaseCell();
        _shopDistributor.ChangeBuffs -= StartBuffsRebound;
        _items.Open -= ClicabilityOn;
        _items.Close -= ClicabilityOff;
        _screenPosition.ChangeScreenOrientation -= OnChangeScreenOrientation;

        if (_isPremium == false)
        {
            _levelLoader.DeleteAll -= DeleteBlock;
            _deleateField.CellBlock(_price);
        }

        Destroy(gameObject);
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
        {
            if (gameObject.TryGetComponent<BoxCollider>(out BoxCollider Collider))
            {
                Collider.enabled = true;
            }
            else
            {
                MeshCollider MeshCollider = GetComponent<MeshCollider>();
                MeshCollider.enabled = true;
            }
        }
    }

    private void ClicabilityOff()
    {
        if (_currentCell.IsStock)
        {
            if (gameObject.TryGetComponent<BoxCollider>(out BoxCollider Collider))
            {
                Collider.enabled = false;
            }
            else
            {
                MeshCollider MeshCollider = GetComponent<MeshCollider>();
                MeshCollider.enabled = false;
            }
        }
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
                    _currentFactor = _currentFactor * Convert.ToInt32(value);
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

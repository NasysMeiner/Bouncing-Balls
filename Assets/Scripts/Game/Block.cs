using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
public class Block : MonoBehaviour
{
    [SerializeField] private float _forceFactor = 1;
    [SerializeField] private TMP_Text _text = null;

    private Cell _currentCell;
    private BufferTexts _bufferText;
    private Cells _cells;
    private Game _game;
    private DeleateField _deleateField;
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

    public Cell CurrentCell => _currentCell;
    public Cells Cells => _cells;
    public DeleateField Delete => _deleateField;
    public Game Game => _game;
    public StockBlocks StockBlocks => _stockBlocks;
    public bool IsStock => _isStock;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball balloon))
        {
            if(_isStock == false)
            {
                int randomNuber = UnityEngine.Random.Range(0, 100);
                Vector3 Change = new Vector3(UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(-20f, 20f), 0);
                TextBall textCristall = null;
                TextBall textMoney = _bufferText.TryGetText();

                if (randomNuber <= _crisstalChance)
                    textCristall = _bufferText.TryGetText(1);

                if (textMoney != null)
                {
                    textMoney.transform.position = Camera.main.WorldToScreenPoint(collision.contacts[0].point);
                    textMoney.transform.position += Change;
                    textMoney.ChangeText(balloon.Profitability, _currentFactor);
                    textMoney.ChangeActiveText(true);
                }

                if (textCristall != null)
                {
                    Change = new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), 0);
                    textCristall.transform.position = Camera.main.WorldToScreenPoint(collision.contacts[0].point);
                    textCristall.transform.position += Change;
                    textCristall.ChangeActiveText(true);
                    _game.ChangeCristall(1);
                }

                _game.AddBounce();
                _game.ChangeScore(_currentFactor * balloon.Profitability);
                _game.ChangeMoney(_currentFactor * balloon.Profitability);
            }

            if(!balloon.IsDestroyed())
                balloon.StartCoroutine("PlayAudio");

            Rigidbody rbBall = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 normal = -collision.contacts[0].normal;
            Vector3 reflection = Vector3.Reflect(collision.relativeVelocity.normalized, normal);
            rbBall.velocity = reflection.normalized * _forceFactor;
        }
    }

    public void Init(int factor, Vector3 newPosition, Cell newCell, Cells cells, Color color, Camera camera, BufferTexts bufferTexts, Game game, DeleateField deleateField, StockBlocks stockBlocks, Shop items, float price, ShopDistributor shopDistributor, bool isPremium = false, int crisstalChance = 0)
    {
        _currentFactor = factor;
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
        _items.Open += CheckClickability;
        _items.Close += CheckClickability;
        _currentCell.TakeCell(this);
        _bufferText = bufferTexts;
        _isPremium = isPremium;
        _isStock = true;
        _game = game;
        _game.ChangeScreenOrientation += OnChangeScreenOrientation;
        _currentForceFactore = _forceFactor;

        if(isPremium == false)
            _game.DeleteAll += DeleteBlock;

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
        _items.Open -= CheckClickability;
        _items.Close -= CheckClickability;
        _game.ChangeScreenOrientation -= OnChangeScreenOrientation;

        if (_isPremium == false)
        {
            _game.DeleteAll -= DeleteBlock;
            _game.CellBlock(_price);
        }

        Destroy(gameObject);
    }

    private void ChangePosition(Vector3 newPosition)
    {
        transform.position = newPosition;

        if (_items.IsPlay)
            CheckClickability(false);
    }

    private void CheckClickability(bool IsActive)
    {
        if (_currentCell.IsStock)
        {
            if (gameObject.TryGetComponent<BoxCollider>(out BoxCollider Collider))
            {
                Collider.enabled = IsActive;
            }
            else
            {
                MeshCollider MeshCollider = GetComponent<MeshCollider>();
                MeshCollider.enabled = IsActive;
            }
        }
    }

    private void StartBuffsRebound(int id, float value)
    {
        switch(id)
        {
            case 0:
                if (value > 0)
                    _forceFactor = _forceFactor * value;
                else
                    _forceFactor = _currentForceFactore;
                break;

            case 1:
                if (value > 0)
                    _currentFactor = _currentFactor * Convert.ToInt32(value);
                else
                    _currentFactor = _factor;
                break;

            case 3:
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
        transform.position = new Vector3(_currentCell.transform.position.x, _currentCell.transform.position.y,transform.position.z);
    }
}

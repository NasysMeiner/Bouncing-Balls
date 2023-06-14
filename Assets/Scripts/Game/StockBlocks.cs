using System;
using System.Collections.Generic;
using UnityEngine;

public class StockBlocks : MonoBehaviour 
{
    [SerializeField] private List<Color> _colorsLevel;
    [SerializeField] private List<Block> _prefabsBlock;
    [SerializeField] private List<int> _minBlocksLevel;
    [SerializeField] private List<int> _startPriceLevel;
    [SerializeField] private List<int> _priceUpLevel;
    [SerializeField] private int _startBlock = 1;
    [SerializeField] private int _cellCountX = 4;
    [SerializeField] private int _cellCountY = 1;
    [SerializeField] private int _startPriceBlock = 50;
    [SerializeField] private TextPriceBlock _priceBlockText;
    [SerializeField] private Cell _prefabCell;
    [SerializeField] private Cells _totalNumberCells;
    [SerializeField] private Constructor _shop;
    [SerializeField] private Camera _camera;
    [SerializeField] private BufferTexts _bufferTexts;
    [SerializeField] private Game _game;
    [SerializeField] private DeleateField _deleateField;
    [SerializeField] private Shop _items;
    [SerializeField] private ShopDistributor _shopDistributor;
    [SerializeField] private float _currentPriceStockUp = 15;
    [SerializeField] private GameObject _spawn;
    
    private float _distanceBetweenBlocks = 0.6f;
    private List<Block> _blocks = new List<Block>();
    private int _priceUp = 20;
    private int _currentPriceBlock;
    private Transform _transform;
    private List<Cell> _cellsStock = new List<Cell>();
    private bool _purchaseOpportunity;
    private int _currentLevel;
    private int _maxLevelPremium = 13;
    private bool _isPremium = false;
    private Block _timeBlock;

    public float CurrentPrice => _currentPriceStockUp;
    public List<Block> Blocks => _blocks;
    public int CurrentPriceBlock => _currentPriceBlock;

    private void OnDisable()
    {
        _shopDistributor.ChangeBuffs -= CreateTimeBlock;
        _game.GenerationStart -= OnGenerationStart;
        _game.LevelUp -= OnLevelUp;
        _game.StartGame -= OnStartGame;
        _game.SubLevelUp -= OnSubLevelUp;
    }

    private void Start()
    {
        _currentLevel = _game.Level;
        _transform = GetComponent<Transform>();
        _currentPriceBlock = _startPriceBlock;
        _priceBlockText.ChangeText(_currentPriceBlock);

        _shopDistributor.ChangeBuffs += CreateTimeBlock;
        _game.GenerationStart += OnGenerationStart;
        _game.LevelUp += OnLevelUp;
        _game.StartGame += OnStartGame;
        _game.SubLevelUp += OnSubLevelUp;
    }

    public void DeleteBlock(Block block)
    {
        List<Block> newListBlocks = new List<Block>();

        foreach (Block currentBlock in _blocks)
        {
            if (currentBlock == block)
                continue;

            newListBlocks.Add(currentBlock);
        }
        
        _blocks = newListBlocks;
    }

    public void ChangeCurrentPrice(float value)
    {
        _currentPriceStockUp += value;
    }

    public Cell TryEmptyCell()
    {
        foreach (var cell in _cellsStock)
        {
            if (cell.IsBusy == false)
                return cell;
        }

        return null;
    }

    public void BuyBlock(float time)
    {
        Cell emptyCell = TryEmptyCell();
        int level = _currentLevel;
        float value = _game.Money;
        float price = _currentPriceBlock;
        int cristallChance = 0;
        bool isPremium = false;

        if (emptyCell != null)
            _purchaseOpportunity = true;

        if (_isPremium)
        {
            level = _maxLevelPremium;
            value = _game.Cristall;
            price = 0;
            cristallChance = 30;
            isPremium = true;
            _isPremium = false;
        }
        else if(_game.Money >= price)
        {
            _game.ChangeMoney(-_currentPriceBlock);
            _currentPriceBlock += _priceUp;
            _priceBlockText.ChangeText(_currentPriceBlock);
        }

        if (_purchaseOpportunity && value > price)
        {
            Block newBlock =  CreateBlock(emptyCell, new Vector3(emptyCell.transform.position.x, emptyCell.transform.position.y, 0), level, cristallChance, isPremium);
            AddBlock(newBlock);

            if(time > 0)
                _timeBlock = newBlock;

            _purchaseOpportunity = false;
        }
    }

    private void OnStartGame()
    {
        _currentLevel = _game.Level;
        ChangePriceBlock();
        InitCells();
    }

    private void ChangePriceBlock()
    {
        _currentPriceBlock = _startPriceLevel[_game.Level - 1];
        _priceUp = _priceUpLevel[_game.Level - 1];
        _priceBlockText.ChangeText(_currentPriceBlock);
    }

    private void OnLevelUp(int level)
    {
        ChangePriceBlock();
        _currentLevel = _minBlocksLevel[level - 1];
    }

    private void OnSubLevelUp(int subLevel)
    {
        _currentLevel += subLevel;
    }

    private Block CreateBlock(Cell emptyCell, Vector3 positionCell, int level, int cristallChance = 0, bool isPremium = false)
    {
        Block newBlock = _shop.CreateBlocks(_prefabsBlock[UnityEngine.Random.Range(0, _prefabsBlock.Count)], level, out int factor);
        newBlock.Init(factor, positionCell, emptyCell, _totalNumberCells, _colorsLevel[factor - 1], _camera, _bufferTexts, _game, _deleateField, this, _items, _currentPriceBlock, _shopDistributor, isPremium, cristallChance);

        return newBlock;
    }

    private void InitCells()
    {
        int currentNumberStartBlocks = _startBlock;
        int cellCountX = _cellCountX;
        int cellCountY = _cellCountY;

        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            cellCountX = _cellCountY;
            cellCountY = _cellCountX;
        }

        for (int y = 0; y < cellCountY; y++)
        {
            for(int x = 0; x < cellCountX; x++)
            {
                Vector3 positionCell = _transform.position + new Vector3(x * _distanceBetweenBlocks, y * _distanceBetweenBlocks, 0);
                Cell newCell = Instantiate(_prefabCell, transform);
                _cellsStock.Add(newCell);
                _totalNumberCells.AddCell(newCell);

                if (currentNumberStartBlocks > 0)
                {
                    newCell.SpawnCell(positionCell, true);
                    AddBlock(CreateBlock(newCell, positionCell, _currentLevel, 0, false));
                    currentNumberStartBlocks--;
                }
                else
                {
                    newCell.SpawnCell(positionCell, false);
                }
            }
        }
    }

    private void OnGenerationStart()
    {
        int startBlock = _startBlock + 1;
        _currentLevel = _game.Level;

        if(startBlock > 8)
        {
            startBlock = 8;
        }

        for (int i = 0; i < startBlock; i++)
        {
            Cell emptyCell = TryEmptyCell();

            if (emptyCell != null)
                AddBlock(CreateBlock(emptyCell, new Vector3(emptyCell.transform.position.x, emptyCell.transform.position.y, 0), _currentLevel, 0, false));
            else
                return;
        }
    }

    private void AddBlock(Block block)
    {
        _blocks.Add(block);
    }

    private void CreateTimeBlock(int id, float value)
    {
        if(id == 4 && value > 0)
        {
            _isPremium= true;
            BuyBlock(value);
        }
        else if(id == 4)
        {
            _timeBlock.DeleteBlock();
        }
    }
}
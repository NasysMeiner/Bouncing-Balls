using System;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    [SerializeField] private List<ShopLevelData> _blocksLevelData = new();
    [SerializeField] private List<ShopLevelData> _ballsLevelData = new();

    private Bank _bank;
    private PlayField _playField;
    private GameManager _gameManager;
    private BallManager _ballManager;
    private BlockManager _blockManager;

    public int CurrentPriceBlock => _blocksLevelData[_gameManager.CurrentLevel].Price;

    public event Action<int> OnBlockPriceIncreased;
    public event Action<int> OnBallPriceIncreased;

    private void Awake()
    {
        _ballManager = GetComponentInChildren<BallManager>();
        _blockManager = GetComponentInChildren<BlockManager>();
    }

    public void Initialize(Bank bank, PlayField playField, GameManager gameManager)
    {
        _bank = bank;
        _playField = playField;
        _gameManager = gameManager;
    }

    public void CreateStartPullObjects(int level, int countBlock, int countBall)
    {
        _blockManager.CreateStartBlocks(_blocksLevelData[level].MinLevel, countBlock);
        _ballManager.CreateStartBalls(_ballsLevelData[level].MinLevel, countBall);
    }

    public void UpdatePrice(int currentLevel)
    {
        OnBlockPriceIncreased?.Invoke(_blocksLevelData[currentLevel].Price);
        OnBallPriceIncreased?.Invoke(_ballsLevelData[currentLevel].Price);
    }

    public void BuyBlock(int currentLevel)
    {
        if (_playField.TryEmptyCell(out Cell _) && TryPayObject(_blocksLevelData, currentLevel, out ShopLevelData shopLevelData))
        {
            _blockManager.CreateBlock(shopLevelData.MinLevel);
            OnBlockPriceIncreased?.Invoke(shopLevelData.Price);
        }
    }

    public void BuyBall(int currentLevel)
    {
        if (TryPayObject(_ballsLevelData, currentLevel, out ShopLevelData shopLevelData))
        {
            _ballManager.CreateBall(shopLevelData.MinLevel);
            OnBallPriceIncreased?.Invoke(shopLevelData.Price);
        }
    }

    private bool TryPayObject(List<ShopLevelData> shopLevelsData, int currentLevel, out ShopLevelData shopLevelData)
    {
        shopLevelData = shopLevelsData[currentLevel];

        if (_bank.TryPay(shopLevelData.Price))
        {
            shopLevelData.Price += shopLevelData.PriceUp;
            return true;
        }

        return false;
    }
}

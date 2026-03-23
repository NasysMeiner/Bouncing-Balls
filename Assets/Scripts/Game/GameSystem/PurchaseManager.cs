using System;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    [SerializeField] private List<ShopLevelData> _blocksLevelData = new();
    [SerializeField] private List<ShopLevelData> _ballsLevelData = new();

    private Bank _bank;
    private PlayField _playField;
    private BallManager _stockBalls;
    private BlockManager _stockBlocks;

    public event Action<int> OnBlockPriceIncreased;
    public event Action<int> OnBallPriceIncreased;

    private void Awake()
    {
        _stockBalls = GetComponentInChildren<BallManager>();
        _stockBlocks = GetComponentInChildren<BlockManager>();
    }

    public void Initialize(Bank bank, PlayField playField)
    {
        _bank = bank;
        _playField = playField;
    }

    public void CreateStartPullObjects(int level, int countBlock, int countBall)
    {
        _stockBlocks.CreateStartBlocks(_blocksLevelData[level].MinLevel, countBlock);
        _stockBalls.CreateStartBalls(_ballsLevelData[level].MinLevel, countBall);
    }

    public void UpdatePrice(int currentLevel)
    {
        OnBlockPriceIncreased?.Invoke(_blocksLevelData[currentLevel].StartPrice);
        OnBallPriceIncreased?.Invoke(_ballsLevelData[currentLevel].StartPrice);
    }

    public void BuyBlock(int currentLevel)
    {
        if (_playField.TryEmptyCell(out Cell _) && TryPayObject(_blocksLevelData, currentLevel, out ShopLevelData shopLevelData))
        {
            _stockBlocks.CreateBlock(shopLevelData.MinLevel);
            OnBlockPriceIncreased?.Invoke(shopLevelData.StartPrice);
        }
    }

    public void BuyBall(int currentLevel)
    {
        if (TryPayObject(_ballsLevelData, currentLevel, out ShopLevelData shopLevelData))
        {
            _stockBalls.CreateBall(shopLevelData.MinLevel);
            OnBallPriceIncreased?.Invoke(shopLevelData.StartPrice);
        }
    }

    private bool TryPayObject(List<ShopLevelData> shopLevelsData, int currentLevel, out ShopLevelData shopLevelData)
    {
        shopLevelData = shopLevelsData[currentLevel];

        if (_bank.TryPay(shopLevelData.StartPrice))
        {
            shopLevelData.StartPrice += shopLevelData.PriceUp;
            return true;
        }

        return false;
    }
}

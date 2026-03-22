using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    [SerializeField] private List<ShopLevelData> _blocksLevelData = new();
    [SerializeField] private List<ShopLevelData> _ballsLevelData = new();

    private StockBalls _stockBalls;
    private StockBlocks _stockBlocks;

    private void Awake()
    {
        _stockBalls = GetComponentInChildren<StockBalls>();
        _stockBlocks = GetComponentInChildren<StockBlocks>();
    }

    public void BuyBlock(int currentLevel)
    {
        //Bank -> AddBlock
    }

    public void BuyBall(int currentLevel)
    {
        //Bank -> AddBall
    }
}

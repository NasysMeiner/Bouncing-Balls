using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] private int _startMoney;
    [SerializeField] private int _startCristall;

    private int _cristall = 0;
    private int _money = 0;

    private int _totalMoneyLevel = 0;

    private StockBlocks _stockBlocks;

    private void OnDestroy()
    {
        if (_stockBlocks != null)
        {
            _stockBlocks.OnBlockAdded -= OnBlockAdded;
            _stockBlocks.OnBlockDeleted -= OnBlockDeleted;
        }
    }

    public void InitEvent(StockBlocks stockBlocks)
    {
        _stockBlocks = stockBlocks;

        _stockBlocks.OnBlockAdded += OnBlockAdded;
        _stockBlocks.OnBlockDeleted += OnBlockDeleted;
    }

    public void AddMoney(int money)
    {
        _money += money;
        _totalMoneyLevel += money;
    }

    public void AddCristall(int cristall)
    {
        _cristall += cristall;
    }

    private void OnBlockAdded(Block block)
    {
        block.ScoreChanged += AddMoney;
        block.CristallChanged += AddCristall;
    }

    private void OnBlockDeleted(Block block)
    {
        block.ScoreChanged -= AddMoney;
        block.CristallChanged -= AddCristall;
    }
}

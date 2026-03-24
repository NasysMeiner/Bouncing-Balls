using System;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] private int _startMoney;
    [SerializeField] private int _startCristall;

    private int _money = 0;
    private int _cristall = 0;

    private int _totalMoneyLevel = 0;

    private BlockManager _stockBlocks;

    public int TotalMoney => _totalMoneyLevel;

    public event Action<int> OnChangedMoney;
    public event Action<int> OnChangedCristall;

    private void OnDestroy()
    {
        if (_stockBlocks != null)
        {
            _stockBlocks.OnBlockAdded -= OnBlockAdded;
            _stockBlocks.OnBlockRemoved -= OnBlockDeleted;
        }
    }

    public void InitEvent(BlockManager stockBlocks)
    {
        AddMoney(_startMoney);
        AddCristall(_startCristall);

        _stockBlocks = stockBlocks;

        _stockBlocks.OnBlockAdded += OnBlockAdded;
        _stockBlocks.OnBlockRemoved += OnBlockDeleted;
    }

    public bool TryPay(int price, bool isCristall = false)
    {
        if(price <= (isCristall ? _cristall : _money))
        {
            if (isCristall)
                AddCristall(-price);
            else
                AddMoney(-price);

            return true;
        }

        return false;
    }

    public void AddMoney(int money)
    {
        _money += money;
        _totalMoneyLevel += money;

        OnChangedMoney?.Invoke(_money);
    }

    public void AddCristall(int cristall)
    {
        _cristall += cristall;

        OnChangedCristall?.Invoke(_cristall);
    }

    private void OnBlockAdded(Block block)
    {
        block.OnScoreEarned += OnScoreEarned;
    }

    private void OnBlockDeleted(Block block)
    {
        block.OnScoreEarned -= OnScoreEarned;
    }

    private void OnScoreEarned(BounceScoreData bounceScoreData)
    {
        if (bounceScoreData.Score > 0)
            AddMoney(bounceScoreData.Score);

        if (bounceScoreData.Cristall > 0)
            AddCristall(bounceScoreData.Cristall);
    }
}

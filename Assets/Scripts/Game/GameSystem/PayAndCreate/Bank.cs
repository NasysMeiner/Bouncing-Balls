using BouncingBalls.Block;
using BouncingBalls.Data;
using System;
using UnityEngine;

namespace BouncingBalls.GameSystem
{
    public class Bank : MonoBehaviour
    {
        [SerializeField] private int _startMoney;
        [SerializeField] private int _startCristall;

        private int _money = 0;
        private int _cristall = 0;

        private int _totalMoneyLevel = 0;

        private BlockManager _stockBlocks;

        public event Action<int> ChangedMoney;
        public event Action<int> ChangedCristall;

        public int TotalMoney => _totalMoneyLevel;
        public int Cristall => _cristall;

        private void OnDestroy()
        {
            if (_stockBlocks != null)
            {
                _stockBlocks.BlockAdded -= OnBlockAdded;
                _stockBlocks.BlockRemoved -= OnBlockDeleted;
            }
        }

        public void InitEvent(BlockManager stockBlocks)
        {
            AddMoney(_startMoney);
            AddCristall(_startCristall);

            _stockBlocks = stockBlocks;

            _stockBlocks.BlockAdded += OnBlockAdded;
            _stockBlocks.BlockRemoved += OnBlockDeleted;
        }

        public void SetCristall(int cristall)
        {
            _cristall = cristall;
        }

        public bool TryPay(int price, bool isCristall = false)
        {
            if (price <= (isCristall ? _cristall : _money))
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

            ChangedMoney?.Invoke(_money);
        }

        public void AddCristall(int cristall)
        {
            _cristall += cristall;

            ChangedCristall?.Invoke(_cristall);
        }

        private void OnBlockAdded(Block.Block block)
        {
            block.ScoreEarned += OnScoreEarned;
        }

        private void OnBlockDeleted(Block.Block block)
        {
            block.ScoreEarned -= OnScoreEarned;
        }

        private void OnScoreEarned(BounceScoreData bounceScoreData)
        {
            if (bounceScoreData.Score > 0)
                AddMoney(bounceScoreData.Score);

            if (bounceScoreData.Cristall > 0)
                AddCristall(bounceScoreData.Cristall);
        }
    }
}
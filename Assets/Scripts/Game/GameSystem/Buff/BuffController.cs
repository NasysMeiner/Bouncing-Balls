using BouncingBalls.Block;
using BouncingBalls.LevelSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls.GameSystem
{
    public class BuffController : MonoBehaviour
    {
        [SerializeField] private bool _useCrystalCurrency = true;

        [Header("BuffPrices")]
        [SerializeField] private int _profitabilityBuffCrystalPrice = 10;
        [Space]
        [SerializeField] private int _forceBounceBuffCrystalPrice = 10;
        [Space]
        [SerializeField] private int _cristallChanceBuffCrystalPrice = 10;
        [Space]
        [SerializeField] private int _timeBlockCrystalPrice = 10;
        [Space]
        [SerializeField] private int _blockDeleterPrice = 30;
        [Space]
        [SerializeField] private List<int> _pricesUpBlockDeleter = new();

        private int _currentLevelBlockDeleter = 0;

        private GameManager _gameManager;
        private Bank _bank;
        private BlockBuffManager _blockBuffManager;
        private BlockDeleter _blockDeleter;

        public int ProfitabilityBuffCrystalPrice => _profitabilityBuffCrystalPrice;
        public int ForceBounceBuffCrystalPrice => _forceBounceBuffCrystalPrice;
        public int CristallChanceBuffCrystalPrice => _cristallChanceBuffCrystalPrice;
        public int TimeBlockCrystalPrice => _timeBlockCrystalPrice;
        public int BlockDeleterPrice => _blockDeleterPrice;
        public bool IsMaxLevelBlockDeleter => _currentLevelBlockDeleter >= _pricesUpBlockDeleter.Count;
        public int CurrentPriceLevelUpBlockDeleter => _pricesUpBlockDeleter[_blockDeleter.CurrentLevelBlockDeleter];
        public bool IsUnlockBlockDeleter => _blockDeleter.IsUnlock;

        public void Initialize(Bank bank,
            GameManager gameManager, BlockDeleter blockDeleter,
            BlockBuffManager blockBuffManager)
        {
            _bank = bank;
            _gameManager = gameManager;
            _blockDeleter = blockDeleter;
            _blockBuffManager = blockBuffManager;
        }

        public bool TryProfitabilityBuff(out float timeCoolDown) =>
            TryActivateBuff(!_blockBuffManager.ProfitabilityBuffIsActive, _profitabilityBuffCrystalPrice,
                _blockBuffManager.StartBuffProfitability, out timeCoolDown);

        public bool TryForceBounceBuff(out float timeCoolDown) =>
            TryActivateBuff(!_blockBuffManager.ForceBounceBuffIsActive, _forceBounceBuffCrystalPrice,
                _blockBuffManager.StartBuffForceBounce, out timeCoolDown);

        public bool TryCristallChanceBuff(out float timeCoolDown) =>
            TryActivateBuff(!_blockBuffManager.CristallChanceBuffIsActive, _cristallChanceBuffCrystalPrice,
                _blockBuffManager.StartBuffCristallChance, out timeCoolDown);

        public bool TryTimeBlockCreate(out float timeCoolDown) =>
            TryActivateBuff(!_blockBuffManager.TimeBlockIsActive, _timeBlockCrystalPrice,
                () => _blockBuffManager.CreateTimeMaxLevelBlock(_gameManager.MaxLevelInBorder), out timeCoolDown);

        public bool TryBlockDeleterUnlock()
        {
            if (!_blockDeleter.IsUnlock && TryPay(_blockDeleterPrice))
            {
                _blockDeleter.Unlock();
                return true;
            }
            return false;
        }

        public bool TryBlockDeleterUp()
        {
            if (_blockDeleter.IsUnlock && TryPay(_pricesUpBlockDeleter[_currentLevelBlockDeleter]))
            {
                _blockDeleter.SetNewCoeffSell(++_currentLevelBlockDeleter);
                return true;
            }
            return false;
        }

        private bool TryActivateBuff(bool condition, int price, Func<float> action, out float timeCoolDown)
        {
            timeCoolDown = 0;

            if (condition && TryPay(price))
            {
                timeCoolDown = action();
                return true;
            }

            return false;
        }

        private bool TryPay(int price)
        {
            return _bank != null && _bank.TryPay(price, _useCrystalCurrency);
        }
    }
}
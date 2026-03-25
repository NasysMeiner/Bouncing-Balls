using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls
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
        private BlockManager _blockManager;
        private BlockDeleter _blockDeleter;

        public int ProfitabilityBuffCrystalPrice => _profitabilityBuffCrystalPrice;
        public int ForceBounceBuffCrystalPrice => _forceBounceBuffCrystalPrice;
        public int CristallChanceBuffCrystalPrice => _cristallChanceBuffCrystalPrice;
        public int TimeBlockCrystalPrice => _timeBlockCrystalPrice;
        public int BlockDeleterPrice => _blockDeleterPrice;
        public bool IsMaxLevelBlockDeleter => _currentLevelBlockDeleter >= _pricesUpBlockDeleter.Count;
        public int CurrentPriceLevelUpBlockDeleter => _pricesUpBlockDeleter[_blockDeleter.CurrentLevelBlockDeleter];
        public bool IsUnlockBlockDeleter => _blockDeleter.IsUnlock;

        public void Initialize(Bank bank, BlockManager blockManager, GameManager gameManager, BlockDeleter blockDeleter)
        {
            _bank = bank;
            _blockManager = blockManager;
            _gameManager = gameManager;
            _blockDeleter = blockDeleter;
        }

        public bool TryProfitabilityBuff(out float timeCoolDown)
        {
            timeCoolDown = 0;

            if (!_blockManager.ProfitabilityBuffIsActive && _bank.TryPay(_profitabilityBuffCrystalPrice, _useCrystalCurrency))
            {
                timeCoolDown = _blockManager.StartBuffProfitability();
                return true;
            }

            return false;
        }

        public bool TryForceBounceBuff(out float timeCoolDown)
        {
            timeCoolDown = 0;

            if (!_blockManager.ForceBounceBuffIsActive && _bank.TryPay(_forceBounceBuffCrystalPrice, _useCrystalCurrency))
            {
                timeCoolDown = _blockManager.StartBuffForceBounce();
                return true;
            }

            return false;
        }

        public bool TryCristallChanceBuff(out float timeCoolDown)
        {
            timeCoolDown = 0;

            if (!_blockManager.CristallChanceBuffIsActive && _bank.TryPay(_cristallChanceBuffCrystalPrice, _useCrystalCurrency))
            {
                timeCoolDown = _blockManager.StartBuffCristallChance();
                return true;
            }

            return false;
        }

        public bool TryTimeBlockCreate(out float timeCoolDown)
        {
            timeCoolDown = 0;

            if (!_blockManager.TimeBlockIsActive && _bank.TryPay(_timeBlockCrystalPrice, _useCrystalCurrency))
            {
                timeCoolDown = _blockManager.CreateTimeMaxLevelBlock(_gameManager.MaxLevelInBorder);
                return true;
            }

            return false;
        }

        public bool TryBlockDeleterUnlock()
        {
            if (!_blockDeleter.IsUnlock && _bank.TryPay(_blockDeleterPrice, _useCrystalCurrency))
            {
                _blockDeleter.Unlock();
                return true;
            }

            return false;
        }

        public bool TryBlockDeleterUp()
        {
            if (_blockDeleter.IsUnlock && _bank.TryPay(_pricesUpBlockDeleter[_currentLevelBlockDeleter], _useCrystalCurrency))
            {
                _blockDeleter.SetNewCoeffSell(++_currentLevelBlockDeleter);
                return true;
            }

            return false;
        }
    }
}
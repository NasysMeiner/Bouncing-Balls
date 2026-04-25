using BouncingBalls.GameSystem;
using BouncingBalls.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls.Block
{
    public class BlockDeleter : MonoBehaviour
    {
        [SerializeField] private float _zAddPosition = 0.5f;
        [SerializeField] private int _startLevelBlockDeleter = 0;
        [SerializeField] private List<float> _coeffSellPerLevel = new();

        private Bank _bank;
        private PurchaseManager _purchaseManager;
        private BlockManager _blockManager;

        private float _currentCoeffSell;
        private bool _isUnlock = false;

        public bool IsUnlock => _isUnlock;
        public int CurrentLevelBlockDeleter { get; private set; }

        private void OnDestroy()
        {
            _blockManager.BlockRemoved -= OnBlockRemoved;
        }

        public void Initialize(Bank bank, PurchaseManager purchaseManager, BlockManager blockManager)
        {
            _bank = bank;
            _purchaseManager = purchaseManager;
            _blockManager = blockManager;

            _blockManager.BlockRemoved += OnBlockRemoved;

            SetNewCoeffSell(_startLevelBlockDeleter);
        }

        public void Unlock()
        {
            if (_isUnlock)
                return;

            _isUnlock = true;
            StartCoroutine(UnlockAnimation());
        }

        public void CellBlock(Block block)
        {
            block.ChangeCell(null);
            _bank.AddMoney((int)(_purchaseManager.CurrentPriceBlock * _currentCoeffSell));
            PoolManager.Instance.SetObject(block, block.ObjectType);
        }

        public void SetNewCoeffSell(int newLevel)
        {
            if (newLevel >= _coeffSellPerLevel.Count)
                return;

            CurrentLevelBlockDeleter = newLevel;

            _currentCoeffSell = _coeffSellPerLevel[newLevel];
        }

        private void OnBlockRemoved(Block block)
        {
            CellBlock(block);
        }

        private IEnumerator UnlockAnimation()
        {
            Vector3 target = new(transform.position.x, transform.position.y, transform.position.z - _zAddPosition);
            bool isWork = true;
            int speedAnimation = 10;

            while (isWork)
            {
                if (transform.position == target)
                    isWork = false;

                transform.position = Vector3.MoveTowards(transform.position, target, speedAnimation * Time.deltaTime);

                yield return null;
            }
        }
    }
}
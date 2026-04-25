using BouncingBalls.Enums;
using System.Collections;
using UnityEngine;

namespace BouncingBalls.Block
{
    public class BlockBuffManager : MonoBehaviour
    {
        [Header("Profitability")]
        [SerializeField] private int _profitabilityValueBuff = 2;
        [SerializeField] private float _timeBuffProfitability = 5f;
        [SerializeField] private float _coolDownProfitability = 2f;

        [Header("ForceBounce")]
        [SerializeField] private int _forceBounceValueBuff = 2;
        [SerializeField] private float _timeBuffForceBounce = 5f;
        [SerializeField] private float _coolDownForceBounce = 2f;

        [Header("CrystalChance")]
        [Range(0, 100)]
        [SerializeField] private int _cristallChanceValue = 20;
        [SerializeField] private float _timeBuffCristallChance = 5f;
        [SerializeField] private float _coolDownCristallChance = 2f;

        [Header("TimeBlock")]
        [SerializeField] private float _timeMaxLevelBlock = 5f;
        [SerializeField] private float _coolDownMaxLevelBlock = 2f;

        private BlockManager _blockManager;
        private Block _timeBlock;

        public bool ProfitabilityBuffIsActive { get; private set; }
        public bool ForceBounceBuffIsActive { get; private set; }
        public bool CristallChanceBuffIsActive { get; private set; }
        public bool TimeBlockIsActive { get; private set; }

        private void OnEnable()
        {
            if (_blockManager != null)
            {
                _blockManager.BlockAdded += OnBlockAdded;
                _blockManager.BlockRemoved += OnBlockRemoved;
            }
        }

        private void OnDisable()
        {
            if (_blockManager != null)
            {
                _blockManager.BlockAdded -= OnBlockAdded;
                _blockManager.BlockRemoved -= OnBlockRemoved;
            }
        }

        public void Initialize(BlockManager blockManager)
        {
            _blockManager = blockManager;
        }

        private void OnBlockAdded(Block block)
        {
            if (block == null) return;
            if (ProfitabilityBuffIsActive) block.ChangeMultiplayProfitability(_profitabilityValueBuff);
            if (ForceBounceBuffIsActive) block.ChangeForceBounce(_forceBounceValueBuff);
            if (CristallChanceBuffIsActive) block.ChangeCristallChance(_cristallChanceValue);
        }

        private void OnBlockRemoved(Block block)
        {
            if (block == null) return;
            if (ProfitabilityBuffIsActive) block.ChangeMultiplayProfitability(1);
            if (ForceBounceBuffIsActive) block.ChangeForceBounce(-_forceBounceValueBuff);
            if (CristallChanceBuffIsActive) block.ChangeCristallChance(-_cristallChanceValue);
        }

        public float StartBuffProfitability()
        {
            StartCoroutine(ActivateBuff(_timeBuffProfitability, _profitabilityValueBuff, BuffType.Profitability));
            return _timeBuffProfitability + _coolDownProfitability;
        }

        public float StartBuffForceBounce()
        {
            StartCoroutine(ActivateBuff(_timeBuffForceBounce, _forceBounceValueBuff, BuffType.ForceBounce));
            return _timeBuffForceBounce + _coolDownForceBounce;
        }

        public float StartBuffCristallChance()
        {
            StartCoroutine(ActivateBuff(_timeBuffCristallChance, _cristallChanceValue, BuffType.CristallChance));
            return _timeBuffCristallChance + _coolDownCristallChance;
        }

        public float CreateTimeMaxLevelBlock(int level)
        {
            StartCoroutine(SpawnTimeBlock(_timeMaxLevelBlock, level));
            return _timeMaxLevelBlock + _coolDownMaxLevelBlock;
        }

        public void ResetAllBuff()
        {
            StopAllCoroutines();
            RevertActiveBuffs();

            ProfitabilityBuffIsActive = false;
            ForceBounceBuffIsActive = false;
            CristallChanceBuffIsActive = false;
            TimeBlockIsActive = false;

            if (_timeBlock != null)
                _blockManager.RemoveBlock(_timeBlock);

            _timeBlock = null;
        }

        private void RevertActiveBuffs()
        {
            foreach (var block in _blockManager.ActiveBlocks)
            {
                if (block == null) continue;
                if (ProfitabilityBuffIsActive) block.ChangeMultiplayProfitability(1);
                if (ForceBounceBuffIsActive) block.ChangeForceBounce(-_forceBounceValueBuff);
                if (CristallChanceBuffIsActive) block.ChangeCristallChance(-_cristallChanceValue);
            }
        }

        private IEnumerator ActivateBuff(float duration, int value, BuffType type)
        {
            SetBuffState(type, true);

            foreach (var block in _blockManager.ActiveBlocks)
                ApplyBuffToBlock(block, type, value);

            yield return new WaitForSeconds(duration);

            foreach (var block in _blockManager.ActiveBlocks)
                RevertBuffOnBlock(block, type, value);

            SetBuffState(type, false);
        }

        private IEnumerator SpawnTimeBlock(float duration, int level)
        {
            TimeBlockIsActive = true;
            _timeBlock = _blockManager.CreateBlock(level);

            yield return new WaitForSeconds(duration);

            if (_timeBlock != null)
                _blockManager.RemoveBlock(_timeBlock);

            TimeBlockIsActive = false;
            _timeBlock = null;
        }

        private void SetBuffState(BuffType type, bool isActive)
        {
            switch (type)
            {
                case BuffType.Profitability: ProfitabilityBuffIsActive = isActive; break;
                case BuffType.ForceBounce: ForceBounceBuffIsActive = isActive; break;
                case BuffType.CristallChance: CristallChanceBuffIsActive = isActive; break;
            }
        }

        private void ApplyBuffToBlock(Block block, BuffType type, int value)
        {
            switch (type)
            {
                case BuffType.Profitability: block.ChangeMultiplayProfitability(value); break;
                case BuffType.ForceBounce: block.ChangeForceBounce(value); break;
                case BuffType.CristallChance: block.ChangeCristallChance(value); break;
            }
        }

        private void RevertBuffOnBlock(Block block, BuffType type, int value)
        {
            switch (type)
            {
                case BuffType.Profitability: block.ChangeMultiplayProfitability(1); break;
                case BuffType.ForceBounce: block.ChangeForceBounce(-value); break;
                case BuffType.CristallChance: block.ChangeCristallChance(-value); break;
            }
        }
    }
}

using BouncingBalls.GameSystem;
using BouncingBalls.LevelSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls.Block
{
    public class BlockManager : MonoBehaviour
    {
        [SerializeField] private Factory _factory;
        [SerializeField] private PlayField _playField;
        [Space]
        [SerializeField] private int _startBlock = 1;
        [SerializeField] private int _minBlockCount = 3;
        [Space]
        [Header("BuffSettings")]
        [Header("Profitability")]
        [SerializeField] private int _profitabilityValueBuff = 2;
        [SerializeField] private float _timeBuffProfitability = 5f;
        [SerializeField] private float _coolDownProfitability = 2f;
        [Space]
        [Header("ForceBounce")]
        [SerializeField] private int _forceBounceValueBuff = 2;
        [SerializeField] private float _timeBuffForceBounce = 5f;
        [SerializeField] private float _coolDownForceBounce = 2f;
        [Space]
        [Header("CristallChanceUp")]
        [Range(0, 100)]
        [SerializeField] private int _cristallChanceValue = 20;
        [SerializeField] private float _timeBuffCristallChance = 5f;
        [SerializeField] private float _coolDownCristallChance = 2f;
        [Header("CristallChanceUp")]
        [SerializeField] private float _timeMaxLevelBlock = 5f;
        [SerializeField] private float _coolDownMaxLevelBlock = 2f;

        private List<Block> _activeBlocks = new List<Block>();
        private Block _timeBlock;

        public event Action<Block> BlockAdded;
        public event Action<Block> BlockRemoved;

        public bool ProfitabilityBuffIsActive { get; private set; }
        public bool ForceBounceBuffIsActive { get; private set; }
        public bool CristallChanceBuffIsActive { get; private set; }
        public bool TimeBlockIsActive { get; private set; }

        public void Initialize(Factory factory, PlayField playField)
        {
            _factory = factory;
            _playField = playField;

            ResetAllBuff();
        }

        public void CreateStartBlocks(int level, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (!_playField.TryEmptyCell(out Cell emptyCell))
                    return;

                CreateBlock(level);
            }
        }

        public Block CreateBlock(int level)
        {
            if (!_playField.TryEmptyCell(out Cell emptyCell))
                return null;

            Block newBlock = _factory.GetRandomBlock(level);
            newBlock.PostInitialize(_playField);
            newBlock.ChangeCell(emptyCell);

            RegisterBlock(newBlock);

            newBlock.gameObject.SetActive(true);

            return newBlock;
        }

        public void OffAllClicability()
        {
            foreach (Block block in _activeBlocks)
                block.ClicabilityOff();
        }

        public void FullReset()
        {
            ResetAllBuff();

            if(_timeBlock != null)
                UnregisterBlock(_timeBlock);

            List<Block> deleteBlockList = new(_activeBlocks);

            foreach (Block block in deleteBlockList)
                UnregisterBlock(block);
        }

        public float StartBuffProfitability()
        {
            StartCoroutine(ActivateProfitabilityBuff(_timeBuffProfitability, _profitabilityValueBuff));

            return _timeBuffProfitability + _coolDownProfitability;
        }

        public float StartBuffForceBounce()
        {
            StartCoroutine(ActivateForceBounceBuff(_timeBuffForceBounce, _forceBounceValueBuff));

            return _timeBuffForceBounce + _coolDownForceBounce;
        }

        public float StartBuffCristallChance()
        {
            StartCoroutine(ActivateCristallChanceBuff(_timeBuffCristallChance, _cristallChanceValue));

            return _timeBuffCristallChance + _coolDownCristallChance;
        }

        public float CreateTimeMaxLevelBlock(int level)
        {
            StartCoroutine(CreateMaxLevelBlock(_timeMaxLevelBlock, level));

            return _timeMaxLevelBlock + _coolDownMaxLevelBlock;
        }

        private IEnumerator CreateMaxLevelBlock(float time, int level)
        {
            TimeBlockIsActive = true;

            _timeBlock = CreateBlock(level);

            yield return new WaitForSeconds(time);

            UnregisterBlock(_timeBlock);

            TimeBlockIsActive = false;
        }

        private IEnumerator ActivateCristallChanceBuff(float timeBuff, int buffValue)
        {
            CristallChanceBuffIsActive = true;

            List<Block> activeBlock = new(_activeBlocks);

            foreach (Block block in activeBlock)
                block.ChangeCristallChance(buffValue);

            yield return new WaitForSeconds(timeBuff);

            activeBlock = new(_activeBlocks);

            foreach (Block block in activeBlock)
                block.ChangeCristallChance(-buffValue);

            CristallChanceBuffIsActive = false;
        }

        private IEnumerator ActivateForceBounceBuff(float timeBuff, int buffValue)
        {
            ForceBounceBuffIsActive = true;

            List<Block> activeBlock = new(_activeBlocks);

            foreach (Block block in activeBlock)
                block.ChangeForceBounce(buffValue);

            yield return new WaitForSeconds(timeBuff);

            activeBlock = new(_activeBlocks);

            foreach (Block block in activeBlock)
                block.ChangeForceBounce(-buffValue);

            ForceBounceBuffIsActive = false;
        }

        private IEnumerator ActivateProfitabilityBuff(float timeBuff, int buffValue)
        {
            ProfitabilityBuffIsActive = true;

            foreach (Block block in _activeBlocks)
                block.ChangeMultiplayProfitability(buffValue);

            yield return new WaitForSeconds(timeBuff);

            foreach (Block block in _activeBlocks)
                block.ChangeMultiplayProfitability(1);

            ProfitabilityBuffIsActive = false;
        }

        private void ResetAllBuff()
        {
            StopAllCoroutines();
            ProfitabilityBuffIsActive = false;
            ForceBounceBuffIsActive = false;
            CristallChanceBuffIsActive = false;
            TimeBlockIsActive = false;
        }

        private void RegisterBlock(Block block)
        {
            block.Deleted += UnregisterBlock;
            _activeBlocks.Add(block);
            BlockAdded?.Invoke(block);
        }

        private void UnregisterBlock(Block block)
        {
            _activeBlocks.Remove(block);
            block.Deleted -= UnregisterBlock;
            BlockRemoved?.Invoke(block);
        }
    }
}
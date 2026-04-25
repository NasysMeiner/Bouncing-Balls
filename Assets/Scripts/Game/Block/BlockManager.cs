using BouncingBalls.GameSystem;
using BouncingBalls.LevelSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls.Block
{
    public class BlockManager : MonoBehaviour
    {
        [SerializeField] private int _startBlock = 1;
        [SerializeField] private int _minBlockCount = 3;

        private Factory _factory;
        private PlayField _playField;

        private List<Block> _activeBlocks = new();

        public event Action<Block> BlockAdded;
        public event Action<Block> BlockRemoved;

        public List<Block> ActiveBlocks => _activeBlocks;

        public void Initialize(Factory factory, PlayField playField)
        {
            _factory = factory;
            _playField = playField;
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
            List<Block> deleteBlockList = new(_activeBlocks);

            foreach (Block block in deleteBlockList)
                UnregisterBlock(block);
        }

        public void RemoveBlock(Block block)
        {
            if (block == null || !_activeBlocks.Contains(block)) return;
            UnregisterBlock(block);
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
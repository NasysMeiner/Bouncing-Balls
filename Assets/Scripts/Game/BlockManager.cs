using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour 
{
    [SerializeField] private Factory _factory;
    [SerializeField] private BlockDeleter _blockDeleter;
    [SerializeField] private PlayField _playField;
    [Space]
    [SerializeField] private int _startBlock = 1;
    [SerializeField] private ScorePopupManager _textBlock;
    
    private List<Block> _activeBlocks = new List<Block>();

    public event Action<Block> OnBlockAdded;
    public event Action<Block> OnBlockRemoved;

    public void Initialize(Factory factory, PlayField playField)
    {
        _factory = factory;
        _playField = playField;
    }

    public void CreateStartBlocks(int level, int count)
    {
        for(int i = 0; i < count; i++)
        {
            if (!_playField.TryEmptyCell(out Cell emptyCell))
                return;

            CreateBlock(level);
        }
    }

    public void CreateBlock(int level)
    {
        if (!_playField.TryEmptyCell(out Cell emptyCell))
            return;

        Block newBlock = _factory.GetRandomBlock(level);
        newBlock.PostInitialize(_playField.Cells, _textBlock);
        newBlock.ChangeCell(emptyCell);

        RegisterBlock(newBlock);

        newBlock.gameObject.SetActive(true);
    }

    public void OffAllClicability()
    {
        foreach(Block block in _activeBlocks)
            block.ClicabilityOff();
    }

    public void FullReset()
    {
        List<Block> deleteBlockList = new(_activeBlocks);

        foreach (Block block in deleteBlockList)
            UnregisterBlock(block);
    }

    private void RegisterBlock(Block block)
    {
        block.Deleted += UnregisterBlock;
        _activeBlocks.Add(block);
        OnBlockAdded?.Invoke(block);
    }

    private void UnregisterBlock(Block block)
    {
        _activeBlocks.Remove(block);
        block.Deleted -= UnregisterBlock;
        _blockDeleter.CellBlock(block);
        OnBlockRemoved?.Invoke(block);
    }
}
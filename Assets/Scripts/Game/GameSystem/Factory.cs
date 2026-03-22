using BouncingBalls;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private List<Color> _colorsLevel;
    [SerializeField] private List<ObjectType> _blockVariants = new();
    [Space]
    [Header("Random chance")]
    [SerializeField] private int _chanceNextLevel = 76;
    [SerializeField] private int _chanceMaxLevel = 97;

    private int _maxChance = 100;

    private int _maxLevel;

    public void InitFactory(int maxLevel)
    {
        _maxLevel = maxLevel;
    }

    public Block GetRandomBlock(int level, int price)
    {
        Block newBlock = PoolManager.Instance.GetObject<Block>(_blockVariants[Random.Range(0, _blockVariants.Count)]);

        int blockLevel = CreateRandomProfitability(level);

        newBlock.GetComponent<Renderer>().material.color = _colorsLevel[blockLevel];
        newBlock.Initialize(blockLevel, price);

        return newBlock;
    }

    private int CreateRandomProfitability(int currentLevel)
    {
        float randomBall = Random.Range(0, _maxChance);
        int profitability;

        if (randomBall >= _chanceMaxLevel && currentLevel + 2 <= _maxLevel)
            profitability = currentLevel + 2;
        else if (randomBall < _chanceMaxLevel && randomBall > _chanceNextLevel && currentLevel + 1 <= _maxLevel)
            profitability = currentLevel + 1;
        else
            profitability = currentLevel;

        return profitability;
    }
}

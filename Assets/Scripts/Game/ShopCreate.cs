using UnityEngine;

public class ShopCreate : MonoBehaviour
{
    [SerializeField] private Transform _spawn;

    private int _maxLevel = 13;
    private int _chanceMaxLevel = 97;
    private int _chanceNextLevel = 76;
    private int _maxChance = 100;

    public Block CreateBlocks(Block prefabBlock, int currentLevel, out int factor)
    {
        factor = CreateRandomProfitability(currentLevel);
        Block newBlock = Instantiate(prefabBlock, _spawn.transform);


        return newBlock;
    }

    public BallMover CreateBalloon(BallMover balloon, Transform transform, int currentLevel, bool isBuffer, out int profability)
    {
        if(isBuffer == false)
            profability = CreateRandomProfitability(currentLevel);
        else
            profability = currentLevel;

        BallMover newBalloon = Instantiate(balloon, transform);
        newBalloon.ChangeState(true);

        return newBalloon;
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

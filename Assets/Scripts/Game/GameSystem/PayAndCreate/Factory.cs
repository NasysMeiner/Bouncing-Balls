using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls
{
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

        public Block GetRandomBlock(int level)
        {
            return GetRandomObject<Block>(level, _blockVariants[Random.Range(0, _blockVariants.Count)]);
        }

        public Ball GetRandomBall(int level)
        {
            return GetRandomObject<Ball>(level, ObjectType.Ball);
        }

        private T GetRandomObject<T>(int level, ObjectType objectType) where T : MonoBehaviour, IInitializable
        {
            T newObject = PoolManager.Instance.GetObject<T>(objectType);

            int levelObject = CreateRandomProfitability(level);

            newObject.GetComponent<Renderer>().material.color = _colorsLevel[levelObject];
            newObject.Initialize(levelObject);

            return newObject;
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
}
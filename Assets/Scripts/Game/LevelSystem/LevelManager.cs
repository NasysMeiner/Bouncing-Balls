using BouncingBalls.Ball;
using BouncingBalls.Block;
using BouncingBalls.GameSystem;
using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls.LevelSystem
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<int> _startCountBlocks;
        [SerializeField] private List<int> _startCountBalls;

        private PlayField _playField;
        private PurchaseManager _purchaseManager;
        private BlockManager _blockManager;
        private BallManager _ballManager;
        private ScreenPosition _screenPosition;
        private Gun _gun;

        public void InitLevelManager(PlayField playField, Gun gun, ScreenPosition screenPosition,
            BlockManager blockManager, BallManager ballManager, PurchaseManager purchaseManager)
        {
            _playField = playField;
            _purchaseManager = purchaseManager;
            _gun = gun;
            _screenPosition = screenPosition;
            _blockManager = blockManager;
            _ballManager = ballManager;
        }

        public void GenerateLevel(int level)
        {
            _playField.GenerateField(level, _gun);
            _screenPosition.FixPositionField(level);
            _purchaseManager.CreateStartPullObjects(level, _startCountBlocks[level], _startCountBalls[level]);
            _purchaseManager.UpdatePrice(level);

            _gun.StartShoot();
        }

        public void StopGame()
        {
            _blockManager.OffAllClicability();
            _gun.StopShoot();
        }

        public void FullReset()
        {
            _gun.FullReset();
            _blockManager.FullReset();
            _ballManager.FullReset();
            _playField.FullReset();
        }
    }
}
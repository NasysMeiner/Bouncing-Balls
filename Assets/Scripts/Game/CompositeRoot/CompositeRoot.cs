using BouncingBalls.Ball;
using BouncingBalls.Block;
using BouncingBalls.Data;
using BouncingBalls.Enums;
using BouncingBalls.GameSystem;
using BouncingBalls.LevelSystem;
using BouncingBalls.View;
using BouncingBalls.WebSystem;
using UnityEngine;

namespace BouncingBalls.Composite
{
    public class CompositeRoot : MonoBehaviour, ICompositeRoot
    {
        [SerializeField] private int _maxLevel = 13;
        [Space]
        [SerializeField] private Gun _gun;
        [Space]
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private PlayField _playField;
        [SerializeField] private Factory _factory;
        [SerializeField] private Bank _bank;
        [SerializeField] private BuffController _buffController;
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private PurchaseManager _purchaseManager;
        [SerializeField] private BlockManager _blockManager;
        [SerializeField] private BlockBuffManager _blockBuffManager;
        [SerializeField] private BallManager _ballManager;
        [SerializeField] private BlockDeleter _blockDeleter;
        [SerializeField] private ScreenPosition _screenPosition;
        [Space]
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private ShopUI _shopUI;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private ScorePopupManager _scorePopupManager;
        [SerializeField] private IconManager _iconManager;
        [Space]
        [SerializeField] private SaveManager _saveManager;
        [SerializeField] private RewardedVideo _rewardedVideo;
        [SerializeField] private RewardedAnnouncement _announcement;
        [SerializeField] private Leaderboard _leaderboard;

        public void Compose()
        {
            InitSystem();
        }

        public void InitSystem()
        {
            _gameManager.Initialize(_levelManager, _uiManager, _scoreController);
            _purchaseManager.Initialize(_bank, _playField, _gameManager);
            _levelManager.InitLevelManager(_playField, _gun, _screenPosition, _blockManager,
                _ballManager, _purchaseManager, _blockBuffManager);
            _bank.InitEvent(_blockManager);
            _buffController.Initialize(_bank, _gameManager, _blockDeleter, _blockBuffManager);
            _scoreController.Initialize(_blockManager, _bank, _gameManager);
            _factory.InitFactory(_maxLevel);
            _blockManager.Initialize(_factory, _playField);
            _blockBuffManager.Initialize(_blockManager);
            _ballManager.Initialize(_factory, _gun);
            _blockDeleter.Initialize(_bank, _purchaseManager, _blockManager);

            _saveManager.Initialize(_gameManager, _blockDeleter, _uiManager, _bank, _iconManager);
            _rewardedVideo.Initialize(_bank);
            _announcement.Initialize(_gameManager);
            _leaderboard.Initialize(_gameManager, _uiManager);

            _saveManager.LoadData += InitLoadPlayerData;

            _saveManager.GetPlayerData();
        }

        private void InitLoadPlayerData(LoadType loadType, PlayerProgressData playerProgressData)
        {
            _saveManager.LoadData -= InitLoadPlayerData;

            if (loadType != LoadType.Failed)
            {
                _gameManager.SetLevel(playerProgressData.Level);
                _bank.SetCristall(playerProgressData.Cristall);
                _blockDeleter.SetNewCoeffSell(playerProgressData.LevelBlockDeleter);

                if (playerProgressData.IsUnlockBlockDeleter)
                    _blockDeleter.Unlock();

                _uiManager.SetPlayerData(playerProgressData.Name, playerProgressData.IconId, playerProgressData.IsShowGuide);
            }

            InitViewSystem();
        }

        private void InitViewSystem()
        {
            _uiManager.Initialize(_gameManager, _purchaseManager, _bank, _iconManager);
            _shopUI.Initialize(_buffController, _gameManager);
            _scoreView.Initialize(_scoreController);
            _scorePopupManager.Initialize(_blockManager);
            _iconManager.Initialize();
        }
    }
}
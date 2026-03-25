using UnityEngine;

namespace BouncingBalls
{
    public class CompositeRootInitSystem : CompositeRoot
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

        public override void Compose()
        {
            InitSystem();
        }

        public void InitSystem()
        {
            _gameManager.Initialize(_levelManager, _uiManager, _scoreController);
            _purchaseManager.Initialize(_bank, _playField, _gameManager);
            _levelManager.InitLevelManager(_playField, _gun, _screenPosition, _blockManager, _ballManager, _purchaseManager);
            _bank.InitEvent(_blockManager);
            _buffController.Initialize(_bank, _blockManager, _gameManager, _blockDeleter);
            _scoreController.Initialize(_blockManager, _bank, _gameManager);
            _factory.InitFactory(_maxLevel);
            _blockManager.Initialize(_factory, _playField);
            _ballManager.Initialize(_factory, _gun);
            _blockDeleter.Initialize(_bank, _purchaseManager);

            _saveManager.Initialize(_gameManager, _blockDeleter, _uiManager, _bank, _iconManager);
            _rewardedVideo.Initialize(_bank);
            _announcement.Initialize(_gameManager);
            _leaderboard.Initialize(_gameManager, _uiManager);

            _saveManager.OnLoadData += InitLoadPlayerData;

            _saveManager.GetPlayerData();
        }

        private void InitLoadPlayerData(LoadType loadType, PlayerProgressData playerProgressData)
        {
            _saveManager.OnLoadData -= InitLoadPlayerData;

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
            _iconManager.Initialize(_uiManager);
        }
    }
}
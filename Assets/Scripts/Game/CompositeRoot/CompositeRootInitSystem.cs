using UnityEngine;

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
    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private PurchaseManager _purchaseManager;
    [SerializeField] private BlockManager _blockManager;
    [SerializeField] private BallManager _ballManager;
    [SerializeField] private ScreenPosition _screenPosition;
    [Space]
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private ScorePopupManager _scorePopupManager;
    
    public override void Compose()
    {
        InitSystem();
    }

    public void InitSystem()
    {
        _gameManager.InitManager(_levelManager, _uiManager, _scoreController);
        _purchaseManager.Initialize(_bank, _playField);
        _levelManager.InitLevelManager(_playField, _gun, _screenPosition, _blockManager, _ballManager, _purchaseManager);
        _bank.InitEvent(_blockManager);
        _scoreController.Initialize(_blockManager, _bank, _gameManager);
        _factory.InitFactory(_maxLevel);
        _blockManager.Initialize(_factory, _playField);
        _ballManager.Initialize(_factory, _gun);

        _uiManager.Initialize(_gameManager, _purchaseManager, _bank);
        _scoreView.Initialize(_scoreController);
        _scorePopupManager.Initialize(_blockManager);
    }
}

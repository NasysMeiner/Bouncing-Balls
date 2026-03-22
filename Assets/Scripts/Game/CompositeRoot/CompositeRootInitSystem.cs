using UnityEngine;

public class CompositeRootInitSystem : CompositeRoot
{
    [SerializeField] private int _maxLevel = 13;
    [Space]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private PlayField _playField;
    [SerializeField] private Factory _factory;
    [SerializeField] private Bank _bank;
    [SerializeField] private PurchaseManager _purchaseManager;
    [SerializeField] private StockBlocks _stockBlocks;
    [SerializeField] private StockBalls _stockBalls;
    

    public override void Compose()
    {
        InitSystem();
    }

    public void InitSystem()
    {
        _gameManager.InitManager(_levelManager);
        _levelManager.InitLevelManager(_playField);
        _bank.InitEvent(_stockBlocks);
        _factory.InitFactory(_maxLevel);
    }
}

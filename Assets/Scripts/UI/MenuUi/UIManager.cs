using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _startView;
    [SerializeField] private BankView _bankView;
    [SerializeField] private LevelStatView _endLevelView;

    [SerializeField] private TMP_Text _levelText;

    private GameManager _gameManager;
    private PurchaseManager _purchaseManager;

    public void Initialize(GameManager gameManager, PurchaseManager purchaseManager, Bank bank)
    {
        _gameManager = gameManager;
        _purchaseManager = purchaseManager;

        _bankView.Initialize(bank, purchaseManager);
    }

    public void StartGame()
    {
        _startView.SetActive(false);
        _gameManager.StartGame();

        _levelText.text = "Lv. " + (_gameManager.CurrentLevel + 1).ToString();
    }

    public void StartNextLevel()
    {
        _gameManager.StartNextLevel();
    }

    public void ViewEndLevelPanel(LevelData levelData)
    {
        _endLevelView.gameObject.SetActive(true);
        _endLevelView.SetLevelData(levelData);
    }

    public void BuyBlock()
    {
        _purchaseManager.BuyBlock(_gameManager.CurrentLevel);
    }

    public void BuyBall()
    {
        _purchaseManager.BuyBall(_gameManager.CurrentLevel);
    }
}

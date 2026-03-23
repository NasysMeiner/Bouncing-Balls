using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _startView;
    [SerializeField] private BankView _bankView;
    [SerializeField] private LevelStatView _endLevelView;
    [SerializeField] private LevelStatView _endGameView;

    [SerializeField] private TMP_Text _levelText;

    private GameManager _gameManager;
    private PurchaseManager _purchaseManager;

    private Image _iconPlayer;

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
        UpdateLevelView();
    }

    public void StartNextLevel()
    {
        _gameManager.StartNextLevel();
        UpdateLevelView();
    }

    public void RestartGame()
    {
        _gameManager.RestartGame();
    }

    public void ChangeIconPlayer(Image image)
    {
        _iconPlayer = image;
    }

    public void ViewEndLevelPanel(LevelData levelData)
    {
        LevelStatView currentView;

        if (_gameManager.CurrentLevel == _gameManager.MaxLevel - 1)
            currentView = _endGameView;
        else
            currentView = _endLevelView;

        currentView.gameObject.SetActive(true);
        currentView.SetLevelData(levelData);
    }

    public void BuyBlock()
    {
        _purchaseManager.BuyBlock(_gameManager.CurrentLevel);
    }

    public void BuyBall()
    {
        _purchaseManager.BuyBall(_gameManager.CurrentLevel);
    }

    public void PauseOn()
    {
        _gameManager.PauseOn();
    }

    public void PauseOff()
    {
        _gameManager.PauseOff();
    }

    private void UpdateLevelView()
    {
        _levelText.text = "Lv. " + (_gameManager.CurrentLevel + 1).ToString();
    }
}

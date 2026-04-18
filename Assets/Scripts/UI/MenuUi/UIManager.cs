using BouncingBalls.Data;
using BouncingBalls.GameSystem;
using BouncingBalls.LevelSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BouncingBalls.View
{
    public class UIManager : MonoBehaviour
    {
        [Header("StartView")]
        [SerializeField] private GameObject _startView;
        [SerializeField] private GameObject _startViewPlayerExist;
        [SerializeField] private GameObject _baseStartView;
        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private Image _playerIcon;
        [SerializeField] private Button _startButton;
        [Space]
        [SerializeField] private BankView _bankView;
        [SerializeField] private LevelStatView _endLevelView;
        [SerializeField] private LevelStatView _endGameView;
        [SerializeField] private GameObject _guideView;

        [SerializeField] private TMP_Text _levelText;

        private GameManager _gameManager;
        private PurchaseManager _purchaseManager;
        private IconManager _iconManager;

        private bool _isLoadPlayer = false;
        private string _playerName = "Anonymos";
        private int _iconIdPlayer = 0;
        private bool _isShowGuide = false;

        public bool IsShowGuide => _isShowGuide;
        public string PlayerName => _playerName;

        public void Initialize(GameManager gameManager, PurchaseManager purchaseManager, Bank bank, IconManager iconManager)
        {
            _gameManager = gameManager;
            _purchaseManager = purchaseManager;
            _iconManager = iconManager;
            _iconManager.SetIconFromId(_iconIdPlayer);

            _bankView.Initialize(bank, purchaseManager);

            if (!_isShowGuide)
            {
                _guideView.SetActive(true);
                _isShowGuide = true;
            }

            if (_isLoadPlayer)
            {
                _startViewPlayerExist.SetActive(true);
                _baseStartView.SetActive(false);
                _playerNameText.text = _playerName;
                _playerIcon.sprite = _iconManager.CurrentIcon.SpriteImage;
            }

            _startButton.interactable = true;
        }

        public void SetPlayerData(string namePlayer, int iconIdPlayer, bool isShowGuide)
        {
            _isLoadPlayer = true;
            _playerName = namePlayer;
            _iconIdPlayer = iconIdPlayer;
            _isShowGuide = isShowGuide;
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

        public void SetName(string name)
        {
            _playerName = name;
        }

        private void UpdateLevelView()
        {
            _levelText.text = "Lv. " + (_gameManager.CurrentLevel + 1).ToString();
        }
    }
}
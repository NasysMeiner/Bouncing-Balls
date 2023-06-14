using Agava.YandexGames;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Advertisement))]
public class Game : MonoBehaviour
{
    [SerializeField] private bool _isUnity;
    [SerializeField] private Bar _progressBar;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Balance _moneyBar;
    [SerializeField] private Balance _cristallBar;
    [SerializeField] private int _level = 1;
    [SerializeField] private int _maxLevel = 6;
    [SerializeField] private float _levelScore = 1000;
    [SerializeField] private float _nextLevelScoreUp = 1000;
    [SerializeField] private float _coefficientMaxScore = 1;
    [SerializeField] private float _coefficientMaxScoreUp = 1;
    [SerializeField] private float _coefficientCell = 0.5f;
    [SerializeField] private ShopDistributor _shopDistributor;
    [SerializeField] private EndLevelPanel _endLevelPanel;
    [SerializeField] private Animator _subLevelAnimator;
    [SerializeField] private GameObject _startGamePanel;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private List<Vector3> _cameraPositions;
    [SerializeField] private Camera _camera;
    [SerializeField] private List<Vector3> _playFieldPosition;
    [SerializeField] private PlayField _playField;
    [SerializeField] private List<Vector3> _stockBlocksPosition;
    [SerializeField] private StockBlocks _stockBlocks;
    [SerializeField] private List<Vector3> _deleteFieldPosition;
    [SerializeField] private DeleateField _deleateField;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _imageIcon;
    [SerializeField] private Icons _icons;
    [SerializeField] private int _gemsRewarded;
    [SerializeField] private GameObject _isAvtorizeOff;
    [SerializeField] private GameObject _isOn;
    [SerializeField] private Image _startIcon;
    [SerializeField] private TMP_Text _startName;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private GameObject _guidePanel;
    [SerializeField] private Panel _BG;
    [SerializeField] private Advertisement _advertisement;

    private float _money = 0;
    private float _totalMoneyLevel = 0;
    private bool _isFreezChangeMoney = false;
    private float _cristall = 0;
    private float _currentLevelScoreMax;
    private float _currentScore = 0;
    private float _partScore;
    private bool _isPartActive2 = false;
    private bool _isPartActive1 = false;
    private bool _isStart = false;
    private bool _isPressed = false;
    private int _bounces = 0;
    private int _totalScore = 0;
    private string _name;
    private ScreenOrientation _deviceOrientation1;
    private ScreenOrientation _deviceOrientation2;
    private int _numberPosition;
    private bool _isGorizontal;
    private float _time;
    private bool _isGame;

    public PlayerInfo playerInfo;

    public int Level => _level;
    public float Money => _money;
    public float TotalMoneyLevel => _totalMoneyLevel;
    public float Cristall => _cristall;
    public int TotalScore => _totalScore;
    public float Score => _currentScore;
    public int Bounces => _bounces;
    public Camera Camera => _camera;
    public List<Vector3> PlayFieldPosition => _playFieldPosition;
    public bool IsGorizontal => _isGorizontal;
    public float ScoreTime => _time;
    public string Name => _name;
    public Icons Icons => _icons;

    public event UnityAction<float> MoneyChanged;
    public event UnityAction<float> CristallChanged;
    public event UnityAction<int> LevelUp;
    public event UnityAction<int> SubLevelUp;
    public event UnityAction DeleteAll;
    public event UnityAction GenerationStart;
    public event UnityAction StartGame;
    public event UnityAction ChangeScreenOrientation;

    private void OnDisable()
    {
        _shopDistributor.ChangeBuffs -= UpgrateSell;
        _advertisement.onCloseAd -= ClearField;
    }

    private void Start()
    {
        int value = 0;
        Time.timeScale = 1;
        _levelText.text = $"lv. {_level}.1";
        _shopDistributor.ChangeBuffs += UpgrateSell;
        _currentLevelScoreMax = _levelScore;
        _partScore = CountParts();
        _advertisement.onCloseAd += ClearField;
        ChangeMoney(0);
        ChangeCristall(0);
        Screen.orientation = ScreenOrientation.AutoRotation;

        if (Screen.orientation == ScreenOrientation.Portrait && Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            _deviceOrientation1 = ScreenOrientation.LandscapeLeft;
            _deviceOrientation2 = ScreenOrientation.LandscapeRight;
            _stockBlocks.transform.eulerAngles += new Vector3(0, 0, -90);
            _numberPosition = 0;
        }
        else
        {
            _deviceOrientation1 = ScreenOrientation.Portrait;
            _deviceOrientation2 = ScreenOrientation.PortraitUpsideDown;
            _numberPosition = 1;
        }

        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            _isGorizontal = true;
            value = _level;
        }

        _camera.transform.position = _cameraPositions[_numberPosition - 1];
        _playField.transform.position = _playFieldPosition[_level - value];
        _stockBlocks.transform.position = _stockBlocksPosition[_numberPosition - 1 ];
        _deleateField.transform.position = _deleteFieldPosition[_numberPosition + 2 - 1];
    }

    private void Update()
    {
        int value = 0;
        Screen.orientation = ScreenOrientation.AutoRotation;

        if (Screen.orientation == _deviceOrientation1 || Screen.orientation == _deviceOrientation2)
        {

            if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                _isGorizontal = true;
                value = _level;
            }

            _camera.transform.position = _cameraPositions[_numberPosition];
            _playField.transform.position = _playFieldPosition[_level - value];
            _stockBlocks.transform.position = _stockBlocksPosition[_numberPosition];

            if (_deleateField.IsUnlock == false)
            {
                _numberPosition += 2;
            }

            _deleateField.transform.position = _deleteFieldPosition[_numberPosition];

            if (_deviceOrientation1 == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            {
                _deviceOrientation1 = ScreenOrientation.LandscapeLeft;
                _deviceOrientation2 = ScreenOrientation.LandscapeRight;
                _stockBlocks.transform.eulerAngles = new Vector3(0, 0, 270);
                _isGorizontal = false;
                _numberPosition = 0;
            }
            else
            {
                _deviceOrientation1 = ScreenOrientation.Portrait;
                _deviceOrientation2 = ScreenOrientation.PortraitUpsideDown;
                _stockBlocks.transform.eulerAngles += new Vector3(0, 0, 90);
                _numberPosition = 1;
            }

            ChangeScreenOrientation?.Invoke();
        }

        if (_deleateField.IsPlayAnimation && _deleateField.transform.position.z > 0.5f)
        {
            Vector3 target = new Vector3(_deleateField.transform.position.x, _deleateField.transform.position.y, _deleateField.transform.position.z - 0.5f);
            _deleateField.transform.position = Vector3.MoveTowards(_deleateField.transform.position, target, 10 * Time.deltaTime);
        }

        if(_isGame)
            _time += Time.deltaTime;
    }

    public void OnStartGame()
    {
        if (_isStart == false)
        {
            StartGame?.Invoke();
            _isStart = true;
            _startGamePanel.SetActive(false);
            _isGame = true;
            _levelText.text = $"lv. {_level}.1";
            ChangeCristall(0);

            if(_playerData.isShowGuide == false)
            {
                Time.timeScale = 0;
                _guidePanel.SetActive(true);
                _BG.gameObject.SetActive(true);
                _playerData.isShowGuide = true;
                OnSetCloudSaveDataButtonClick();
            }
            else
            {
                _BG.gameObject.SetActive(false);
            }
        }
    }

    public void ChangeName(string name)
    {
        _name = name;
        _playerData.name = name;
    }

    public void ShowAdvertisement()
    {
        if (!_isUnity)
            _advertisement.ShowAd();
        else
            ClearField(true);

        _totalMoneyLevel = 0;
        _currentScore = 0;
        _isFreezChangeMoney = false;
    }

    //public IEnumerator PressedButton(Button button)
    //{
    //    button.interactable = false;

    //    yield return new WaitForSeconds(5);

    //    button.interactable = true;
    //}

    public void ChangePause(int value)
    {
        Time.timeScale = value;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _endGamePanel.SetActive(false);
        _startGamePanel.SetActive(true);
    }

    public void AddBounce()
    {
        _bounces+= 1;
    }

    public void ChangeScore(float value)
    {
        _currentScore += value;
        _progressBar.ChangeBarLevel(_currentScore, _currentLevelScoreMax);

        if (_currentScore >= _currentLevelScoreMax && _isGame)
        {
            _isGame = false;
            EndLevel();
            return;
        }

        if (_currentScore >= _partScore * 2 && _isPartActive1 == false)
        {
            _isPartActive1 = true;
            SubLevelUp?.Invoke(1);
            _levelText.text = $"lv. {_level}.2";
            _subLevelAnimator.SetTrigger("SubLevel1");
        }
        else if (_currentScore >= _partScore * 5 && _isPartActive2 == false)
        {
            _isPartActive2 = true;
            SubLevelUp?.Invoke(2);
            _levelText.text = $"lv. {_level}.3";
            _subLevelAnimator.SetTrigger("SubLevel2");
        }
    }

    public void ChangeMoney(float value)
    {
        if (!_isFreezChangeMoney)
        {
            _money += value;
            _moneyBar.ChangeText(_money);
            MoneyChanged?.Invoke(_money);

            if (value > 0)
            {
                _totalMoneyLevel += value;       
            }
        }
    }

    public void LoadLevel(int level, int score, float cristall, string name, float money)
    {
        _level = level;
        _totalScore = score;
        _cristall = cristall;
        _name = name;
        _money = money;

        ChangeCristall(0);
        ChangeMoney(0);

        for(int i = 0; i < level - 1; i++)
        {
            ChangeMaxScoreValue();
        }
    }

    public void ChangeCristall(float value)
    {
        _cristall += value;
        _cristallBar.ChangeText(_cristall);
        CristallChanged?.Invoke(_cristall);
    }

    public void CellBlock(float price)
    {
        ChangeMoney(price * _coefficientCell);
    }

    //public void ChangeShowGuide()
    //{
    //    _playerData.isShowGuide = true;
    //}

    public void ClearField(bool value)
    {
        if (_isPressed == false)
        {
            _isPressed = true;
            _isGame = true;
            _bounces = 0;
            _currentScore = 0;
            _moneyBar.ChangeText(_money);
            _progressBar.ChangeBarLevel(_currentScore, _currentLevelScoreMax);
            _endLevelPanel.gameObject.SetActive(false);
            _BG.gameObject.SetActive(false);
            _levelText.text = $"lv. {_level}.1";
            GenerationStart?.Invoke();
            Time.timeScale = 1;
        }
    }

    public void OnGetCloudSaveDataButtonClick(string loadedString, int score, string name)
    {
        if (loadedString != "{}")
        {
            playerInfo = JsonUtility.FromJson<PlayerInfo>(loadedString);
            LoadPlayerData(playerInfo, score, name);
        }
    }

    private void UpgrateSell(int id, float value)
    {
        if(id == 5)
        {
            if (_coefficientCell == 0.7f)
                _coefficientCell = 1;
            else
                _coefficientCell += value;
        }
    }

    private void EndLevel()
    {
        float bonus;
        ChangeCristall(20);
        _totalScore += _bounces + (int)_currentScore + (int)_totalMoneyLevel;

        if (_time >= 1000)
            bonus = _totalScore * 0.99f;
        else
            bonus = _totalScore * _time / 1000;

        _totalScore -= (int)bonus;

        OnGetLeaderboardEntriesButtonClick(_totalScore);
        _playerData.score = _totalScore;
        _playerData.cristall = _cristall;
        _playerData.money = _money;

        _BG.gameObject.SetActive(true);
        _endLevelPanel.gameObject.SetActive(true);
        _isPressed = false;
        _isPartActive1 = false;
        _isPartActive2 = false;
        _subLevelAnimator.SetTrigger("EndLevel");
        ChangeLevel();
    }

    private void ChangeLevel()
    {
        if(_level + 1 <= _maxLevel)
        {
            _isFreezChangeMoney = true;
            _totalMoneyLevel = 0;
            _currentScore = 0;
            _time = 0;
            ChangeMaxScoreValue();
            _level += 1;
            _playerData.level = _level;
            DeleteAll?.Invoke();      
            LevelUp?.Invoke(_level);
            OnSetCloudSaveDataButtonClick();
        }
        else
        {
            EndGame();
        }
    }

    private void ChangeMaxScoreValue()
    {
        _currentLevelScoreMax = _currentLevelScoreMax / 2 + _nextLevelScoreUp * _coefficientMaxScore;
        _partScore = CountParts();
        _coefficientMaxScore += _coefficientMaxScoreUp / 2;
    }

    private void OnSetCloudSaveDataButtonClick(bool isEnd = false)
    {
        if(isEnd)
            _playerData.ResetData();

        RewritePlayerData();
        string jsonString = JsonUtility.ToJson(playerInfo);

        if(!_isUnity)
            PlayerAccount.SetCloudSaveData(jsonString);
    }

    private void RewritePlayerData()
    {
        playerInfo.level = _playerData.level;
        playerInfo.isUnlockBascket = _playerData.isUnlockBascket;
        playerInfo.levelUp = _playerData.levelUp;
        playerInfo.cristall = _playerData.cristall;
        playerInfo.name = _playerData.name;
        playerInfo.icon = _playerData.icon;
        playerInfo.money = _playerData.money;
        playerInfo.isShowGuide = _playerData.isShowGuide;
        playerInfo.score = _playerData.score;
    }

    private void EndGame()
    {
        Time.timeScale = 0;
        _nameText.text = _name;
        _imageIcon.sprite = _icons.IconsNew[_playerData.icon];
        _endGamePanel.SetActive(true);
        _playerData.ResetData();
        OnSetCloudSaveDataButtonClick(true);
        OnGetLeaderboardEntriesButtonClick(_totalScore);
    }

    private void OnGetLeaderboardEntriesButtonClick(int value)
    {
        if (!_isUnity)
        {
            if (PlayerAccount.IsAuthorized)
            {
                Leaderboard.GetPlayerEntry("NewLeaders", (result) =>
                {
                    if (result.score < value)
                    {
                        Leaderboard.SetScore("NewLeaders", value);
                    }
                });
            }
        }
    }

    private float CountParts()
    {
        return _currentLevelScoreMax * 0.1f;
    }

    private void LoadPlayerData(PlayerInfo playerInfo, int score, string name)
    {
        _playerData.score = score;
        _playerData.name = name;
        _playerData.level = playerInfo.level;
        _playerData.levelUp = playerInfo.levelUp;
        _playerData.cristall = playerInfo.cristall;
        _playerData.isUnlockBascket = playerInfo.isUnlockBascket;
        _playerData.icon = playerInfo.icon;
        _playerData.isShowGuide = playerInfo.isShowGuide;
        _playerData.money = playerInfo.money;
        _playerData.score = playerInfo.score;
        _playerData.LoadDataPlayer();
        _startIcon.sprite = Icons.IconsNew[_playerData.icon];
        _startName.text = _playerData.name;
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public int score;
        public int level;
        public string name;
        public float cristall;
        public bool isUnlockBascket;
        public int levelUp;
        public int icon;
        public float money;
        public bool isShowGuide;
    }
}

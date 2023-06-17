using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Advertisement))]
public class LevelLoader : MonoBehaviour
{
    [SerializeField] public bool isUnity;
    [SerializeField] private int _level = 1;
    [SerializeField] private int _maxLevel = 6;
    [SerializeField] private EndLevelPanel _endLevelPanel;
    [SerializeField] private Animator _subLevelAnimator;
    [SerializeField] private GameObject _startGamePanel;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private DeleteField _deleateField;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _imageIcon;
    [SerializeField] private Icons _icons;
    [SerializeField] private Image _startIcon;
    [SerializeField] private TMP_Text _startName;
    [SerializeField] private GameObject _guidePanel;
    [SerializeField] private Panel _BG;
    [SerializeField] private Advertisement _advertisement;
    [SerializeField] private ScoreBar _scoreBar;
    [SerializeField] private TextScoreBar _textScoreBar;
    [SerializeField] private PlayerInfo _playerInfo;

    private bool _isFreezChangeMoney = false;
    private float _time;
    private bool _isGame;
    private int _subLevel = 0;

    public float ScoreTime => _time;
    public bool IsFreezChangeMoney => _isFreezChangeMoney;
    public int SubLevel => _subLevel;
    public int Level => _level;
    public int MaxLevel => _maxLevel;
    public float TimeLevel => _time;

    public event UnityAction<int> LevelUp;
    public event UnityAction SubLevelUp;
    public event UnityAction DeleteAll;
    public event UnityAction GenerationStart;
    public event UnityAction StartGame;

    private void OnEnable()
    {
        _scoreBar.SubLevelUp += OnSubLevelUp;
        _scoreBar.EndLevel += OnEndLevel;
        _advertisement.OnCloseAd += ClearField;
    }

    private void OnDisable()
    {
        _scoreBar.SubLevelUp -= OnSubLevelUp;
        _scoreBar.EndLevel -= OnEndLevel;
        _advertisement.OnCloseAd -= ClearField;
    }

    private void Update()
    {
        if (_isGame)
            _time += Time.deltaTime;
    }

    public void OnStartGame()
    {
        bool isStart = false;

        if (isStart == false)
        {
            Time.timeScale = 1;
            isStart = true;
            _isGame = true;
            StartGame?.Invoke();
            _startGamePanel.SetActive(false);
            _subLevel = 1;
            _playerInfo.UpdateData();
            _textScoreBar.ChangeTextLevel(Level, SubLevel);

            if (_playerInfo.IsShowGuide == false)
            {
                Time.timeScale = 0;
                _guidePanel.SetActive(true);
                _BG.gameObject.SetActive(true);
                _playerInfo.ShowGuideOn();
            }
            else
            {
                _BG.gameObject.SetActive(false);
            }
        }
    }

    private void OnSubLevelUp(int newSubLevel)
    {
        _subLevel = newSubLevel;
        _textScoreBar.ChangeTextLevel(Level, SubLevel);
        _textScoreBar.SubLevelAnimation(SubLevel);
        SubLevelUp?.Invoke();
    }

    private void OnEndLevel()
    {
        if (_level + 1 <= _maxLevel)
        {
            _level++;
            _subLevel = 1;
            _BG.gameObject.SetActive(true);
            _isFreezChangeMoney = true;
            DeleteAll?.Invoke();
            LevelUp?.Invoke(_level);
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void EndLevelPanelOn()
    {
        _endLevelPanel.gameObject.SetActive(true);
    }

    public void EndGamePanelOn()
    {
        _endGamePanel.SetActive(true);
    }

    public void ShowAdvertisement()
    {
        if (!isUnity)
            _advertisement.ShowAd();
        else
            ClearField(true);
    }

    public void LoadLevel(int level, int icon)
    {
        _level = level;
        _subLevel = 1;
        _textScoreBar.ChangeTextLevel(_level, _subLevel);
    }

    public void SetName(string name)
    {
        _nameText.text = name;
        _startName.text = name;
    }

    public void SetIcon(int value)
    {
        _imageIcon.sprite = _icons.IconsNew[value];
        _startIcon.sprite = _icons.IconsNew[value];
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _endGamePanel.SetActive(false);
        _startGamePanel.SetActive(true);
    }

    public void ClearField(bool value)
    {
        _isGame = true;
        _endLevelPanel.gameObject.SetActive(false);
        _BG.gameObject.SetActive(false);
        GenerationStart?.Invoke();
        _isFreezChangeMoney = false;
        _textScoreBar.ChangeTextLevel(_level, _subLevel);
        _textScoreBar.SubLevelAnimation(_subLevel);
        Time.timeScale = 1;
    }
}

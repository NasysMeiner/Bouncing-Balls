using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] private int _levelScoreMax = 1000;
    [SerializeField] private int _nextLevelScoreUp = 1000;
    [SerializeField] private int _coefficientMaxScore = 1;
    [SerializeField] private int _coefficientMaxScoreUp = 1;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private Image _image;

    private int _score;
    private float _partScore;
    private int _fifthLevel = 2;
    private int _halfLevel = 5;
    private bool _isFifthSubLevel = false;
    private bool _isMaxSubLevel = false;
    private bool _isEndLevel = false;

    public event UnityAction<int> ScoreChange;
    public event UnityAction EndLevel;
    public event UnityAction<int> SubLevelUp;

    private void Start()
    {
        ChangeMaxScoreValue();
    }

    private void OnEnable()
    {
        _levelLoader.GenerationStart += OnGenerationStart;
    }

    private void OnDisable()
    {
        _levelLoader.GenerationStart -= OnGenerationStart;
    }

    public void ChangeBarLevel(int currentScore, int maxScore)
    {
        _image.fillAmount = currentScore / (float)maxScore;
    }

    public void LoadLevelScoreMax()
    {
        ChangeMaxScoreValue();
    }

    public void ChangeScore(int value)
    {
        if (!_isEndLevel)
        {
            _score += value;
            ChangeBarLevel(_score, _levelScoreMax);
        }

        if (_score >= _levelScoreMax)
            _score = _levelScoreMax;

        ScoreChange?.Invoke(_score);

        if (_score >= _levelScoreMax)
        {
            //_isGame = false;
            _isEndLevel = true;
            EndLevel?.Invoke();
            ChangeMaxScoreValue();
            return;
        }

        if (_score >= _partScore * _fifthLevel && !_isFifthSubLevel)
        {
            _isFifthSubLevel = true;
            SubLevelUp?.Invoke(2);
        }
        else if (_score >= _partScore * _halfLevel && !_isMaxSubLevel)
        {
            _isMaxSubLevel = true;
            SubLevelUp?.Invoke(3);
        }
    }

    private void ChangeMaxScoreValue()
    {
        _levelScoreMax = _levelScoreMax + _nextLevelScoreUp * _coefficientMaxScore;
        _partScore = CountParts();
        _coefficientMaxScore += _coefficientMaxScoreUp;
    }

    private float CountParts()
    {
        return _levelScoreMax * 0.1f;
    }

    private void OnGenerationStart()
    {
        _score = 0;
        _isEndLevel = false;
        _isFifthSubLevel = false;
        _isMaxSubLevel = false;
        ChangeScore(0);
    }
}

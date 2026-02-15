using UnityEngine;

public class AudioCounter : MonoBehaviour
{
    [SerializeField] private int _playMusic = 5;
    [SerializeField] private LevelLoader _levelLoader;

    private int _currentMusic = 0;
    private bool _isStop = false;

    public bool IsStop => _isStop;

    private void OnEnable()
    {
        _levelLoader.LevelUpgraded += OnLevelUp;
    }

    private void OnDisable()
    {
        _levelLoader.LevelUpgraded -= OnLevelUp;
    }

    private void Update()
    {
        if (_currentMusic < _playMusic && _isStop)
            _isStop = false;
    }

    public void Subscribe(BallAudio ball)
    {
        ball.MusicStarting += TurnUpVolume;
        ball.MusicEnded += TurnDownVolume;
    }

    public void Unsubscribe(BallAudio ball)
    {
        ball.MusicStarting -= TurnUpVolume;
        ball.MusicEnded -= TurnDownVolume;
    }

    private void TurnUpVolume()
    {
        _currentMusic++;

        if (_currentMusic >= _playMusic)
            _isStop = true;
    }

    private void TurnDownVolume()
    {
        _currentMusic--;
    }

    private void OnLevelUp(int value)
    {
        _currentMusic = 0;
    }
}

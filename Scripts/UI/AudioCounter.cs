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
        _levelLoader.LevelUp += OnLevelUp;
    }

    private void OnDisable()
    {
        _levelLoader.LevelUp -= OnLevelUp;
    }

    private void Update()
    {
        if (_currentMusic < _playMusic && _isStop)
            _isStop = false;
    }

    public void Subscribe(BallAudio ball)
    {
        ball.OnStartMusic += PlusMusic;
        ball.OnEndMusic += MinusMusic;
    }

    public void Unsubscribe(BallAudio ball)
    {
        ball.OnStartMusic -= PlusMusic;
        ball.OnEndMusic -= MinusMusic;
    }

    private void PlusMusic()
    {
        _currentMusic++;

        if (_currentMusic >= _playMusic)
            _isStop = true;
    }

    private void MinusMusic()
    {
        _currentMusic--;
    }

    private void OnLevelUp(int value)
    {
        _currentMusic = 0;
    }
}

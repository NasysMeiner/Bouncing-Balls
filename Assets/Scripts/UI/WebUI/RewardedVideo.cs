using Agava.YandexGames;
using System;
using UnityEngine;

public class RewardedVideo : MonoBehaviour
{
    [SerializeField] private int _crystallCount;

    private Bank _bank;

    private event Action _onVideoOpened;

    public void Initialize(Bank bank)
    {
        _bank = bank;
    }

    private void SetRevardForVideo()
    {
        _bank.AddCristall(_crystallCount);
    }

    private void SetActiveAudioListener()
    {
        AudioListener.volume = 1;
        Time.timeScale = 1;
    }

    private void OnErrorVideo(string value)
    {
        AudioListener.volume = 1;
        Time.timeScale = 1;
    }

    public void OnShowVideo()
    {
        AudioListener.volume = 0;
        Time.timeScale = 0;
        VideoAd.Show(_onVideoOpened, SetRevardForVideo, SetActiveAudioListener, OnErrorVideo);
    }
}

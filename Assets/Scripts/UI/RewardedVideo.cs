using Agava.YandexGames;
using System;
using UnityEngine;

public class RewardedVideo : MonoBehaviour
{
    [SerializeField] private PlayerInfo _PlayerInfo;
    [SerializeField] private int _geamsCount;

    private event Action _onVideoOpened;

    private void SetRevardForVideo()
    {
        _PlayerInfo.ChangeCristall(_geamsCount);
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

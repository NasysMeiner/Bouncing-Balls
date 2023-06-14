using System;
using System.Collections;
using Agava.YandexGames;
using UnityEngine;

public class RewardedVideo : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private int _geamsCount;

    private event Action _onVideoOpened;
    private event Action _onRewardedCallback;
    public event Action _onCloseAd;

    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private void OnEnable()
    {
        _onRewardedCallback += () =>
        {
            SetRevardForVideo();
        };

        _onCloseAd += () =>
        {
            SetActiveAudioListener();
        };
    }

    private void OnDisable()
    {
        _onRewardedCallback -= () =>
        {
            SetRevardForVideo();
        };

        _onCloseAd -= () =>
        {
            SetActiveAudioListener();
        };
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif

        yield return YandexGamesSdk.Initialize();
    }

    private void SetRevardForVideo()
    {
        _game.ChangeCristall(_geamsCount);
    }

    private void SetActiveAudioListener()
    {
        AudioListener.volume = 1;
        _game.ChangePause(1);
    }

    public void OnShowVideo()
    {
        AudioListener.volume = 0;
        _game.ChangePause(0);
        VideoAd.Show(_onVideoOpened, _onRewardedCallback, _onCloseAd);
    }
}

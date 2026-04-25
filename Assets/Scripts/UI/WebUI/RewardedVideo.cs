using Agava.YandexGames;
using BouncingBalls.GameSystem;
using System;
using UnityEngine;

namespace BouncingBalls.WebSystem
{
    public class RewardedVideo : MonoBehaviour
    {
        [SerializeField] private int _crystallCount;

        private Bank _bank;

        private event Action VideoOpened;

        public void Initialize(Bank bank)
        {
            _bank = bank;
        }

        public void OnShowVideo()
        {
            AudioListener.volume = 0;
            Time.timeScale = 0;
            VideoAd.Show(VideoOpened, SetRevardForVideo, SetActiveAudioListener, OnErrorVideo);
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
    }
}
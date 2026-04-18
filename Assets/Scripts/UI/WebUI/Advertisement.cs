using Agava.YandexGames;
using Lean.Localization;
using System;
using System.Collections;
using UnityEngine;

namespace BouncingBalls.WebSystem
{
    public class Advertisement : MonoBehaviour
    {
        private const string _languageRussian = "Russian";
        private const string _languageEnglish = "English";
        private const string _languageTurkish = "Turkish";

        private event Action OpenAd;
        public event Action<bool> CloseAd;

        private void OnEnable()
        {
            CloseAd += OnCloseAd;
        }

        private void OnDisable()
        {
            CloseAd -= OnCloseAd;
        }

        private IEnumerator Start()
        {
#if !UNITY_WEBGL || UNITY_EDITOR

            yield break;
#endif
            yield return YandexGamesSdk.Initialize();

            string language = YandexGamesSdk.Environment.i18n.lang;

            if (language == "ru")
                language = _languageRussian;
            else if (language == "en")
                language = _languageEnglish;
            else if (language == "tr")
                language = _languageTurkish;

            LeanLocalization.SetCurrentLanguageAll(language);
        }

        public void ShowAd()
        {
            AudioListener.volume = 0;
            Time.timeScale = 0;
            InterstitialAd.Show(OpenAd, CloseAd);
        }

        private void OnInBackgroundChange(bool inBackground)
        {
            AudioListener.pause = inBackground;
            AudioListener.volume = inBackground ? 0f : 1f;
        }

        private void OnCloseAd(bool isClosed)
        {
            Time.timeScale = 1;
            AudioListener.volume = 1;
        }
    }
}
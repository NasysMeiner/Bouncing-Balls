using Agava.WebUtility;
using Agava.YandexGames;
using Lean.Localization;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Advertisement : MonoBehaviour
{
    [SerializeField] private GameObject _isAvtorizeOff;
    [SerializeField] private GameObject _isOn;
    [SerializeField] private Button _button;
    [SerializeField] private PlayerData _playerData;

    private const string _languageRussian = "Russian";
    private const string _languageEnglish = "English";
    private const string _languageTurkish = "Turkish";

    private event Action OnOpenAd;
    public event Action<bool> OnCloseAd;

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;

        OnCloseAd += (bool isClosed) =>
        {
            SetActiveAudioListener(isClosed);
        };
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;

        OnCloseAd -= (bool isClosed) =>
        {
            SetActiveAudioListener(isClosed);
        };
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR

        yield break;
#endif
        _button.interactable = false;

        yield return YandexGamesSdk.Initialize();

        string language = YandexGamesSdk.Environment.i18n.lang;

        if (language == "ru")
            language = _languageRussian;
        else if (language == "en")
            language = _languageEnglish;
        else if (language == "tr")
            language = _languageTurkish;

        LeanLocalization.SetCurrentLanguageAll(language);
        StartCoroutine(OnGetCloudSaveDataButtonClick());
    }

    public void ShowAd()
    {
        AudioListener.volume = 0;
        Time.timeScale = 0;
        InterstitialAd.Show(OnOpenAd, OnCloseAd);
    }

    private void OnInBackgroundChange(bool inBackground)
    {
        AudioListener.pause = inBackground;
        AudioListener.volume = inBackground ? 0f : 1f;
    }

    private void SetActiveAudioListener(bool isClosed)
    {
        Time.timeScale = 1;
        AudioListener.volume = 1;
    }

    private IEnumerator OnGetCloudSaveDataButtonClick()
    {
        string loadedString = "None";
        string noData = "{}";
        PlayerAccount.GetCloudSaveData((data) => loadedString = data);

        while (loadedString == "None")
        {
            yield return null;
        }

        if (loadedString == noData)
        {
            _button.interactable = true;
            yield break;
        }

        _isAvtorizeOff.SetActive(false);
        _isOn.SetActive(true);
        _playerData.LoadPlayerData(loadedString);
        _button.interactable = true;
    }
}

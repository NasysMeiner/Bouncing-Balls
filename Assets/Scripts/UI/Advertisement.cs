using System;
using System.Collections;
using UnityEngine;
using Agava.YandexGames;
using UnityEngine.UI;
using Agava.WebUtility;
using Newtonsoft.Json;
using Lean.Localization;
using TMPro;

public class Advertisement : MonoBehaviour
{
    [SerializeField] private GameObject _isAvtorizeOff;
    [SerializeField] private GameObject _isOn;
    [SerializeField] private Image _startIcon;
    [SerializeField] private TMP_Text _startName;
    [SerializeField] private Button _button;
    [SerializeField] private Game _game;

    private int _score;

    private event Action onOpenAd;
    public event Action<bool> onCloseAd;

    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;

        onCloseAd += (bool isClosed) =>
        {
            SetActiveAudioListener(isClosed);
        };
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;

        onCloseAd -= (bool isClosed) =>
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
        Console.WriteLine("off");

        yield return YandexGamesSdk.Initialize();

        Console.WriteLine("initYndx");

        string name = "Nan";
        string language = YandexGamesSdk.Environment.i18n.lang;

        if (language == "ru")
            language = "Russian";
        else if (language == "en")
            language = "English";
        else if (language == "tr")
            language = "Turkish";

        LeanLocalization.SetCurrentLanguageAll(language);
        Console.WriteLine("language");
        OnRequestPersonalProfileDataPermissionButtonClick();
        OnGetProfileDataButtonClick();

        if (PlayerAccount.IsAuthorized)
        {
            Leaderboard.GetPlayerEntry("NewLeaders", (result) =>
            {
                if (result.score > 0)
                {
                    name = result.player.publicName;

                    if (string.IsNullOrEmpty(name))
                        name = "Anonymous";

                    _score = result.score;
                }
                else
                {
                    name = result.player.publicName;

                    if (string.IsNullOrEmpty(name))
                        name = "Anonymous";

                    _score = 0;
                }
            });

            while (name == "Nan")
            {
                yield return null;
            }
        }

        Console.WriteLine("Start");

        StartCoroutine(OnGetCloudSaveDataButtonClick(name));
    }

    public void ShowAd()
    {
        AudioListener.volume = 0;
        Time.timeScale = 0;
        InterstitialAd.Show(onOpenAd, onCloseAd);
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

    private void OnRequestPersonalProfileDataPermissionButtonClick()
    {
        if (PlayerAccount.IsAuthorized)
            PlayerAccount.RequestPersonalProfileDataPermission();
    }

    private void OnGetProfileDataButtonClick()
    {
        if (PlayerAccount.IsAuthorized)
        {
            string name;

            PlayerAccount.GetProfileData((result) =>
            {
                name = result.publicName;

                if (string.IsNullOrEmpty(name))
                    name = "Anonymous";

                _game.ChangeName(name);
            });
        }
    }

    private IEnumerator OnGetCloudSaveDataButtonClick(string name)
    {
        string loadedString = "None";
        PlayerAccount.GetCloudSaveData((data) => loadedString = data);

        while (loadedString == "None")
        {
            yield return null;
        }

        if (loadedString == "{}")
        {
            _button.interactable = true;
            yield break;
        }

        _game.OnGetCloudSaveDataButtonClick(loadedString, _score, name);
        _isAvtorizeOff.SetActive(false);
        _isOn.SetActive(true);
        _button.interactable = true;
    }
}

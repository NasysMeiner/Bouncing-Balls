using Agava.YandexGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutorizePanel : MonoBehaviour
{
    [SerializeField] private GameObject _autorizePanel;
    [SerializeField] private LeaderboardView _leaderboardView;
    [SerializeField] private Liderboard _liderboard;
    [SerializeField] private Panel _panel;
    [SerializeField] private Panel _BG;

    public void Authorized()
    {
        PlayerAccount.Authorize();

        if (PlayerAccount.IsAuthorized)
        {
            _autorizePanel.gameObject.SetActive(false);
            _BG.gameObject.SetActive(false);
            Time.timeScale = 1;
            PlayerAccount.RequestPersonalProfileDataPermission();
            _liderboard.CheckReating();
        }
    }

    public void Unauthorized()
    {
        if(PlayerAccount.IsAuthorized == false)
        {
            Time.timeScale = 0;
            _autorizePanel.gameObject.SetActive(true);
            _BG.gameObject.SetActive(true);
        }
        else
        {
            PlayerAccount.RequestPersonalProfileDataPermission();
            _panel.PlayAnimationLiderboard("Open");
            _liderboard.CheckReating();
        }
    }
}

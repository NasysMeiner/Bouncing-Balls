using Agava.YandexGames;
using UnityEngine;

public class AutorizePanel : MonoBehaviour
{
    [SerializeField] private GameObject _autorizePanel;
    [SerializeField] private LeaderboardView _leaderboardView;
    [SerializeField] private Leaderboard _liderboard;
    [SerializeField] private Panel _panel;
    [SerializeField] private Panel _BG;

    private const string _textOpenLeaderbord = "Open";

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
        if (PlayerAccount.IsAuthorized == false)
        {
            Time.timeScale = 0;
            _autorizePanel.gameObject.SetActive(true);
            _BG.gameObject.SetActive(true);
        }
        else
        {
            PlayerAccount.RequestPersonalProfileDataPermission();
            _panel.PlayAnimationLeaderboard(_textOpenLeaderbord);
            _liderboard.CheckReating();
        }
    }
}

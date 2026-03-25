using Agava.YandexGames;
using UnityEngine;

namespace BouncingBalls
{
    public class AutorizePanel : MonoBehaviour
    {
        [SerializeField] private GameObject _isAutorizePanel;
        [SerializeField] private LeaderboardView _leaderboardView;
        [SerializeField] private Leaderboard _liderboard;
        [SerializeField] private GameManager _gameManager;

        public void Authorized()
        {
            if (!_gameManager.IsUnity)
            {
                PlayerAccount.Authorize();

                if (PlayerAccount.IsAuthorized)
                {
                    PlayerAccount.RequestPersonalProfileDataPermission();
                    _liderboard.CheckRating();
                }
            }

            Time.timeScale = 1;
            _isAutorizePanel.SetActive(false);
        }

        public void Unauthorized()
        {
            if (!_gameManager.IsUnity)
            {
                if (PlayerAccount.IsAuthorized == false)
                {
                    Time.timeScale = 0;
                    _isAutorizePanel.gameObject.SetActive(true);
                }
                else
                {
                    PlayerAccount.RequestPersonalProfileDataPermission();
                    _liderboard.CheckRating();
                }
            }
        }
    }
}
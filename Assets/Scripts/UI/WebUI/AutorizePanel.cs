using Agava.YandexGames;
using BouncingBalls.LevelSystem;
using UnityEngine;

namespace BouncingBalls.WebSystem
{
    public class AutorizePanel : MonoBehaviour
    {
        [SerializeField] private GameObject _autorizePanel;
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
            _autorizePanel.SetActive(false);
        }

        public void Unauthorized()
        {
            if (!_gameManager.IsUnity)
            {
                if (PlayerAccount.IsAuthorized == false)
                {
                    Time.timeScale = 0;
                    _autorizePanel.SetActive(true);
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
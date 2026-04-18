using Agava.YandexGames;
using BouncingBalls.LevelSystem;
using BouncingBalls.View;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls.WebSystem
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] private LeaderboardView _leaderboardView;
        [SerializeField] private Rating _ratingPlayerView;

        private GameManager _gameManager;
        private UIManager _uiManager;

        private List<PlayerInfoLeaderboard> _userInfo = new();
        private string _nameLeaderboard = "NewLeaders";

        public void Initialize(GameManager gameManager, UIManager uIManager)
        {
            _gameManager = gameManager;
            _uiManager = uIManager;
        }

        public void CheckRating()
        {
            if (!_gameManager.IsUnity && PlayerAccount.IsAuthorized)
            {
                _userInfo.Clear();

                Agava.YandexGames.Leaderboard.GetPlayerEntry(_nameLeaderboard, (result) =>
                {
                    if (result != null)
                    {
                        string name = result.player.publicName;
                        _uiManager.SetName(name);

                        if (string.IsNullOrEmpty(name))
                            name = "Anonymos";

                        _ratingPlayerView.Inst(result.rank, name, result.score);
                    }
                });

                Agava.YandexGames.Leaderboard.GetEntries(_nameLeaderboard, (result) =>
                {
                    int leanguageLeadeboard = result.entries.Length;
                    leanguageLeadeboard = Math.Clamp(leanguageLeadeboard, 1, 10);

                    for (int i = 0; i < leanguageLeadeboard; i++)
                    {
                        string name = result.entries[i].player.publicName;
                        int score = result.entries[i].score;
                        int rank = result.entries[i].rank;

                        if (string.IsNullOrEmpty(name))
                            name = "Anonymos";

                        _userInfo.Add(new PlayerInfoLeaderboard(name, score, rank));
                    }

                    _leaderboardView.ViewLeaderbordContent(_userInfo);
                });
            }

        }

        public void OnGetLeaderboardEntries(int value)
        {
            if (!_gameManager.IsUnity && PlayerAccount.IsAuthorized)
            {
                Agava.YandexGames.Leaderboard.GetPlayerEntry(_nameLeaderboard, (result) =>
                {
                    if (result.score < value)
                    {
                        Agava.YandexGames.Leaderboard.SetScore(_nameLeaderboard, value);
                    }
                });
            }
        }

        public void CheckPlayerName()
        {
            if (PlayerAccount.IsAuthorized)
            {
                Agava.YandexGames.Leaderboard.GetPlayerEntry(_nameLeaderboard, (result) =>
                {
                    string name = result.player.publicName;

                    if (string.IsNullOrEmpty(name))
                        name = "Anonymos";
                });
            }
        }
    }
}
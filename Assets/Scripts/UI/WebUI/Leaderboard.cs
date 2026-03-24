using Agava.YandexGames;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Rating _ratingPlayer;
    [SerializeField] private PrefabPlayers _prefabPlayers;
    [SerializeField] private LeaderboardView _leaderboard;
    [SerializeField] private PlayerInfo _playerInfo;

    private List<PrefabPlayers> _ratingListPlayers = new List<PrefabPlayers>();
    private List<PlayerInfoLeaderboard> _userInfo = new List<PlayerInfoLeaderboard>();
    private string _nameLeaderboard = "NewLeaders";

    public void CheckReating()
    {
        if (PlayerAccount.IsAuthorized)
        {
            foreach (var raitingt in _ratingListPlayers)
            {
                Destroy(raitingt.gameObject);
            }

            _userInfo.Clear();

            Agava.YandexGames.Leaderboard.GetPlayerEntry(_nameLeaderboard, (result) =>
            {
                if (result != null)
                {
                    string name = result.player.publicName;
                    _playerInfo.SetName(name);

                    if (string.IsNullOrEmpty(name))
                        name = "Anonymos";

                    _ratingPlayer.Inst(result.rank, name, result.score);
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

                _leaderboard.ViewLeaderbordContent(_userInfo);
            });
        }

    }

    public void OnGetLeaderboardEntries(int value)
    {
        if (PlayerAccount.IsAuthorized)
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

public class PlayerInfoLeaderboard
{
    public string Name { get; private set; }
    public int Score { get; private set; }
    public int Rank { get; private set; }

    public PlayerInfoLeaderboard(string name, int score, int rank)
    {
        Name = name;
        Score = score;
        Rank = rank;
    }
}

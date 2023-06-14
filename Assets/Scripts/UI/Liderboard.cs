using Agava.YandexGames;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Liderboard : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Game _game;
    [SerializeField] private Rating _ratingPlayer;
    [SerializeField] private PrefabPlayers _prefabPlayers;
    [SerializeField] private LeaderboardView _leaderboard;

    private List<PrefabPlayers> _ratingListPlayers = new List<PrefabPlayers>();
    private List<PlayerInfoLeaderboard> _playerInfo = new List<PlayerInfoLeaderboard>();

    public void CheckReating()
    {
        if (PlayerAccount.IsAuthorized)
        {
            foreach (var raitingt in _ratingListPlayers)
            {
                Destroy(raitingt.gameObject);
            }

            _playerInfo.Clear();

            Leaderboard.GetPlayerEntry("NewLeaders", (result) =>
            {
                if (result == null)
                {
                    Console.WriteLine("Player is not present in the leaderboard.");
                }
                else
                {
                    string name = result.player.publicName;

                    if (string.IsNullOrEmpty(name))
                        name = "Anonymos";

                    _ratingPlayer.Inst(result.rank, name, result.score);
                }
            });

            Leaderboard.GetEntries("NewLeaders", (result) =>
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

                    _playerInfo.Add(new PlayerInfoLeaderboard(name, score, rank));
                }

                _leaderboard.ViewLeaderbordContent(_playerInfo);
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

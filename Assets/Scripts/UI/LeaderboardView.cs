using System.Collections.Generic;
using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private PrefabPlayers _prefabPlayers;
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _spawnpoint;

    private List<PrefabPlayers> _stockPrefab = new List<PrefabPlayers>();

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            PrefabPlayers newPlayer = Instantiate(_prefabPlayers, _spawnpoint);
            _stockPrefab.Add(newPlayer);
            newPlayer.Init(i + 1, "-", 0);
        }
    }

    public void ViewLeaderbordContent(List<PlayerInfoLeaderboard> playerInfo)
    {
        int b = 1;

        foreach (var player in _stockPrefab)
        {
            player.Init(b, "-", 0);
            b++;
        }

        for (int a = 0; a < playerInfo.Count; a++)
        {
            PrefabPlayers newPlayer = _stockPrefab[a];

            _stockPrefab[a].Init(playerInfo[a].Rank, playerInfo[a].Name, playerInfo[a].Score);
            newPlayer.transform.SetParent(_transform);
        }
    }
}

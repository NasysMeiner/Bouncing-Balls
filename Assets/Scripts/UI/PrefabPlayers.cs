using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rating))]
public class PrefabPlayers : MonoBehaviour
{
    private Rating _rating;

    public void Init(int rank, string name, int score)
    {
        _rating = GetComponent<Rating>();
        _rating.Inst(rank, name, score);
    }
}

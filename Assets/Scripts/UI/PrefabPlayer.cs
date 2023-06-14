using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rating))]
public class PrefabPlayer : MonoBehaviour
{
    private Rating _rating;

    private void Start()
    {
        _rating = GetComponent<Rating>();
    }

    public void Init(int rank, string name, int score)
    {
        _rating.Inst(rank, name, score);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rating : MonoBehaviour
{
    [SerializeField] private TMP_Text _rankText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private List<Image> _ratings;

    public void Inst(int rank, string name, int score)
    {
        _rankText.text = rank.ToString();
        _nameText.text = name;
        _scoreText.text = score.ToString();
        int value = 1;

        foreach(var ratings in _ratings)
        {
            if(rank == value)
            {
                _ratings[value - 1].enabled = true;
                _rankText.gameObject.SetActive(false);
            }

            value++;
        }
    }
}

using TMPro;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void ChangeText(float value)
    {
        _text.text = $"{value}";
    }
}

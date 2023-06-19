using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Balance : MonoBehaviour
{
    private TMP_Text _text;

    private void OnEnable()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void ChangeText(float currentValue)
    {
        _text.text = currentValue.ToString();
    }
}

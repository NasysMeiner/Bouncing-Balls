using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Bar : MonoBehaviour
{
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void ChangeBarLevel(float currentScore, float maxScore)
    {
        _image.fillAmount = currentScore / maxScore;
    }
}

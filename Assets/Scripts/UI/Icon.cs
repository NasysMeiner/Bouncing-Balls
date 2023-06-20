using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    [SerializeField] private GameObject _enable;
    [SerializeField] private Image _image;

    public GameObject Enable => _enable;
    public Image Image => _image;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private string _name;

    public string Name => _name;

    public void ChangeActive(bool value)
    {
        _image.enabled = value;
    }
}

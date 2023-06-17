using System.Collections.Generic;
using UnityEngine;

public class Icons : MonoBehaviour
{
    [SerializeField] private Icon _startIcon;
    [SerializeField] private LevelLoader _game;
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private List<Sprite> _icons;

    private Icon _currentIcon;

    public List<Sprite> IconsNew => _icons;

    private void Start()
    {
        ChangeIcon(_startIcon);
    }

    public void ChangeIcon(Icon icon)
    {
        if(_currentIcon != null)
        {
            _currentIcon.Enable.SetActive(false);
        }

        for(int i = 0; i < _icons.Count; i++)
        {
            if (_icons[i] == icon.Image.sprite)
                _playerInfo.SetIcon(i);
        }

        _currentIcon = icon;
        _currentIcon.Enable.SetActive(true);
    }
}

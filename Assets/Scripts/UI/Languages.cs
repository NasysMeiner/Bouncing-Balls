using System.Collections.Generic;
using UnityEngine;

public class Languages : MonoBehaviour
{
    [SerializeField] private List<Language> _languages;

    private Language _currentLanguage;

    private void Start()
    {
        _currentLanguage = _languages[0];
        _currentLanguage.ChangeActive(true);
    }

    public void ChangeLanguage(Language language)
    {
        foreach (Language lang in _languages)
        {
            if(lang == language)
            {
                _currentLanguage.ChangeActive(false);
                _currentLanguage = lang;
                Lean.Localization.LeanLocalization.SetCurrentLanguageAll($"{_currentLanguage.Name}");
                _currentLanguage.ChangeActive(true);
                break;
            }
        }
    }
}

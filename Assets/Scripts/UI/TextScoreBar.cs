using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TextScoreBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _textSubLevel;

    private Animator _animatorSubLevel;
    private int _fifthLevel = 2;

    private void Start()
    {
        _animatorSubLevel = GetComponent<Animator>();
    }

    public void ChangeTextLevel(int Level, int subLevel)
    {
        _textSubLevel.text = $"lv. {Level}.{subLevel}";
    }

    public void SubLevelAnimation(int subLevel)
    {
        if (subLevel == _fifthLevel)
        {
            PlayAnimationSubLevel1();
        }
        else if (subLevel == 1)
        {
            PlayAnimationEndLevel();
        }
        else if (subLevel != 0 && subLevel != _fifthLevel && subLevel != 1)
        {
            PlayAnimationSubLevel2();
        }
    }
   
    public void PlayAnimationEndLevel()
    {
        _animatorSubLevel.SetTrigger("EndLevel");
    }

    private void PlayAnimationSubLevel1()
    {
        _animatorSubLevel.SetTrigger("SubLevel1");
    }

    private void PlayAnimationSubLevel2()
    {
        _animatorSubLevel.SetTrigger("SubLevel2");
    }
}

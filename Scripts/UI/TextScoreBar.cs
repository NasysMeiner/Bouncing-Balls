using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TextScoreBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _textSubLevel;

    private Animator _animatorSubLevel;
    private int _fifthLevel = 2;
    private string _textAnimationEndLevel = "EndLevel";
    private string _textAnimationSubLevel1 = "SubLevel1";
    private string _textAnimationSubLevel2 = "SubLevel2";

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
        _animatorSubLevel.SetTrigger(_textAnimationEndLevel);
    }

    private void PlayAnimationSubLevel1()
    {
        _animatorSubLevel.SetTrigger(_textAnimationSubLevel1);
    }

    private void PlayAnimationSubLevel2()
    {
        _animatorSubLevel.SetTrigger(_textAnimationSubLevel2);
    }
}

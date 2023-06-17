using System.Collections;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Panel _leaderboardPanel;

    private bool _isClose = true;
    private bool _isOpen = false;

    public void ChangeActivePanel(bool value)
    {
        gameObject.SetActive(value);
    }

    public void ChangeTime()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    public void PlayAnimation(string anim)
    {
        _animator.SetTrigger(anim);
    }

    public void PlayAnimationLeaderboard(string anim)
    {
        if (anim == "Open")
        {
            if (_isClose && _isOpen == false)
            {
                _leaderboardPanel.gameObject.SetActive(true);
                _isOpen = true;
                _animator.SetTrigger(anim);
            }
        }
        else
        {
            _animator.SetTrigger(anim);

            if (anim == "Exit")
            {
                _leaderboardPanel.gameObject.SetActive(false);
                _isOpen = false;
                _isClose = false;
            }

            StartCoroutine(IsClose());
        }
    }

    private IEnumerator IsClose()
    {
        yield return new WaitForSeconds(0.4f);

        _isClose = true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Panel _panel;

    private Vector3 _startPosition;
    private bool _isClose = true;
    private bool _isOpen = false;

    private void Start()
    {
        _startPosition = transform.position;
    }

    public void ChangeActivePanel(bool value)
    {
        gameObject.SetActive(value);
    }

    public void ChangeActiveLiderboard(bool value)
    {
        gameObject.SetActive(value);
    }

    public void ChangeTime()
    {
        if(Time.timeScale == 0)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    public void ChangePositionPanel(int x2)
    {
        if (transform.position == _startPosition)
            transform.position = new Vector3(x2, transform.position.y, transform.position.z);
        else
            transform.position = _startPosition;
    }

    public void PlayAnimation(string anim)
    {
        _animator.SetTrigger(anim);
    }

    public void PlayAnimationLiderboard(string anim)
    {
        if(anim == "Open")
        {
            if (_isClose && _isOpen == false)
            {
                _panel.gameObject.SetActive(true);
                _isOpen = true;
                _animator.SetTrigger(anim);
            }
        }
        else
        {
            _animator.SetTrigger(anim);

            if (anim == "Exit")
            {
                _panel.gameObject.SetActive(false);
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

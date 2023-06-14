using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProductButton : MonoBehaviour
{
    [SerializeField] private bool _isBuff = false;
    [SerializeField] private Image _image;
    [SerializeField] private Animator _animator;
    [SerializeField] private Button _button;

    private ProductView _productView;
    private bool _isPlay = false;
    private bool _buffsAnim = false;
    private bool _upAnim = false;
    private float _timeBuffs;

    public bool IsBuff => _isBuff;

    public void PlayAnimation()
    {
        if (_buffsAnim)
        {
            if (_isPlay == false)
            {
                _animator.SetTrigger("Play");
                _button.interactable = false;
                StartCoroutine(AnimationTimeBuff());
                _isBuff = true;
                _isPlay = true;
            }
            else
            {
                _animator.SetTrigger("End");
                _isBuff = false;
                _isPlay = false;
                _productView.EndAnimation();
            }
        }
    }

    public void InitButton(bool buff, bool up, ProductView productView, float timeBuffs)
    {
        _buffsAnim = buff;
        _upAnim = up;
        _productView = productView;
        _timeBuffs = timeBuffs;
    }

    private IEnumerator AnimationTimeBuff()
    {
        for(int i = (int)_timeBuffs * 2 - 1; i >= 0; i--)
        {
            _image.fillAmount = i / (_timeBuffs * 2);

            yield return new WaitForSeconds(0.5f);
        }

        _image.fillAmount = 1;
    }
}

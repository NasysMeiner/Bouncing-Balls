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
    private float _timeBuffs;
    private const string _textStartAnimation = "Play";
    private const string _textExitAnimation = "End";

    public bool IsBuff => _isBuff;

    public void PlayAnimation()
    {
        if (_buffsAnim)
        {
            if (_isPlay == false)
            {
                _animator.SetTrigger(_textStartAnimation);
                _button.interactable = false;
                StartCoroutine(AnimationTimeBuff());
                _isBuff = true;
                _isPlay = true;
            }
            else
            {
                _animator.SetTrigger(_textExitAnimation);
                _isBuff = false;
                _isPlay = false;
                _productView.EndAnimation();
            }
        }
    }

    public void InitButton(bool buff, ProductView productView, float timeBuffs)
    {
        _buffsAnim = buff;
        _productView = productView;
        _timeBuffs = timeBuffs;
    }

    private IEnumerator AnimationTimeBuff()
    {
        float waitingTime = 1f;

        for (int i = (int)_timeBuffs; i > 0; i--)
        {
            yield return new WaitForSeconds(waitingTime);

            _image.fillAmount = i / (_timeBuffs);

        }

        _image.fillAmount = 1;
    }
}

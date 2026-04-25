using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BouncingBalls.View
{
    public class CooldownButton : MonoBehaviour, IInitializeButton
    {
        [SerializeField] private Button _button;
        [SerializeField] private bool _isUseCoolDown;
        [SerializeField] private Image _imageCoolDown;
        [SerializeField] private TMP_Text _textPrice;
        [SerializeField] private Animator _animator;

        private const string _textStartAnimation = "Play";
        private const string _textExitAnimation = "End";

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        public void Initialize(int value)
        {
            _textPrice.text = value.ToString();
        }

        public void ResetButton()
        {
            if (!_isUseCoolDown)
                return;

            StopAllCoroutines();

            if (!_button.interactable)
                _animator.SetTrigger(_textExitAnimation);

            _button.interactable = true;
            _imageCoolDown.fillAmount = 1;
        }

        public void SetTimeInactive(float timeInactive = 0)
        {
            StartCoroutine(StartTimeInactive(timeInactive));
        }

        public void OffButton()
        {
            _animator.SetTrigger(_textStartAnimation);
            _button.interactable = false;
        }

        private IEnumerator StartTimeInactive(float timeInactive)
        {
            if (timeInactive > 0)
            {
                OffButton();

                if (_isUseCoolDown)
                {
                    float currentTime = timeInactive;
                    while (currentTime > 0)
                    {
                        _imageCoolDown.fillAmount = currentTime / timeInactive;

                        yield return null;

                        currentTime -= Time.deltaTime;
                    }

                    ResetButton();
                }
            }
        }
    }
}
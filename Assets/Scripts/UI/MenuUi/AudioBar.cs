using UnityEngine;
using UnityEngine.UI;

namespace BouncingBalls
{
    public class AudioBar : MonoBehaviour
    {
        [SerializeField] private AudioVolumeType _volumeType = AudioVolumeType.Effects;
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _iconMuted;
        [SerializeField] private Image _iconUnmuted;

        [Space]
        [SerializeField] private float _minValue = 0f;
        [SerializeField] private float _maxValue = 1f;

        private float _previousVolume = 0.5f;
        private bool _isInitialized;

        private void OnEnable()
        {
            SyncWithAudioManager();
        }

        private void Start()
        {
            InitializeSlider();
            SetupSlider();
        }

        private void InitializeSlider()
        {
            if (_slider == null)
            {
                _slider = GetComponent<Slider>();
            }

            _slider.minValue = _minValue;
            _slider.maxValue = _maxValue;
        }

        private void SetupSlider()
        {
            _slider.onValueChanged.RemoveAllListeners();
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void SyncWithAudioManager()
        {
            if (!_isInitialized)
            {
                float currentVolume = GetVolumeFromManager();
                _slider.value = currentVolume;
                _previousVolume = currentVolume > 0 ? currentVolume : 0.5f;
                UpdateIcons(currentVolume);
                _isInitialized = true;
            }
        }

        private void OnSliderValueChanged(float value)
        {
            SetVolumeToManager(value);
            UpdateIcons(value);

            if (value > 0)
            {
                _previousVolume = value;
            }
        }

        public void ToggleMute()
        {
            if (_slider.value > 0)
            {
                _previousVolume = _slider.value;
                _slider.value = 0;
            }
            else
            {
                _slider.value = _previousVolume;
            }
        }

        private float GetVolumeFromManager()
        {
            switch (_volumeType)
            {
                case AudioVolumeType.Music:
                    return AudioManager.Instance.MusicVolume;
                case AudioVolumeType.Effects:
                    return AudioManager.Instance.MusicVolume;
                default:
                    return _slider.value;
            }
        }

        private void SetVolumeToManager(float value)
        {
            switch (_volumeType)
            {
                case AudioVolumeType.Music:
                    AudioManager.Instance.SetMusicVolume(value);
                    break;
                case AudioVolumeType.Effects:
                    AudioManager.Instance.SetEffectsVolume(value);
                    break;
            }
        }

        private void UpdateIcons(float volume)
        {
            if (_iconMuted != null && _iconUnmuted != null)
            {
                bool isMuted = volume <= 0f;
                _iconMuted.enabled = isMuted;
                _iconUnmuted.enabled = !isMuted;
            }
        }
    }
}
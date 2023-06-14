using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _iconMax;
    [SerializeField] private Image _iconMin;
    [SerializeField] private AudioSource _music;

    private float _audio = 2.55f;
    private float _pastVolumeAudio = 0;
    private float _pastVolumeMusic = 0;

    public float Audio => _audio;

    public event UnityAction<float> ChangeVolumeBalls;

    public void OffMusic()
    {
        if (_slider.value == 0)
        {
            _slider.value = _pastVolumeMusic;
        }
        else
        {
            _pastVolumeMusic = _slider.value;
            _slider.value = 0;
        }

        ChangeVolume();
    }

    public void OffAudio()
    {
        if(_slider.value == 0)
        {
            _slider.value = _pastVolumeAudio;
        }
        else
        {
            _pastVolumeAudio = _slider.value;
            _slider.value = 0;
        }

        ChangeAudio();
    }

    public void ChangeAudio()
    {
        _audio = (255 * _slider.value) / 255f;

        if (_audio == 0)
        {
            _iconMin.enabled = true;
            _iconMax.enabled = false;
        }
        else
        {
            _iconMin.enabled = false;
            _iconMax.enabled = true;
        }

        ChangeVolumeBalls?.Invoke(_audio);
    }

    public void ChangeVolume()
    {
        _music.volume = (255 * _slider.value)/255;

        if (_music.volume == 0)
        {
            _iconMin.enabled = true;
            _iconMax.enabled = false;
        }
        else
        {
            _iconMin.enabled = false;
            _iconMax.enabled = true;
        }
    }
}

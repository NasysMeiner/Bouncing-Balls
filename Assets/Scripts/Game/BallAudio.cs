using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class BallAudio : MonoBehaviour
{
    private AudioSource _audio;
    private float _audioTime;
    private AudioCounter _audioCounter;
    private AudioBar _audioBar;

    public event UnityAction MusicStarting;
    public event UnityAction MusicEnded;

    public void Init(AudioCounter audioCounter, AudioBar audioBar)
    {
        _audioCounter = audioCounter;
        _audioBar = audioBar;

        OnChangeVolume(_audioBar.Audio);
        _audioBar.ChangeVolumeBalls += OnChangeVolume;
        audioCounter.Subscribe(this);
    }

    private void OnEnable()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioTime = _audio.clip.length;
    }

    public void StartPlayAudio()
    {
        StartCoroutine(PlayAudio());
    }

    private IEnumerator PlayAudio()
    {
        if (_audioCounter.IsStop == false && _audio.isActiveAndEnabled)
        {
            _audio.Play();
            MusicStarting.Invoke();

            yield return new WaitForSeconds(_audioTime);

            MusicEnded.Invoke();
        }
    }

    public void StopAudioPlay()
    {
        StopCoroutine(PlayAudio());
    }

    public void Unsubscribe()
    {
        _audioBar.ChangeVolumeBalls -= OnChangeVolume;
    }

    private void OnChangeVolume(float volume)
    {
        _audio.volume = volume;

        if (volume > 1)
            _audio.volume = (volume) / 255f;

    }
}

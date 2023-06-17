using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BallAudio : MonoBehaviour
{
    private AudioSource _audio;
    private float _audioTime;
    private AudioCounter _audioCounter;
    private AudioBar _audioBar;

    public event UnityAction OnStartMusic;
    public event UnityAction OnEndMusic;

    public void Init(AudioCounter audioCounter, AudioBar audioBar)
    {
        _audioCounter = audioCounter;
        _audioBar = audioBar;

        ChangeVolume(_audioBar.Audio);
        _audioBar.ChangeVolumeBalls += ChangeVolume;
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
            OnStartMusic.Invoke();

            yield return new WaitForSeconds(_audioTime);

            OnEndMusic.Invoke();
        }
    }

    public void StopAudioPlay()
    {
        StopCoroutine(PlayAudio());
    }

    public void Unsubscribe()
    {
        _audioBar.ChangeVolumeBalls -= ChangeVolume;
    }

    private void ChangeVolume(float volume)
    {
        _audio.volume = volume;

        if (volume > 1)
            _audio.volume = (volume) / 255f;

    }
}

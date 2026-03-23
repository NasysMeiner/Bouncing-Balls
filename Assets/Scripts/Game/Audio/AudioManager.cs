using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _audioSource;
    [Range(0f, 1f)]
    [SerializeField] private float _musicVolume;
    [Range(0f, 1f)]
    [SerializeField] private float _effectVolume;
    [SerializeField] private float _cooldownTime = 0.1f;
    [Space]
    [SerializeField] private List<AudioProfile> _audioProfiles = new();

    private Dictionary<string, AudioClip> _soundDictionary;

    private float _lastPlayTime = 0;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _soundDictionary = new();

        foreach(var profile in _audioProfiles)
            _soundDictionary.Add(profile.Name, profile.AudioClip);
    }

    public void PlaySound(string nameAudio)
    {
        if(_soundDictionary.TryGetValue(nameAudio, out AudioClip audioClip))
        {
            _audioSource.clip = audioClip;
            _audioSource.volume = _musicVolume;
            _audioSource.Play(); 
        }
    }

    public void PlayEffectAudio(string nameAudio)
    {
        if ((Time.time - _lastPlayTime) > _cooldownTime && _soundDictionary.TryGetValue(nameAudio, out AudioClip audioClip))
        {
            _audioSource.PlayOneShot(audioClip, _effectVolume);
            _lastPlayTime = Time.time;
        }
    }
}
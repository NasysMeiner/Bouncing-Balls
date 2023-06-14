using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 5;
    [SerializeField] private TrailRenderer _trail;

    private Rigidbody _rigidbody;
    private Gun _gun;
    private Camera _camera;
    private Vector3 _pointsTransform;
    private bool _isQueue = false;
    private int _profitability = 1;
    private float _speed;
    private Buffer _buffer;
    private int _id;
    private AudioSource _audio;
    private float _audioTime;
    private AudioCounter _audioCounter;
    private AudioBar _audioBar;

    public event UnityAction startMusic;
    public event UnityAction endMusic;

    public int Id => _id;
    public Rigidbody Rigidbody => _rigidbody;
    public float MaxSpeed => _maxSpeed;
    public float Speed => _speed;
    public TrailRenderer Train => _trail;
    public int Profitability => _profitability;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioTime = _audio.clip.length;
    }

    private void FixedUpdate()
    {
        _speed = _rigidbody.velocity.magnitude;

        if (_speed > _maxSpeed)
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;

        _pointsTransform = _camera.WorldToViewportPoint(transform.position);

        if ((_pointsTransform.y < -0.1f || _pointsTransform.y > 1.1f || _pointsTransform.x < -0.1f || _pointsTransform.x > 1.1) && _isQueue != true)
        {
            ChangeState(true);
            ChangeRenderer(false);
            _gun.AddBalls(_buffer.GetBall(this));
        }
    }

    public void Init(Gun gun, Camera camera, int profitability, Color newColor, Buffer buffer, int id, AudioCounter audioCounter, AudioBar audioBar)
    {
        _gun = gun;
        _buffer = buffer;
        _camera = camera;
        _profitability = profitability;
        _id = id;
        _audioCounter = audioCounter;
        _audioBar = audioBar;
        ChangeVolume(_audioBar.Audio);
        _audioBar.ChangeVolumeBalls += ChangeVolume;
        transform.position = _buffer.transform.position;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = newColor;
    }

    public void ChangeRenderer(bool isActive)
    {
        _trail.enabled = isActive;
    }

    public void ChangeState(bool state)
    {
        _rigidbody.isKinematic = state;
        _isQueue = state;
    }

    public void AddForceBalls(Vector3 force)
    {
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }

    public IEnumerator PlayAudio()
    {
        if (_audioCounter.IsStop == false && _audio.isActiveAndEnabled)
        {
            _audio.Play();
            startMusic.Invoke();

            yield return new WaitForSeconds(_audioTime);

            endMusic.Invoke();
        }
    }

    public void StopPlay()
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

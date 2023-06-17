using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BallAudio))]
public class BallMover : MonoBehaviour
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
    private BallAudio _ballAudio;
 
    public int Id => _id;
    public Rigidbody Rigidbody => _rigidbody;
    public float MaxSpeed => _maxSpeed;
    public float Speed => _speed;
    public TrailRenderer Train => _trail;
    public int Profitability => _profitability;
    public BallAudio BallAudio => _ballAudio;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ballAudio = GetComponent<BallAudio>();
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
        _ballAudio.Init(audioCounter, audioBar);
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
}

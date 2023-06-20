using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BallAudio))]
public class BallMover : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 5;
    [SerializeField] private TrailRenderer _trail;

    private Rigidbody _rigidbody;
    private Gun _gun;
    private bool _isQueue = false;
    private int _profitability = 1;
    private float _speed;
    private Buffer _buffer;
    private int _id;
    private BallAudio _ballAudio;
 
    public int Id => _id;
    public bool IsQueue => _isQueue;
    public Rigidbody Rigidbody => _rigidbody;
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
    }

    public void Init(Gun gun, int profitability, Color newColor, Buffer buffer, int id, AudioCounter audioCounter, AudioBar audioBar)
    {
        _gun = gun;
        _buffer = buffer;
        _profitability = profitability;
        _id = id;
        _ballAudio.Init(audioCounter, audioBar);
        transform.position = _buffer.transform.position;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = newColor;
    }

    public void ReturnBall()
    {
        ChangeStateOn();
        ChangeAnimationOff();
        _gun.AddBalls(_buffer.GetBall(this));
    }

    public void ChangeStateOn()
    {
        _rigidbody.isKinematic = true;
        _isQueue = true;
    }

    public void ChangeStateOff()
    {
        _rigidbody.isKinematic = false;
        _isQueue = false;
    }

    public void AddForceBalls(Vector3 force)
    {
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }

    private void ChangeAnimationOff()
    {
        _trail.enabled = false;
    }
}

using BouncingBalls.GameSystem;
using BouncingBalls.View;
using System.Collections;
using UnityEngine;

namespace BouncingBalls.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour, IInitializable
    {
        [SerializeField] private float _maxSpeed = 5;
        [SerializeField] private TrailRenderer _trailRenderer;

        private int _profitability = 1;
        private float _speed;

        private Gun _gun;
        private Rigidbody _rigidbody;
        private Renderer _renderer;

        public Rigidbody Rigidbody => _rigidbody;
        public int Profitability => _profitability;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _speed = _rigidbody.velocity.magnitude;

            if (_speed > _maxSpeed)
                _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
        }

        public void Initialize(int factor)
        {
            _profitability = factor;

            _renderer = GetComponent<Renderer>();
        }

        public void PostInitialize(Gun gun)
        {
            _gun = gun;
        }

        public void ResetObject()
        {
            _renderer.enabled = true;
            _trailRenderer.gameObject.SetActive(true);
            _rigidbody.isKinematic = false;
        }

        public void ReturnBall()
        {
            ResetBall();
            StartCoroutine(TimeWaitReturnBall());
        }

        public void ResetBall()
        {
            StopAllCoroutines();
            _renderer.enabled = false;
            _trailRenderer.gameObject.SetActive(false);
            _trailRenderer.Clear();
            _rigidbody.isKinematic = true;
        }

        private IEnumerator TimeWaitReturnBall()
        {
            yield return null;

            _gun.AddBall(this);
        }
    }

}
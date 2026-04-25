using BouncingBalls.LevelSystem;
using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls.GameSystem
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private float _timeToShot;
        [SerializeField] private float _force;
        [SerializeField] private float _rotationRange = 75;
        [Space]
        [SerializeField] private float _forceBounce = 1;

        private float _time = 0;
        private bool _isStop = true;
        private Queue<Ball.Ball> _queueBall = new();

        private void Update()
        {
            if (!_isStop)
            {
                if (_time >= _timeToShot && _queueBall.Count > 0)
                    Shoot();

                _time += Time.deltaTime;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Ball.Ball ball))
            {
                Rigidbody rbBall = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 normal = -collision.contacts[0].normal;
                Vector3 reflection = Vector3.Reflect(collision.relativeVelocity.normalized, normal);
                rbBall.velocity = reflection.normalized * _forceBounce;
            }
        }

        public void AddBall(Ball.Ball ball)
        {
            if (ball == null)
                return;

            _queueBall.Enqueue(ball);
            ball.transform.position = transform.GetChild(0).position;
        }

        public void ChangePosition(Cell cell, int leftOrRight)
        {
            int halfCircle = 180;
            transform.position = cell.GetPointPosition();
            float finalRotation = Random.Range(-_rotationRange, _rotationRange);

            if (leftOrRight == 0)
                finalRotation = finalRotation + halfCircle;

            transform.eulerAngles = new Vector3(0, 0, finalRotation);
        }

        public void StartShoot()
        {
            _isStop = false;
        }

        public void StopShoot()
        {
            _isStop = true;
        }

        public void FullReset()
        {
            _queueBall.Clear();
        }

        private void Shoot()
        {
            Ball.Ball ball = _queueBall.Dequeue();

            if (ball != null)
            {
                ball.transform.position = transform.GetChild(1).position;
                ball.gameObject.SetActive(true);
                ball.ResetObject();
                ball.Rigidbody.velocity = Vector3.zero;
                ball.Rigidbody.AddForce(CalculeitDirection() * _force, ForceMode.Impulse);

                _time = 0;
            }
        }

        private Vector3 CalculeitDirection()
        {
            return (transform.GetChild(1).position - transform.GetChild(0).position).normalized;
        }
    }
}
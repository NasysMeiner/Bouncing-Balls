using BouncingBalls.Enums;
using BouncingBalls.GameSystem;
using BouncingBalls.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls.Ball
{
    public class BallManager : MonoBehaviour
    {
        private Factory _factory;
        private Gun _gun;

        private List<Ball> _activeBalls = new List<Ball>();

        public void Initialize(Factory factory, Gun gun)
        {
            _factory = factory;
            _gun = gun;
        }

        public void CreateStartBalls(int level, int count)
        {
            for (int i = 0; i < count; i++)
                CreateBall(level);
        }

        public void CreateBall(int level)
        {
            Ball newBall = _factory.GetRandomBall(level);
            newBall.PostInitialize(_gun);
            _activeBalls.Add(newBall);
            _gun.AddBall(newBall);
        }

        public void FullReset()
        {
            foreach (Ball ball in _activeBalls)
            {
                ball.ResetBall();
                PoolManager.Instance.SetObject(ball, ObjectType.Ball);
            }

            _activeBalls.Clear();
        }
    }
}
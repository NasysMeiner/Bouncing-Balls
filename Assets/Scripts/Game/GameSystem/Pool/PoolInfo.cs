using BouncingBalls.Enums;
using System;
using UnityEngine;

namespace BouncingBalls.Pool
{
    [Serializable]
    public class PoolInfo
    {
        public ObjectType ObjectType;
        public MonoBehaviour Prefab;
        public int Count;
        public Transform Parent;
    }
}
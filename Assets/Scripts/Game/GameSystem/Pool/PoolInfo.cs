using System;
using UnityEngine;

namespace BouncingBalls
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
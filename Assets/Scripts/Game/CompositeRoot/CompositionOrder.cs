using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls
{
    public class CompositionOrder : MonoBehaviour
    {
        [SerializeField] List<CompositeRoot> _compositeRoots;

        private void Awake()
        {
            foreach (var compositeRoot in _compositeRoots)
            {
                compositeRoot.Compose();
            }
        }
    }
}
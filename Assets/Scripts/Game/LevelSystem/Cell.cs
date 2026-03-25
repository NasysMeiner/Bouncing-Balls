using UnityEngine;

namespace BouncingBalls
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private float _addZPosition = -0.05f;

        private bool _isBusy = false;
        private bool _isStock = false;

        public bool IsBusy => _isBusy;
        public bool IsStock => _isStock;

        public void SpawnCell(bool IsStock)
        {
            _isStock = IsStock;
        }

        public void TakeCell()
        {
            _isBusy = true;
        }

        public void ReleaseCell()
        {
            _isBusy = false;
        }

        public bool CheckInRadiusCell(Vector3 targetPosition, float radius)
        {
            targetPosition.z = transform.position.z;

            return (targetPosition - transform.position).magnitude <= radius;
        }

        public Vector3 GetPointPosition()
        {
            return new Vector3(transform.position.x, transform.position.y, transform.position.z + _addZPosition);
        }
    }
}
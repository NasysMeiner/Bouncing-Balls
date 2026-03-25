using UnityEngine;
using UnityEngine.Events;

namespace BouncingBalls
{
    public class ScreenPosition : MonoBehaviour
    {
        [Header("Horizontal")]
        [SerializeField] private Vector3 _cameraPositionHorizontal;
        [SerializeField] private Vector3 _fieldPositionHorizontal;
        [SerializeField] private Vector3 _stockFieldPositionHorizontal;
        [SerializeField] private Vector3 _blockDeleterPostionHorizontal;
        [Header("Vertical")]
        [SerializeField] private Vector3 _cameraPositionVertical;
        [SerializeField] private Vector3 _fieldPositionVertical;
        [SerializeField] private Vector3 _stockFieldPositionVertical;
        [SerializeField] private Vector3 _blockDeleterPostionVertical;
        [Space]
        [SerializeField] private PlayField _playField;
        [SerializeField] private BlockManager _stockBlocks;
        [SerializeField] private BlockDeleter _blockDeleter;

        private Camera _camera;

        private bool _isLockRotation = true;

        private ScreenOrientation _currentScreenOrientation;

        public event UnityAction ChangeScreenOrientation;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (!_isLockRotation && Screen.orientation != _currentScreenOrientation)
                ScreenOrientationChange();
        }

        public void FixPositionField(int level)
        {
            ScreenOrientationChange();
            _isLockRotation = false;
        }

        private void ScreenOrientationChange()
        {
            Screen.orientation = ScreenOrientation.AutoRotation;

            if (Screen.orientation == ScreenOrientation.Portrait && Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            {
                _camera.transform.position = _cameraPositionVertical;
                _playField.transform.position = _fieldPositionVertical;
                _playField.StockFieldObject.position = _stockFieldPositionVertical;
                _playField.StockFieldObject.rotation = Quaternion.Euler(0, 0, 0);
                _blockDeleterPostionVertical.z = _blockDeleter.transform.position.z;
                _blockDeleter.transform.position = _blockDeleterPostionVertical;
            }
            else
            {
                _camera.transform.position = _cameraPositionHorizontal;
                _playField.transform.position = _fieldPositionHorizontal;
                _playField.StockFieldObject.position = _stockFieldPositionHorizontal;
                _playField.StockFieldObject.rotation = Quaternion.Euler(0, 0, 90);
                _blockDeleterPostionHorizontal.z = _blockDeleter.transform.position.z;
                _blockDeleter.transform.position = _blockDeleterPostionHorizontal;
            }

            _currentScreenOrientation = Screen.orientation;
        }
    }
}
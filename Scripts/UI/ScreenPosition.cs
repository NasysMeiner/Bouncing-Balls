using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenPosition : MonoBehaviour
{
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private List<Vector3> _cameraPositions;
    [SerializeField] private Camera _camera;
    [SerializeField] private List<Vector3> _playFieldPosition;
    [SerializeField] private PlayField _playField;
    [SerializeField] private List<Vector3> _stockBlocksPosition;
    [SerializeField] private StockBlocks _stockBlocks;
    [SerializeField] private List<Vector3> _deleteFieldPosition;
    [SerializeField] private DeleteField _deleateField;

    private ScreenOrientation _deviceOrientation1;
    private ScreenOrientation _deviceOrientation2;
    private int _numberPosition;
    private bool _isGorizontal;

    public event UnityAction ChangeScreenOrientation;

    public List<Vector3> PlayFieldPosition => _playFieldPosition;
    public bool IsGorizontal => _isGorizontal;
    public Camera Camera => _camera;

    private void Start()
    {
        int value = 0;
        Screen.orientation = ScreenOrientation.AutoRotation;

        if (Screen.orientation == ScreenOrientation.Portrait && Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            _deviceOrientation1 = ScreenOrientation.LandscapeLeft;
            _deviceOrientation2 = ScreenOrientation.LandscapeRight;
            _stockBlocks.transform.eulerAngles += new Vector3(0, 0, -90);
            _numberPosition = 0;
        }
        else
        {
            _deviceOrientation1 = ScreenOrientation.Portrait;
            _deviceOrientation2 = ScreenOrientation.PortraitUpsideDown;
            _numberPosition = 1;
        }

        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            _isGorizontal = true;
            value = _playerInfo.Level;
        }

        _camera.transform.position = _cameraPositions[_numberPosition - 1];
        _playField.transform.position = _playFieldPosition[_playerInfo.Level - value];
        _stockBlocks.transform.position = _stockBlocksPosition[_numberPosition - 1];
        _deleateField.transform.position = _deleteFieldPosition[_numberPosition + 1];
    }

    private void Update()
    {
        int value = 0;
        Screen.orientation = ScreenOrientation.AutoRotation;

        if (Screen.orientation == _deviceOrientation1 || Screen.orientation == _deviceOrientation2)
        {

            if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                _isGorizontal = true;
                value = _playerInfo.Level;
            }

            _camera.transform.position = _cameraPositions[_numberPosition];
            _playField.transform.position = _playFieldPosition[_playerInfo.Level - value];
            _stockBlocks.transform.position = _stockBlocksPosition[_numberPosition];

            if (_deleateField.IsUnlock == false)
            {
                _numberPosition += 2;
            }

            Debug.Log(_numberPosition);

            _deleateField.transform.position = _deleteFieldPosition[_numberPosition];

            if (_deviceOrientation1 == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            {
                _deviceOrientation1 = ScreenOrientation.LandscapeLeft;
                _deviceOrientation2 = ScreenOrientation.LandscapeRight;
                _stockBlocks.transform.eulerAngles = new Vector3(0, 0, 270);
                _isGorizontal = false;
                _numberPosition = 0;
            }
            else
            {
                _deviceOrientation1 = ScreenOrientation.Portrait;
                _deviceOrientation2 = ScreenOrientation.PortraitUpsideDown;
                _stockBlocks.transform.eulerAngles += new Vector3(0, 0, 90);
                _numberPosition = 1;
            }

            ChangeScreenOrientation?.Invoke();
        }
    }
}

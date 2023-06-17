using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Block))]

public class Draggable : MonoBehaviour
{
    [SerializeField] private Color _newColor;
    [SerializeField] private Cells _cells;
    [SerializeField] private float _radiusCell;
    [SerializeField] private float _radiuseDelete;

    private DeleteField _deleateField;
    private float _startPositionZ;
    private float _screenCameraDistance;
    private Block _block;
    private Vector3 _mousePositionNearClipPlane;
    private Color _startColor;
    private Vector3 _position;
    private Renderer _renderer;
    private float _horizontalPositionCamera = 6.2f;
    private float _verticalPositionCamera = 10.6f;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _block = GetComponent<Block>();
        _deleateField = _block.Delete;
        _newColor = _renderer.material.color;
        _newColor.a = 0.5f;
        _screenCameraDistance = Camera.main.nearClipPlane + _verticalPositionCamera;
        _startColor = _renderer.material.color;
        _startPositionZ = transform.position.z;
    }

    private void OnMouseDown()
    {
        if(_block.ScreenPosition.Camera.transform.position.z < -_verticalPositionCamera)
        {
            _screenCameraDistance = Camera.main.nearClipPlane + _verticalPositionCamera;
        }
        else
        {
            _screenCameraDistance = Camera.main.nearClipPlane + _horizontalPositionCamera;
        }

        _cells = _block.Cells;
        _mousePositionNearClipPlane = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenCameraDistance);
        _position = Camera.main.ScreenToWorldPoint(_mousePositionNearClipPlane);
        transform.position = _position;
        _renderer.material.color = _newColor;
    }

    private void OnMouseDrag()
    {
        _mousePositionNearClipPlane = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenCameraDistance);
        _position = Camera.main.ScreenToWorldPoint(_mousePositionNearClipPlane);
        transform.position = _position;
    }

    private void OnMouseUp()
    {
        _renderer.material.color = _startColor;

        if(Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(_deleateField.transform.position.x, _deleateField.transform.position.y)) <= _radiuseDelete && _deleateField.IsUnlock && _block.StockBlocks.Blocks.Count > 3)
        {
            _block.DeleteBlock();

            return;
        }

        foreach (Cell point in _cells.TotalNumberCells)
        {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(point.transform.position.x, point.transform.position.y)) <= _radiusCell && point.IsBusy == false)
            {
                _block.CurrentCell.ReleaseCell();
                _block.ChangeCell(point, new Vector3(point.transform.position.x, point.transform.position.y, _startPositionZ));

                return;
            }
        }

        transform.position = new Vector3(_block.CurrentCell.transform.position.x, _block.CurrentCell.transform.position.y, _startPositionZ);
    }
}

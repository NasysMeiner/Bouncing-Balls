using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Block))]
public class Draggable : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float _alphaColorValue;
    [SerializeField] private float _radiusCell;

    private Renderer _renderer;
    private Color _startColor;
    private Color _changeColor;

    private Block _block;
    private Camera _mainCamera;

    private bool _isBlockDeleterArea = false;
    private float _verticalPositionCamera = 0.6f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out BlockDeleter blockDeleter) && blockDeleter.IsUnlock)
            _isBlockDeleterArea = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out BlockDeleter _))
            _isBlockDeleterArea = false;
    }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _block = GetComponent<Block>();

        _startColor = _renderer.material.color;
        _changeColor = _startColor;
        _changeColor.a = _alphaColorValue;

        _mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        _renderer.material.color = _changeColor;
    }

    private void OnMouseDrag()
    {
        Move();
    }

    private void OnMouseUp()
    {
        _renderer.material.color = _startColor;

        if (_isBlockDeleterArea)
            _block.DeleteBlock();
        else if (_block.Cells.TryGetCellFromRadius(transform.position, _radiusCell, out Cell newCell))
            _block.ChangeCell(newCell);
        else
            transform.position = _block.CurrentCell.GetPointPosition();
    }

    private void Move()
    {
        float zPos = -_mainCamera.transform.position.z + _block.CurrentCell.GetPointPosition().z - _verticalPositionCamera;
        Vector3 newPosition = new(
            Input.mousePosition.x,
            Input.mousePosition.y,
            zPos
        );
        transform.position = _mainCamera.ScreenToWorldPoint(newPosition);
    }
}

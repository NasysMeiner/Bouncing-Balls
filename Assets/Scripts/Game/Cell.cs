using UnityEngine;

public class Cell : MonoBehaviour 
{
    private float _zPosition = 0.12f;
    private Block _cell;
    private Vector3 _transformBlock;
    private bool _isBusy = false;
    private bool _isStock = false;
    private bool _isSet = false;
    //private int _id;

    public bool IsBusy => _isBusy;
    public Block Block => _cell;
    public bool IsStock => _isStock;
    public bool IsSet => _isSet;

    public void SpawnCell(Vector3 transformBlock, bool IsStock)
    {
        _transformBlock = transformBlock;
        _isStock = IsStock;
        transform.position = new Vector3(_transformBlock.x, _transformBlock.y, _zPosition);
        //_id = id;
    }

    public void TakeCell(Block Block = null, Gun gun = null)
    {
        if(Block != null)
        {
            _cell = Block;
            _isBusy = true;
        }
        else if(gun != null)
        {
            _isBusy = true;
        }
    }

    public void ChangePutField(bool isBusyField)
    {
        _isSet = isBusyField;
    }

    public void ReleaseCell()
    {
        _cell = null;
        _isBusy = false;
    }
}

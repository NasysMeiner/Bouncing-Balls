using UnityEngine;

public class Cell : MonoBehaviour
{
    private bool _isBusy = false;
    private bool _isStock = false;

    public bool IsBusy => _isBusy;
    public bool IsStock => _isStock;

    public void SpawnCell(Vector3 transformBlock, bool IsStock)
    {
        _isStock = IsStock;
        transform.position = new Vector3(transformBlock.x, transformBlock.y, transformBlock.z);
    }

    public void TakeCell(Block block = null, Gun gun = null)
    {
        _isBusy = true;
    }

    public void ReleaseCell()
    {
        _isBusy = false;
    }
}

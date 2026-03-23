using BouncingBalls;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    [SerializeField] private List<Vector2> _fieldXY = new List<Vector2>();
    [SerializeField] private Vector2 _stockFieldXY;
    [SerializeField] private GameObject _stockFieldObject;
    [SerializeField] private float _stepCell;
    [Space]
    [SerializeField] private Cells _totalNumberCells;

    private List<Cell> _cellsPlayField;
    private List<Cell> _cellsStockField;

    private Vector2 _currentFieldXY;

    public Transform StockFieldObject => _stockFieldObject.transform;
    public Cells Cells => _totalNumberCells;

    public bool TryEmptyCell(out Cell emptyCell)
    {
        emptyCell = _cellsStockField.FirstOrDefault(cell => !cell.IsBusy);
        return emptyCell != null;
    }

    public void GenerateField(int level, Gun gun)
    {
        _currentFieldXY = _fieldXY[level];
        _cellsPlayField = SpawnFieldFromParent(_currentFieldXY, transform);
        _cellsStockField = SpawnFieldFromParent(_stockFieldXY, _stockFieldObject.transform, true);
        ChangeGunPosition(gun);
    }

    private List<Cell> SpawnFieldFromParent(Vector2 fieldXY, Transform parent, bool isStock = false)
    {
        List<Cell> newCells = new List<Cell>();

        for (int y = 0; y < fieldXY.y; y++)
        {
            for (int x = 0; x < fieldXY.x; x++)
            {
                Cell cell = PoolManager.Instance.GetObject<Cell>(ObjectType.Cell);
                cell.SpawnCell(isStock);
                cell.transform.parent = parent;
                cell.gameObject.SetActive(true);
                cell.transform.position = new Vector3(parent.position.x + x * _stepCell, parent.position.y + y * _stepCell, parent.position.z);
                newCells.Add(cell);
                _totalNumberCells.AddCell(cell);
            }
        }

        return newCells;
    }

    private void ChangeGunPosition(Gun gun)
    {
        float newPositionXGun;
        int lefrOrRight = Random.Range(0, 2);

        if (lefrOrRight == 0)
            newPositionXGun = transform.position.x;
        else
            newPositionXGun = transform.position.x + _stepCell * (_currentFieldXY.x - 1);

        int hight = (int)Random.Range(1, _currentFieldXY.y - 1);
        Vector3 gunPosition = new (newPositionXGun, transform.position.y + _stepCell * hight, transform.position.z);
        Cell currentCell = _totalNumberCells.GetCellFromPosition(gunPosition);
        currentCell.TakeCell();
        gun.ChangePosition(currentCell, lefrOrRight);
    }

    internal void FullReset()
    {
        foreach (Cell cell in _cellsPlayField)
        {
            cell.ReleaseCell();
            PoolManager.Instance.SetObject(cell, ObjectType.Cell);
        }

        foreach (Cell cell in _cellsStockField)
        {
            cell.ReleaseCell();
            PoolManager.Instance.SetObject(cell, ObjectType.Cell);
        }

        _stockFieldObject.transform.rotation = Quaternion.identity;
    }
}

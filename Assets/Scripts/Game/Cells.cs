using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cells : MonoBehaviour
{
    [SerializeField] private HashSet<Cell> _cells = new();

    public HashSet<Cell> CellArray => _cells;

    public void AddCell(Cell newCell)
    {
        _cells.Add(newCell);
    }

    public Cell GetCellFromPosition(Vector3 cellPosition)
    {
        foreach (Cell cell in _cells)
            if (cellPosition == cell.transform.position)
                return cell;

        return null;
    }

    public bool TryGetCellFromRadius(Vector3 position, float radius, out Cell cell)
    {
        cell = _cells.FirstOrDefault(c => !c.IsBusy && c.CheckInRadiusCell(position, radius));

        return cell != null;
    }
}

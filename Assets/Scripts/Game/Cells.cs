using System.Collections.Generic;
using UnityEngine;

public class Cells : MonoBehaviour
{
    [SerializeField] private HashSet<Cell> _cells = new HashSet<Cell>();

    public HashSet<Cell> TotalNumberCells => _cells;

    public void AddCell(Cell newCell)
    {
        _cells.Add(newCell);
    }

    public Cell SerachCell(Vector3 currentCell)
    {
        foreach (Cell cell in _cells)
        {
            if (currentCell == cell.transform.position)
                return cell;
        }

        return null;
    }
}

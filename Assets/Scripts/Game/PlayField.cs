using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    [SerializeField] private Cells _totalNumberCells;
    [SerializeField] private Cell _prefabCell;
    [SerializeField] private float _fieldX;
    [SerializeField] private float _fieldY;
    [SerializeField] private List<Vector2> _fieldXY = new List<Vector2>();
    [SerializeField] private float _stepCell;
    [SerializeField] private Point _prefabPoint;
    [SerializeField] private Gun _gun;
    [SerializeField] private Vector3 _pointCenterField;
    [SerializeField] private Game _game;

    private List<Cell> _cellsField = new List<Cell>();
    private List<Cell> _openCells = new List<Cell>();

    private void OnEnable()
    {
        _game.GenerationStart += ResetField;
        _game.StartGame += OnStartGame;
    }

    private void OnDisable()
    {
        _game.GenerationStart -= ResetField;
        _game.StartGame -= OnStartGame;
    }

    private void OnStartGame()
    {
        SetDimensionsField();
        SpawnCell();
        ChangeGunPosition();
    }

    private void SpawnCell()
    {
        for (int y = 0; y < _fieldY; y++)
        {
            for (int x = 0; x < _fieldX; x++)
            {
                if (_openCells.Count > 0)
                {
                    foreach(Cell cell in _openCells)
                    {
                        if(cell.IsSet == false)
                        {
                            cell.transform.position = new Vector3(transform.position.x + x * _stepCell, transform.position.y + y * _stepCell, transform.position.z);
                            cell.ChangePutField(true);
                            _cellsField.Add(cell);
                            _openCells.Remove(cell);

                            break;
                        }
                    }
                }
                else
                {
                    Cell newCell = Instantiate(_prefabCell, transform);
                    newCell.ChangePutField(true);
                    Vector3 positionNewCell = new Vector3(transform.position.x + x * _stepCell, transform.position.y + y * _stepCell, 0);
                    newCell.SpawnCell(positionNewCell, false);
                    _cellsField.Add(newCell);
                    _totalNumberCells.AddCell(newCell);
                }
            }
        }

        if(_game.IsGorizontal == false)
        {
            transform.position = _game.PlayFieldPosition[_game.Level];
        }
    }

    private void ResetField()
    {
        foreach(Cell cell in _cellsField)
        {
            cell.ChangePutField(false);
            cell.ReleaseCell();
            _openCells.Add(cell);
        }

        _cellsField.Clear();
        SetDimensionsField();
        SpawnCell();
        ChangeGunPosition();
    }

    private void SetDimensionsField()
    {
        Vector2 newField = _fieldXY[_game.Level - 1];
        _fieldX = newField.x;
        _fieldY = newField.y;
    }

    private void ChangeGunPosition()
    {
        float newPositionXGun;
        int lefrOrRight = Random.Range(0, 2);

        if (lefrOrRight == 0)
            newPositionXGun = transform.position.x;
        else
            newPositionXGun = transform.position.x + _stepCell * (_fieldX - 1);

        int hight = (int)Random.Range(1, _fieldY - 1);
        Cell currentCell = _totalNumberCells.SerachCell(new Vector3(newPositionXGun, transform.position.y + _stepCell * hight, transform.position.z));
        currentCell.TakeCell(null, _gun);
        _gun.ChangePosition(currentCell, lefrOrRight);
    }
}

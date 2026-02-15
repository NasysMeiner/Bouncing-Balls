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
    [SerializeField] private Gun _gun;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private ScreenPosition _screenPosition;

    private List<Cell> _cellsField = new List<Cell>();
    private Queue<Cell> _openCells = new();

    private void OnEnable()
    {
        _levelLoader.GenerationStarting += ResetField;
        _levelLoader.GameStarting += OnStartGame;
    }

    private void OnDisable()
    {
        _levelLoader.GenerationStarting -= ResetField;
        _levelLoader.GameStarting -= OnStartGame;
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
                    Cell cell = _openCells.Dequeue();
                    cell.transform.position = new Vector3(transform.position.x + x * _stepCell, transform.position.y + y * _stepCell, transform.position.z);
                    _cellsField.Add(cell);
                    Debug.Log(cell.transform.position);
                }
                else
                {
                    Cell newCell = Instantiate(_prefabCell, transform);
                    _openCells.Enqueue(newCell);
                    _totalNumberCells.AddCell(newCell);
                    x--;
                }
            }
        }

        if (_screenPosition.IsGorizontal == false)
            transform.position = _screenPosition.PlayFieldPosition[_playerInfo.Level];
    }

    private void ResetField()
    {
        foreach (Cell cell in _cellsField)
        {
            cell.ReleaseCell();
            _openCells.Enqueue(cell);
        }

        _cellsField.Clear();
        SetDimensionsField();
        SpawnCell();
        ChangeGunPosition();
    }

    private void SetDimensionsField()
    {
        Vector2 newField = _fieldXY[_playerInfo.Level - 1];
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

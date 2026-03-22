using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _startLevel = 0;

    private LevelManager _levelManager;

    private int _currentLevel;

    public void InitManager(LevelManager levelManager)
    {
        _levelManager = levelManager;

        _currentLevel = _startLevel;
    }

    public void CreateLevel()
    {
        _levelManager.GenerateLevel(_startLevel);
    }
}

using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private PlayField _playField;

    public event Action StartedGame;
    public event Action EndedGame;

    public void InitLevelManager(PlayField playField)
    {
        _playField = playField;
    }

    public void GenerateLevel(int level)
    {
        _playField.GenerateField(level);
    }
}

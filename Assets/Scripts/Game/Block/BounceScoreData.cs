using UnityEngine;

public class BounceScoreData
{
    public int Score;
    public int Cristall;
    public Vector3 BouncePosition;

    public BounceScoreData(int score, int cristall, Vector3 bouncePosition)
    {
        Score = score;
        Cristall = cristall;
        BouncePosition = bouncePosition;
    }
}

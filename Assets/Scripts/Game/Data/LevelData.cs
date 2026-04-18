using BouncingBalls.Enums;

namespace BouncingBalls.Data
{
    public class LevelData
    {
        public int TotalScore;
        public int Score;
        public int BounceCount;
        public int TotalMoney;
        public int TimeInLevel;

        public int GetStat(StatType statType)
        {
            switch (statType)
            {
                case StatType.TotalScore:
                    return TotalScore;
                case StatType.Score:
                    return Score;
                case StatType.BounceCount:
                    return BounceCount;
                case StatType.TotalMoney:
                    return TotalMoney;
                case StatType.TimeInLevel:
                    return TimeInLevel;
                default:
                    return 0;
            }
        }
    }
}
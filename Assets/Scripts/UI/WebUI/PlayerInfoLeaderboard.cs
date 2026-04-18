namespace BouncingBalls.WebSystem
{
    public class PlayerInfoLeaderboard
    {
        public string Name { get; private set; }
        public int Score { get; private set; }
        public int Rank { get; private set; }

        public PlayerInfoLeaderboard(string name, int score, int rank)
        {
            Name = name;
            Score = score;
            Rank = rank;
        }
    }
}
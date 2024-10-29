namespace Jackpotless
{
    [System.Serializable]
    public class UserData
    {
        public int HighScore;
        public int Wins;

        public UserData(int highScore, int wins)
        {
            this.HighScore = highScore;
            this.Wins = wins;
        }
    }
}

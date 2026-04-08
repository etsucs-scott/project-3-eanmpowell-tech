using System.Diagnostics.Tracing;
//How the highscore is created, and shows different scores based on the field size
namespace Minesweeper.Core
{
    public class HighScore
    {
        public int Size { get; set; }
        public int Seconds { get; set; }
        public int Moves { get; set; }
        public int Seed { get; set; }
        public string Timestamp { get; set; }
    }
}
//Uses board size, time spent, amount of moves, and seed to calculate and display score
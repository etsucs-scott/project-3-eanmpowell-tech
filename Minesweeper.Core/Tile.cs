namespace Minesweeper.Core
//Shows the info of each tile, including whether or not its a bomb, flagged, revealed, or how many bombs are next to it.
{
    public class Tile
    {
        public bool IsBomb { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public int AdjacentMines { get; set; }
    }
}
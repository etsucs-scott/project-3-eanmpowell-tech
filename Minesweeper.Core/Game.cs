namespace Minesweeper.Core
{
    public class Game
    {
        public Board Board { get; }

        public bool IsGameOver { get; private set; }
        public bool IsWin { get; private set; }

        private int totalSafeTiles;
        private int revealedSafeTiles;

        public Game(int size, int seed)
        {
            Board = new Board(size, seed);

            totalSafeTiles = 0;

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (!Board.GetTile(r, c).IsBomb)
                    {
                        totalSafeTiles++;
                    }
                }
            }

            revealedSafeTiles = 0;
        }

        public void Reveal(int row, int col)
        {
            if (IsGameOver)
                return;

            var tile = Board.GetTile(row, col);

            // 🔹 Cannot reveal flagged tiles
            if (tile.IsFlagged || tile.IsRevealed)
                return;

            FloodReveal(row, col);

            CheckWin();
        }

        public void ToggleFlag(int row, int col)
        {
            if (IsGameOver)
                return;

            var tile = Board.GetTile(row, col);

            // 🔹 Only allow flagging hidden tiles
            if (!tile.IsRevealed)
            {
                tile.IsFlagged = !tile.IsFlagged;
            }
        }

        // 🔹 Flood fill for zero-adjacent tiles
        private void FloodReveal(int row, int col)
        {
            var tile = Board.GetTile(row, col);

            if (tile.IsRevealed || tile.IsFlagged)
                return;

            tile.IsRevealed = true;

            // 🔹 If bomb → game over
            if (tile.IsBomb)
            {
                IsGameOver = true;
                IsWin = false;
                return;
            }

            revealedSafeTiles++;

            // 🔹 If not zero, stop expansion
            if (tile.AdjacentMines > 0)
                return;

            // 🔹 Explore neighbors (8 directions)
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0)
                        continue;

                    int newRow = row + dr;
                    int newCol = col + dc;

                    // Bounds check
                    if (newRow >= 0 && newRow < Board.Size &&
                        newCol >= 0 && newCol < Board.Size)
                    {
                        FloodReveal(newRow, newCol);
                    }
                }
            }
        }

        private void CheckWin()
        {
            if (revealedSafeTiles == totalSafeTiles)
            {
                IsGameOver = true;
                IsWin = true;
            }
        }
    }
}
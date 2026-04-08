using System;

namespace Minesweeper.Core
{
    public class Board
    {
        private Tile[,] tiles;

        public int Size { get; }

        public Board(int size, int seed)
        {
            Size = size;
            tiles = new Tile[size, size];

            // Initialize tiles
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    tiles[r, c] = new Tile();
                }
            }

            GenerateMines(seed);
            CalculateAdjacentMines();
        }

        // 🔹 THIS fixes your error
        public Tile GetTile(int row, int col)
        {
            return tiles[row, col];
        }

        private void GenerateMines(int seed)
        {
            Random rand = new Random(seed);

            int mineCount = Size switch
            {
                8 => 10,
                12 => 25,
                16 => 40,
                _ => 10
            };

            int placed = 0;

            while (placed < mineCount)
            {
                int r = rand.Next(Size);
                int c = rand.Next(Size);

                if (!tiles[r, c].IsBomb)
                {
                    tiles[r, c].IsBomb = true;
                    placed++;
                }
            }
        }

        private void CalculateAdjacentMines()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    if (tiles[r, c].IsBomb)
                        continue;

                    int count = 0;

                    for (int dr = -1; dr <= 1; dr++)
                    {
                        for (int dc = -1; dc <= 1; dc++)
                        {
                            int nr = r + dr;
                            int nc = c + dc;

                            if (nr >= 0 && nr < Size &&
                                nc >= 0 && nc < Size &&
                                tiles[nr, nc].IsBomb)
                            {
                                count++;
                            }
                        }
                    }

                    tiles[r, c].AdjacentMines = count;
                }
            }
        }
    }
}
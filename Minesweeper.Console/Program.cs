using System;
using System.Diagnostics;
using Minesweeper.Core;
//main program, what holds everything together as it all runs.

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Minesweeper ===");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            if (choice == "2")
                break;

            if (choice != "1")
                continue;

            StartGame();
        }
    }

    static void StartGame()
    {
        // 🔹 Board size
        Console.WriteLine("Choose board size (8, 12, 16):");
        int size;

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out size) &&
                (size == 8 || size == 12 || size == 16))
            {
                break;
            }

            Console.WriteLine("Invalid size. Enter 8, 12, or 16:");
        }

        // 🔹 Seed
        Console.WriteLine("Enter seed (or press Enter for random):");
        string seedInput = Console.ReadLine();

        int seed;

        if (string.IsNullOrWhiteSpace(seedInput))
        {
            seed = DateTime.Now.Millisecond +
                   DateTime.Now.Second * 1000 +
                   DateTime.Now.Minute * 60000;

            Console.WriteLine($"Using generated seed: {seed}");
        }
        else
        {
            while (!int.TryParse(seedInput, out seed))
            {
                Console.WriteLine("Invalid seed. Enter a number:");
                seedInput = Console.ReadLine();
            }

            Console.WriteLine($"Using seed: {seed}");
        }

        Game game = new Game(size, seed);

        Stopwatch timer = new Stopwatch();
        timer.Start();

        int moves = 0;

        while (!game.IsGameOver)
        {
            Console.Clear();
            PrintBoard(game);

            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("r row col  → reveal");
            Console.WriteLine("f row col  → flag");
            Console.WriteLine("q          → quit");
            Console.WriteLine();

            string inputLine = Console.ReadLine().Trim();
            string[] input = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (input.Length == 0)
                continue;

            string command = input[0].ToLower();

            if (command == "q")
                return;

            if (input.Length != 3)
            {
                Console.WriteLine("Invalid input. Press Enter...");
                Console.ReadLine();
                continue;
            }

            if (!int.TryParse(input[1], out int row) ||
                !int.TryParse(input[2], out int col))
            {
                Console.WriteLine("Invalid numbers. Press Enter...");
                Console.ReadLine();
                continue;
            }

            if (row < 0 || row >= size || col < 0 || col >= size)
            {
                Console.WriteLine("Out of bounds. Press Enter...");
                Console.ReadLine();
                continue;
            }

            if (command == "r")
            {
                game.Reveal(row, col);
                moves++;
            }
            else if (command == "f")
            {
                game.ToggleFlag(row, col);
                moves++;
            }
        }

        timer.Stop();
        int seconds = (int)timer.Elapsed.TotalSeconds;

        Console.Clear();
        PrintBoard(game);

        if (game.IsWin)
        {
            Console.WriteLine("🎉 YOU WIN!");
            Console.WriteLine($"Time: {seconds}s | Moves: {moves}");

            try
            {
                HighScoreService service = new HighScoreService();

                service.AddScore(new HighScore
                {
                    Size = size,
                    Seconds = seconds,
                    Moves = moves,
                    Seed = seed,
                    Timestamp = DateTime.Now.ToString("s")
                });

                Console.WriteLine("Score saved!");
            }
            catch
            {
                Console.WriteLine("Failed to save score.");
            }
        }
        else
        {
            Console.WriteLine("💥 GAME OVER");
        }

        Console.WriteLine($"Seed used: {seed}");
        Console.WriteLine("Press Enter to return to menu...");
        Console.ReadLine();
    }

    // 🔹 Board rendering
    static void PrintBoard(Game game)
    {
        for (int r = 0; r < game.Board.Size; r++)
        {
            for (int c = 0; c < game.Board.Size; c++)
            {
                var tile = game.Board.GetTile(r, c);

                if (tile.IsRevealed)
                {
                    if (tile.IsBomb)
                    {
                        Console.Write("b ");
                    }
                    else if (tile.AdjacentMines == 0)
                    {
                        Console.Write(". ");
                    }
                    else
                    {
                        Console.Write(tile.AdjacentMines + " ");
                    }
                }
                else
                {
                    if (tile.IsFlagged)
                        Console.Write("f ");
                    else
                        Console.Write("# ");
                }
            }

            Console.WriteLine();
        }
    }
}
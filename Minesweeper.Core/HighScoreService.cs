using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Minesweeper.Core
{
    public class HighScoreService
    {
        private readonly string filePath = "data/highscores.csv";

        public List<HighScore> LoadScores()
        {
            var scores = new List<HighScore>();

            try
            {
                if (!Directory.Exists("data"))
                    Directory.CreateDirectory("data");

                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, "size,seconds,moves,seed,timestamp\n");
                    return scores;
                }

                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines.Skip(1)) // skip header
                {
                    var parts = line.Split(',');

                    if (parts.Length != 5)
                        continue;

                    scores.Add(new HighScore
                    {
                        Size = int.Parse(parts[0]),
                        Seconds = int.Parse(parts[1]),
                        Moves = int.Parse(parts[2]),
                        Seed = int.Parse(parts[3]),
                        Timestamp = parts[4]
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading scores: " + ex.Message);
            }

            return scores;
        }

        public void SaveScores(List<HighScore> scores)
        {
            try
            {
                var lines = new List<string>
                {
                    "size,seconds,moves,seed,timestamp"
                };

                foreach (var s in scores)
                {
                    lines.Add($"{s.Size},{s.Seconds},{s.Moves},{s.Seed},{s.Timestamp}");
                }

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving scores: " + ex.Message);
            }
        }

        public void AddScore(HighScore newScore)
        {
            var scores = LoadScores();

            scores.Add(newScore);

            // Group by board size
            var grouped = scores
                .GroupBy(s => s.Size)
                .SelectMany(group =>
                    group
                    .OrderBy(s => s.Seconds)   // fastest time first
                    .ThenBy(s => s.Moves)      // fewer moves wins ties
                    .Take(5)                   // top 5 only
                )
                .ToList();

            SaveScores(grouped);
        }
    }
}
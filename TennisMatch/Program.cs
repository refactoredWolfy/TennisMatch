using System;

public class TennisMatch
{
    public string PlayerA { get; private set; }
    public string PlayerB { get; private set; }
    public int ScoreA { get; private set; }
    public int ScoreB { get; private set; }

    public TennisMatch(string playerA, string playerB)
    {
        if (string.IsNullOrWhiteSpace(playerA) || string.IsNullOrWhiteSpace(playerB))
            throw new ArgumentException("Player names cannot be empty");

        PlayerA = playerA;
        PlayerB = playerB;
        ScoreA = 0;
        ScoreB = 0;
    }

    public void AddPoint(string player)
    {
        if (player == "A")
            ScoreA++;
        else if (player == "B")
            ScoreB++;
        else
            throw new ArgumentException("Player must be 'A' or 'B'");
    }

    public string GetGameScore()
    {
        // Check for game win first
        if (ScoreA >= 4 && ScoreA >= ScoreB + 2)
            return $"Game {PlayerA}";

        if (ScoreB >= 4 && ScoreB >= ScoreA + 2)
            return $"Game {PlayerB}";

        // Deuce (both at 3 or more and equal)
        if (ScoreA >= 3 && ScoreB >= 3 && ScoreA == ScoreB)
            return "Deuce";

        // Advantage
        if (ScoreA >= 4 && ScoreA == ScoreB + 1)
            return $"Advantage {PlayerA}";

        if (ScoreB >= 4 && ScoreB == ScoreA + 1)
            return $"Advantage {PlayerB}";

        // Normal scoring (cap at 40 for display)
        string displayScoreA = ScoreA <= 3 ? ConvertScore(ScoreA) : "40";
        string displayScoreB = ScoreB <= 3 ? ConvertScore(ScoreB) : "40";

        return $"{displayScoreA} - {displayScoreB}";
    }

    private string ConvertScore(int score)
    {
        switch (score)
        {
            case 0: return "0";
            case 1: return "15";
            case 2: return "30";
            case 3: return "40";
            default: return "40";
        }
    }

    public bool IsGameOver()
    {
        string score = GetGameScore();
        return score.StartsWith("Game");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Tennis Game Simulator ===\n");

        TennisMatch match = new TennisMatch("Federer", "Nadal");
        Random rnd = new Random();

        int pointNumber = 1;

        // Keep playing until someone wins the game
        while (!match.IsGameOver())
        {
            // Let user choose or random? Let's make it interactive!
            Console.Write($"Point {pointNumber}: Press A for {match.PlayerA} or B for {match.PlayerB} (or R for random): ");
            string input = Console.ReadLine()?.ToUpper();

            string player;
            if (input == "R")
            {
                player = rnd.Next(2) == 0 ? "A" : "B";
                Console.WriteLine($"  Random choice: Player {player}");
            }
            else if (input == "A" || input == "B")
            {
                player = input;
            }
            else
            {
                Console.WriteLine("  Invalid input. Try again.");
                continue;
            }

            match.AddPoint(player);
            Console.WriteLine($"  Score: {match.GetGameScore()}\n");
            pointNumber++;
        }

        // Game is over - display winner
        string finalScore = match.GetGameScore();
        Console.WriteLine($"🏆 {finalScore} 🏆");

        // Show final point statistics
        Console.WriteLine($"\nTotal points played: {pointNumber - 1}");
        Console.WriteLine($"Final score: {match.ScoreA} - {match.ScoreB}");

        // Ask for rematch
        Console.Write("\nPlay another game? (Y/N): ");
        if (Console.ReadLine()?.ToUpper() == "Y")
        {
            Console.Clear();
            Main(args); // Restart (simple recursion - fine for this small demo)
        }
        else
        {
            Console.WriteLine("Thanks for playing! Great effort on your coding journey!");
        }
    }
}
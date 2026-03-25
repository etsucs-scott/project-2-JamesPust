using System;

public class Program
{
    static void Main(string[] args)
    {
        int playerCount = 0;
        if (args.Length > 0 && int.TryParse(args[0], out int argCount) && argCount >= 2 and <= 4){
            playerCount = argCount;
        }
        else
        {
            Output.PrintBanner();

            while (playerCount < 2 or > 4)
            {
                Console.WriteLine("How many players? (2, 3, or 4): ");
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out playerCount) || playerCount < 2 or > 4)
                {
                    Console.WriteLine("Please enter 2, 3, or 4.");
                    playerCount = 0;
                }
            }
        }

        var playerNames = Enumerable.Range(1, playerCount).Select(i => $"Player {i}").ToList();
        var deck = new Deck();
        var engine = new WarCore(playerNames);

        engine.Deal(deck);

        Console.WriteLine("");
        Console.WriteLine($"Starting war with {playerCount} players.");
        Output.PrintDeal(engine.Hands.ToDictionary(kv.Key, kv => kv.Value.Count));

        while(true)
        {
            if (engine.IsGameOver(out GameResult? finalResult))
            {
                Output.PrintGameResults(finalResult);
                break;
            }

            RoundResult round = engine.PlayRound();
            Output.PrintRound(round);
        }


    }
   
}

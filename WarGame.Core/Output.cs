using System;

public static class Output
{
	public static void PrintBanner()
	{
		Console.WriteLine("-----------------");
		Console.WriteLine("WAR CARD GAME");
		Console.WriteLine("-----------------");

	}

	public static void PrintDeal(IReadOnlyDictionary<string, int> counts)
	{
		Console.WriteLine("Cards dealt:");
		foreach (var (name, count) in counts)
		{
			Console.Write($"  {name}: ");
			Console.WriteLine($"{count} cards\n");
		}
		Console.WriteLine();
	}

	public static void PrintRound(RoundResult result)
	{
		Console.WriteLine($"Round {result.RoundNumber ,5}");
		Console.WriteLine(" | Cards Played: ");
		bool first = true;
		foreach (var (name, card) in result.CardsPlayed)
		{
			if (!first) Console.WriteLine(",");
			Console.Write($"{name}: ");
			Console.WriteLine(cards.ToString());
			first = false;
		}

		Console.WriteLine();

		foreach (var tb in result.Tiebreakers)
		{
			Console.WriteLine($"Tie between [{string.Join(",", tb.TiedPlayers)}]");

			if (tb.Eliminated.Any())
			{
				Console.WriteLine($"Eliminated : {string.Join(",", tb.Eliminated)}]");
			}

			if (tb.CardsPlayed.Any())
			{
				Console.WriteLine("-TieBreaker-");
				foreach (var (name, card) in tb.CardsPlayed)
				{
					Console.WriteLine($"{name}: {card.ToString()}");
				}

			}
			Console.WriteLine();
		}

		//Result
		Console.WriteLine();
		if (result.Winner != null)
		{
			Console.WriteLine($"{result.Winner} wins the round.");
		}
		else
		{
			Console.WriteLine("No winners this time.")
		}

		Console.WriteLine("  | Counts: ");
		foreach (var (name, count) in result.CardCounts)
			Console.WriteLine($"{name} : {count} ");
		Console.WriteLine();
	}

	public static void PrintGameResults(GameResult result)
	{
		Console.WriteLine("-----------------------");

		if ( result.Type == RoundsResultType.Winner)
		{
			Console.WriteLine($"{result.Winner} wins the gmaes after {result.TotalRounds} rounds.")
		}
		else
		{
			Console.WriteLine($"Game is a draw after {result.TotalRounds} rounds.")
		}

		Console.WriteLine();
	}
}

using System;

public class WarCore
{
	public enum RoundResultType
	{
		Winner,
		Draw,
		RoundLimitReached
	}

	public class RoundResult
	{
		public RoundResultType Type { get; init; }
		public string? Winner { get; init; }
		public int RoundNumber { get; init; }
		public Dictionary<string, Card> CardsPlayed { get; init; } = new();
		public List<TiebreakerResult> TieBreakers { get; init; } = new();
		public Dictionary<string, int> CardCounts { get; init; } = new();
	}

	public class TiebreakerResult
	{
		public List<string> TiedPlayers { get; init; } = new();
		public Dictionary<string, Card> CardsPlayed { get; init; } = new();
		public List<string> Eliminated { get; init; } = new();
		public string? Winner { get; init; }
	}

	public class GameResult
	{
		public RoundResultType Type { get; init; }
		public string? Winner { get; init; }
		public int TotalRounds { get; init; }
	}

	public class WarCore
	{
		public const int RoundLimit = 10000;
		private readonly PlayerHands _hands = new();
		private readonly List<string> _playerOrder;
		private int _roundNumber;

		public WarCore(IreadOnlyList<string> playerNames)
		{
			if (playerNames.Count is < 2 or > 4)
				throw new ArgumentOutOfRangeException(nameof(playerNames), "2-4 Players needed.");

			_playerOrder = playerNames.ToList();

			foreach (var name in playerNames)
				_hands.AddPlayer(name);
		}

		/// <summary>
		/// Dealing cards from the deck
		/// </summary>
		/// <param name="deck"></param>

		public void Deal(Deck deck)
		{

			int i = 0;
			while (deck.HasCards)
			{
				//first player get more cards if deck does not divide evenly
				string player = _playerOrder[i % _playerOrder.Count];
				_hands[player].AddCard(deck.Deal());
				i++;
			}

		}

		/// <summary>
		/// Core Gameplay Loop
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>

		public bool IsGameOver(out GameResult? result)
		{
			var active = _hands.ActivePlayers.ToList();

			if (_roundNumber >= RoundLimit)
			{
				int max = active.Max(p => _hands[p].Count);
				var leaders = active.Where(p => _hands[p].Count == max).ToList();

				result = new GameResult
				{
					Type = leaders.Count == 1 ? RoundResultType.Winner : RoundResultType.Draw,
					Winner = leaders.Count == 1 ? leaders[0] : null,
					TotalRounds = _roundNumber
				};
				return true;
			}

			if (active.Count == 1)
			{
				result = new GameResult
				{
					Type = RoundResultType.Winner,
					Winner = active[0],
					TotalRounds = _roundNumber
				};
				return true;
			}

			result = null;
			return false;
		}

		public RoundResult PlayRound()
		{
			_roundNumber++;
			var pot = new List<Card>();
			var tiebreakers = new List<TiebreakerResult>();

			//removes players with no cards
			foreach (var name in _playerOrder.ToList())
			{
				if (_hands.All.CantainsKey(name) && !_hands[name].HasCards)
					_hands.RemovePlayer(name);
			}

			var activePlayers = _hands.ActivePlayers.ToList();
			var played = new PlayedCards();
			foreach (var name in activePlayers)
			{
				var card = _hands[name].DrawCard();
				played.Set(name, card);
				pot.Add(card);
			}

			var intialCards = played.All.ToDictionary(kv => kv.Key, kv => kv.Value);
			string? roundWinner = Resolve(played, pot, tiebreakers);
			if (roundWinner != null)
				_hands[roundWinner].AddCards(pot);
			var counts = _hands.All.ToDictionary(kv => kv.Key, kv => kv.Value.Count);
			return new RoundResult
			{
				Type = roundWinner != null ? RoundResultType.Winner : RoundResultType.Draw,
				Winner = roundWinner,
				RoundNumber = _roundNumber,
				CardsPlayed = initialCards,
				CardCounts = counts
			};
		}

		private string? Resolve(PlayedCards played, List<Card> pot, List<TiebreakerResult> tiebreakers)
		{
			var winners = played.Winners.ToList();
			if (winners.Count == 1)
				return winners[0];
			var tbResult = new TiebreakerResult { TiedPlayers = winners };
			var tbPlayed = new PlayedCards();
			var eliminated = new List<string>();'
			
			foreach (var name in winners)
			{
				if (!_hands[name].HasCards)
				{
					eliminated.Add(name);
					_hands.RemovePlayer(name);
					continue;
				}

				var card = _hands[name].DrawCard();
				tbPlayed.Set(name, card);
				pot.Add(card);
				tbResult.CardsPlayed[name] = card;
			}

			tbResult.Eliminated.AddRange(eliminated);
			var remaining = tbPlayed.All.Keys.ToList();
			if (remaining.Count == 0)
			{
				tiebreakers.Add(tbResult);
				return null;
			}

			if (remaining.Count == 1)
			{
				tbResult.Winner = remaining[0];
				tiebreakers.Add(tbResult);
				return remaining[0];
			}

			tiebreakers.Add(tbResult);
			return Resolve(tbPlayed, pot, tiebreakers);

		}
		public IReadOnlyDictionary<string, Hand> Hands => _hands.All;
		public int RoundNumber => _roundNumber;

	}
}

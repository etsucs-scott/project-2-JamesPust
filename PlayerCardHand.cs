using System;

public class PlayerCardHand
{
	/// <summary>
	/// single players hand
	/// </summary>
	public class Hand
	{
		private readonly Queue<Card> _cards = new();

		public int Count => _cards.Count;
		public bool HasCards => _cards.Count > 0;

		public void AddCard(Card card) => _cards.Enqueue(card);
		public Card DrawCard() => _cards.Dequeue();
		public void AddCards(IEnumerable<Card> cards)
		{
			foreach (var card in cards)
				_cards.Enqueue(card);
		}

	}
	/// <summary>
	/// Each player hand, keyed by name
	/// </summary>
	public class PlayerHands
	{
		private readonly Dictionary<string, Hand> _hands = new();
		public IReadOnlyDictionary<string, Hand> All => _hands;
		public Hand this[string playerName] => _hands[playerName];
		
		public void AddPlayer(string playerName) =>
			_hands[playerName] = new Hand();

		public IEnumerable<string> ActivePlayers =>
			_hands.Where(kv => kv.Value.HasCards).Select(kv  => kv.Key);

		public void RemovePlayer(string playerName) =>
			_hands.Remove(playerName);

	}

	public class PlayerCards
	{
		private readonly Dictionary<string, Card> _played = new();
        public IReadOnlyDictionary<string, Card> All => _played;
		
		public void Set(string playerName, Card card) => 
			_played[playerName] = card;

		public void Clear() =>
			_played.Clear();
		
		public Rank HighestRank =>
			_played.Values.Max(char => c.Rank);

		public IEnumerable<string> Winners =>
			_played.Where(kv => kv.Value.Rank == HighestRank).Select(kv => kv.Key);
		
    }
}

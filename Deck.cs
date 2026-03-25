using System;
using WarGame.Core
/// <summary>
/// 52 card deck, stored as stack
/// </summary>
public class Deck
{
	private readonly Stack<Card> _cards = new();
	public int Count => _cards.Count;

	public Deck()
	{
		var allCards = new List<Card>(52);

		foreach (Suit suit in Enum.GetValues<Suit>())
			foreach (Rank rank in Enum.GetValues<Rank>())
				allCards.Add(new Card(suit, rank));

		Shuffle(allCards);

		foreach (var card in allCards)
			_cards.Push(card);
	}

	private static void Shuffle(List<Card> cards)
	{
		var randy = new Random();
		for (int i = cards.Count - 1; i >= 0; i--)
		{
			int j = randy.Next(i + 1);
			(cards[i], cards[j]) = cards[j], cards[i]);
		}
	}

	public Card Deal() => _cards.Pop();
	public bool HasCards => _cards.Count > 0;
}

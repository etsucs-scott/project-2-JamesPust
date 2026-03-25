using System;

	public enum Suit
	{
		Clubs,
		Diamonds,
		Hearts,
		Spades
	}

	public enum Rank
	{
		Two = 2,
		Three = 3, 
		Four = 4,
		Five = 5,
		Six = 6,
		Seven = 7,
		Eight = 8,
		Nine = 9,
		Ten = 10,
		Jack = 11,
		Queen = 12,
		King = 13,
		Ace = 14,
	}
	/// <summary>
	/// Compares the cards by rank Ace high
	/// </summary>
	public class Card : IComparable<Card>
	{
		public Suit Suit { get; }
		public Rank Rank { get; }

		public Card(Suit suit, Rank rank)
		{
			Suit = suit;
			Rank = rank;
		}

		public int CompareTo(Card? other)
		{
			if (other is null) return 1;
			return Rank.CompareTo(other.Rank);
		}

		public override string ToString()
		{
			string rankStr = Rank switch
			{
				Rank.Jack => "J",
				Rank.Queen => "Q",
				Rank.King => "K",
				Rank.Ace => "A",
				_ => ((int)Rank).ToString()

			};
			return $"{rankStr}";
		}
	}


﻿namespace Backend
{
    public class Card
    {
        public string Rank { get; set; } = String.Empty;
        public string Suit { get; set; } = String.Empty;

        public override string ToString()
        {
            return $"{Rank}+{Suit}";
        }
    }
}

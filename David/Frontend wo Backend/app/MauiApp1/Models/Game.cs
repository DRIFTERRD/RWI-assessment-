using System;
using System.Collections.Generic;
using System.Linq;

namespace MauiApp1.Models
{
    public class BlackjackGame
    {
        private List<Card> deck;
        private List<Card> playerHand;
        private List<Card> dealerHand;
        private Random random;

        public List<Card> PlayerHand => playerHand;
        public List<Card> DealerHand => dealerHand;
        public int PlayerScore => CalculateScore(playerHand);
        public int DealerScore => CalculateScore(dealerHand);
        public bool IsPlayerBust => PlayerScore > 21;
        public bool IsDealerBust => DealerScore > 21;

        public bool IsDealerWon => !IsPlayerBust && (DealerScore > PlayerScore || IsPlayerBust || DealerScore == 21 && DealerHand.Count == 2);

        public BlackjackGame()
        {
            random = new Random();
            deck = CreateDeck();
            playerHand = new List<Card>();
            dealerHand = new List<Card>();
            ShuffleDeck();
            DealInitialCards();
        }

        private List<Card> CreateDeck()
        {
            var suits = new[] { "hearts", "diamonds", "clubs", "spades" };
            var ranks = new[] { "ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "jack", "queen", "king" };
            var deck = new List<Card>();

            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    deck.Add(new Card { Rank = rank, Suit = suit });
                }
            }

            return deck;
        }

        private void ShuffleDeck()
        {
            int n = deck.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1); // Get a random index between 0 and i (inclusive)
                // Swap deck[i] with deck[j]
                var temp = deck[i];
                deck[i] = deck[j];
                deck[j] = temp;
            }
        }
        private void DealInitialCards()
        {
            // Deal first card to player
            DealCardToPlayer();

            // Deal first card to dealer
            DealCardToDealer();

            // Deal second card to player
            DealCardToPlayer();


        }

        public Card DealCardToPlayer()
        {
            if (deck.Any())
            {
                var card = deck[0];
                playerHand.Add(card);
                deck.RemoveAt(0); // Remove card from the deck
                return card;
            }
            else
            {
                throw new InvalidOperationException("Deck is empty. Cannot deal card.");
            }
        }

        public Card DealCardToDealer()
        {
            if (deck.Any())
            {
                var card = deck[0];
                dealerHand.Add(card);
                deck.RemoveAt(0); // Remove card from the deck
                return card;
            }
            else
            {
                throw new InvalidOperationException("Deck is empty. Cannot deal card.");
            }
        }

        private int CalculateScore(List<Card> hand)
        {
            int score = 0;
            int aceCount = 0;

            foreach (var card in hand)
            {
                if (int.TryParse(card.Rank, out int value))
                {
                    score += value;
                }
                else if (card.Rank == "jack" || card.Rank == "queen" || card.Rank == "king")
                {
                    score += 10;
                }
                else if (card.Rank == "ace")
                {
                    aceCount++;
                    score += 11;
                }
            }

            // Adjust score if there are Aces and the total score is over 21
            while (aceCount > 0 && score > 21)
            {
                score -= 10; // Change Ace value from 11 to 1
                aceCount--;
            }

            return score;
        }
    }
}

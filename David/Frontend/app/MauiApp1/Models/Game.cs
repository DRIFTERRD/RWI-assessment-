using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace MauiApp1.Models
{
    public class BlackjackGame
    {
        private List<Card> playerHand;
        private List<Card> dealerHand;
        private TcpClient _client;
        public Connection player;
        public int? win;
        public int newValue;

        public List<Card> PlayerHand => playerHand;
        public List<Card> DealerHand => dealerHand;
        public int PlayerScore => CalculateScore(playerHand);
        public int DealerScore => CalculateScore(dealerHand);
        public bool IsPlayerBust => PlayerScore > 21;
        public bool IsDealerBust => DealerScore > 21;

        public bool IsDealerWon => !IsPlayerBust && (DealerScore > PlayerScore || IsPlayerBust || DealerScore == 21 && DealerHand.Count == 2);

        public BlackjackGame(int Bet)
        {
          
            playerHand = new List<Card>();
            dealerHand = new List<Card>();
            _client = new TcpClient("127.0.0.1", 1248);
            player = new Connection(_client.GetStream());
            win = null;
            newValue = 0;
            new Thread(() => threadHandeler()).Start();
            Task.Run(async () => await player.Send($"Bet {Bet}"));
        }

        public async void threadHandeler()
        {
            while (true)
            {
                string message = await player.Receive();
                string[] split = message.Split(" ");
                if (split[0] == "Player")
                {
                    playerHand.Clear();
                    string[] cards = split[1].Split(",");
                    foreach (string card in cards)
                    {
                        string[] items = card.Split("+");
                        Card item = new Card { Rank = items[0], Suit = items[1] };
                        playerHand.Add(item);
                    }
                }
                else if (split[0] == "Dealer")
                {
                    dealerHand.Clear();
                    string[] cards = split[1].Split(",");
                    foreach (string card in cards)
                    {
                        string[] items = card.Split("+");
                        Card item = new Card { Rank = items[0], Suit = items[1] };
                        dealerHand.Add(item);
                    }
                }
                else if (split[0] == "Finish")
                {
                    win = int.Parse(split[1]);
                    newValue = int.Parse(split[2]);
                    break;
                }
                else
                {
                    Console.WriteLine($"Error with message: {message}");
                }
            }
        }
    
        public Card? DealCardToPlayer()
        {
            if (playerHand.Count == 0)
                return null;
            return playerHand.Last();
        }

        public Card? DealCardToDealer()
        {
            if (dealerHand.Count == 0)
                return null;
            return dealerHand.Last();
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

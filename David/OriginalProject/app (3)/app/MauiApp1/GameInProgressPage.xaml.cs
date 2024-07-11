using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace MauiApp1
{
    public partial class GameInProgressPage : ContentPage
    {
        private CardImageManager cardImageManager;
        public int currentValue;
        public int bank;
        private BlackjackGame game;
        private Dictionary<string, FileImageSource> cardImageSources;
        private HashSet<string> displayedCards; // HashSet to track displayed cards


        public GameInProgressPage(ref int betAmount,ref  int bankValue)
        {
            InitializeComponent();

            this.currentValue = betAmount;
            this.bank = bankValue;

            game = new BlackjackGame();
            cardImageManager = new CardImageManager();


            displayedCards = new HashSet<string>(); // Initialize the HashSet

            UpdateBetValueLabel();
            UpdateBankValue();
            DisplayInitialCards();
            UpdatePlayerValueLabel();
            UpdateDealerValueLabel();
        }

        private async void DisplayInitialCards()
        {
            // Display the player's first card
            await Task.Delay(500);
            DisplayPlayerCards();


            // Display the dealer's first card
            await Task.Delay(600);
            DisplayDealerCards();

            // Display the player's second card
            await Task.Delay(700);
            DisplayPlayerCards();

            UpdatePlayerValueLabel();
            UpdateDealerValueLabel();
        }
        private void DisplayPlayerCards()
        {
            // Ensure CardStack is horizontal layout
            CardStack.Orientation = StackOrientation.Horizontal;

            // Display the player's cards
            foreach (var card in game.PlayerHand)
            {
                AddCardToStack(card, CardStack);
            }
        }

        private void DisplayDealerCards(bool showAllCards = false)
        {
           // DealerCardStack.Children.Clear(); // Clear existing cards

            if (showAllCards)
            {
                foreach (var card in game.DealerHand)
                {
                    AddCardToStack(card, DealerCardStack, smallerSize: true);
                }
            }
            else
            {
                // Only show the first card
                if (game.DealerHand.Count > 0)
                {
                    AddCardToStack(game.DealerHand[0], DealerCardStack, smallerSize: true);
                }
            }
        }

        private void AddCardToStack(Card card, StackLayout stack, bool smallerSize = false)
        {
            var cardKey = $"{card.Rank}_of_{card.Suit}";

            try
            {
                if (!displayedCards.Contains(cardKey))
                {
                    var cardImage = new Image
                    {
                        Source = cardImageManager.GetCardImageSource(cardKey),
                        WidthRequest = smallerSize ? 100 : 150,
                        HeightRequest = smallerSize ? 133 : 200,
                        Margin = new Thickness(5),
                    };

                    stack.Children.Add(cardImage);
                    displayedCards.Add(cardKey); // Add to displayed cards set
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding card {cardKey}: {ex.Message}");
            }
        }

        private void UpdatePlayerValueLabel()
        {
            PlayerValueLabel.Text = $"Player Value: {game.PlayerScore}";
        }

        private void UpdateDealerValueLabel()
        {
            DealerValueLabel.Text = $"Dealer Value: {game.DealerScore}";
        }

        private void CheckPlayerScore()
        {
            UpdatePlayerValueLabel();
            if (game.IsPlayerBust)
            {
                bank -= currentValue;
                currentValue = 0;
                DisplayAlert("You Lose!", "You went over 21.\nYou lose.\nGo Back and Select a new bet ammount", "OK");
                HitButton.IsEnabled = false;
                StandButton.IsEnabled = false;
            }
            else if (game.IsDealerWon)
            {
                bank -= currentValue;
                currentValue = 0;
                DisplayAlert("You Lose!", "Dealer wins.\nYou lose.\nGo Back and Select a new bet ammount", "OK");
                HitButton.IsEnabled = false;
                StandButton.IsEnabled = false;
            }
            UpdateBetValueLabel();
        }

        private async void HitButton_Clicked(object sender, EventArgs e)
        {
            if (!game.IsPlayerBust)
            {
                var card = game.DealCardToPlayer();
                AddCardToStack(card, CardStack);
                await Task.Delay(500);
                CheckPlayerScore();

                // Check again after updating the score
                if (game.IsPlayerBust)
                {
                    HitButton.IsEnabled = false;
                    StandButton.IsEnabled = false;
                }

                else if (game.IsPlayerBust)
                {
                    HitButton.IsEnabled = false;
                    StandButton.IsEnabled = false;
                }
                UpdatePlayerValueLabel();
            }
        }

        private async void StandButton_Clicked(object sender, EventArgs e)
        {
            // Disable Hit and Stand buttons
            HitButton.IsEnabled = false;
            StandButton.IsEnabled = false;

            
            DisplayDealerCards(true); // Show all dealer's cards

            // Deal cards to the dealer until score is 17 or higher
            while (game.DealerScore < 17)
            {
                var card = game.DealCardToDealer();
                AddCardToStack(card, DealerCardStack, smallerSize: true);
                await Task.Delay(500);
            }

            UpdateDealerValueLabel();

            // Compare player's and dealer's hands
            CompareHands();
        }

        private async void CompareHands()
        {
            int playerScore = game.PlayerScore;
            int dealerScore = game.DealerScore;

            if (game.IsDealerBust || playerScore > dealerScore)
            {
                bank += currentValue * 2; // Player wins or dealer busts
                UpdateBankValue();
                await DisplayAlert("You Win!", $"Player: {playerScore}\nDealer: {dealerScore}\nGo Back and Select a new bet ammount", "OK");
            }
            else if (playerScore < dealerScore)
            {
                // Dealer wins
                // No change needed in bank value, currentValue is already subtracted from bank
               await DisplayAlert("You Lose!", $"Player: {playerScore}\nDealer: {dealerScore}\nGo Back and Select a new bet ammount", "OK");
            }
            else if(playerScore == 21 &&  dealerScore == 21) {
                UpdateBankValue();
               await DisplayAlert("You Win!", $"Player: {playerScore}\nDealer: {dealerScore}\nGo Back and Select a new bet ammount", "OK");
            }
            else
            {
                // Tie scenario
                bank += currentValue; // Add the current bet amount to the bank
                UpdateBankValue();
               await DisplayAlert("It's a Tie!", $"Player: {playerScore} Dealer: {dealerScore}It's a tie! Go Back and Select a new bet ammount", "OK");
            }

            currentValue = 0; // Reset bet amount
            UpdateBankValue(); // Update bank value label
            UpdateBetValueLabel();              
        }



        private async void HitButton_(object sender, EventArgs e)
        {
            if (!game.IsPlayerBust)
            {
                var card = game.DealCardToPlayer();
                AddCardToStack(card, CardStack);
                await Task.Delay(500);
                CheckPlayerScore();

                // Check again after updating the score
                if (game.IsPlayerBust)
                {
                    HitButton.IsEnabled = false;
                    StandButton.IsEnabled = false;
                }
            }
            else if (game.IsPlayerBust)
            {
                HitButton.IsEnabled = false;
                StandButton.IsEnabled = false;
            }
        }

        void UpdateBetValueLabel()
        {
            BetValueLabel.Text = $"{currentValue}$";
        }

        void UpdateBankValue()
        {
            Bank_Winnings.Text = $"Bank: {bank}$";
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
           
            await Navigation.PopAsync(); 
        }

    }
}
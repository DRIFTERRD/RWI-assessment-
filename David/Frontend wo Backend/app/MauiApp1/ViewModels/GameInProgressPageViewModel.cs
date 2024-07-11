using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;

namespace MauiApp1.ViewModels
{
    public partial class GameInProgressPageViewModel : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            House.currentGame = new BlackjackGame();

            displayedCards = new HashSet<string>(); // Initialize the HashSet
            PlayerCards = new StackLayout();
            DealerCards = new StackLayout();

            HitButtonEnable = true;
            StandButtonEnable = true;
            BackButtonEnable = true;

            UpdateBetValueLabel();
            UpdateBankValue();
            DisplayInitialCards();
            UpdatePlayerValueLabel();
            UpdateDealerValueLabel();
        }

        private HashSet<string> displayedCards; // HashSet to track displayed cards

        [ObservableProperty]
        private StackLayout _playerCards;

        [ObservableProperty]
        private StackLayout _dealerCards;

        [ObservableProperty]
        private string _betValueText = String.Empty;

        [ObservableProperty]
        private string _bankWinningsText = String.Empty;

        [ObservableProperty]
        private string _playerValueText = String.Empty;

        [ObservableProperty]
        private string _dealerValueText = String.Empty;

        [ObservableProperty]
        private bool _hitButtonEnable = true;

        [ObservableProperty]
        private bool _standButtonEnable = true;

        [ObservableProperty]
        private bool _backButtonEnable = true;

        public GameInProgressPageViewModel()
        {
            House.currentGame = new BlackjackGame();

            displayedCards = new HashSet<string>(); // Initialize the HashSet
            PlayerCards = new StackLayout();
            DealerCards = new StackLayout();

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
            if (House.currentGame == null)
                return;

            // Display the player's cards
            foreach (var card in House.currentGame.PlayerHand)
            {
                AddCardToStack(card, PlayerCards);
            }
        }

        private void DisplayDealerCards(bool showAllCards = false)
        {
            if (House.currentGame == null)
               return;

            if (showAllCards)
            {
                foreach (var card in House.currentGame.DealerHand)
                {
                    AddCardToStack(card, DealerCards, smallerSize: true);
                }
            }
            else
            {
                // Only show the first card
                if (House.currentGame.DealerHand.Count > 0)
                {
                    AddCardToStack(House.currentGame.DealerHand[0], DealerCards, smallerSize: true);
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
                        Source = CardImageManager.GetCardImageSource(cardKey),
                        WidthRequest = smallerSize ? 100 : 150,
                        HeightRequest = smallerSize ? 133 : 200,
                        Margin = new Thickness(5),
                    };

                    stack.Add(cardImage);
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
            PlayerValueText = $"Player Value: {House.currentGame?.PlayerScore}";
        }

        private void UpdateDealerValueLabel()
        {
            DealerValueText = $"Dealer Value: {House.currentGame?.DealerScore}";
        }

        private async void CheckPlayerScore()
        {
            if (House.currentGame == null)
                return;

            UpdatePlayerValueLabel();

            if (House.currentGame.IsPlayerBust)
            {
                House.bankValue -= House.currentValue;
                if (House.bankValue < 0)
                {
                    House.bankValue = 0;
                }
                House.currentValue = 0;
                Application.Current?.MainPage?.DisplayAlert("You Lose!", "You went over 21.\nYou lose.\nGo Back and Select a new bet ammount", "OK");
                HitButtonEnable = false;
                StandButtonEnable = false;
            }
            else if (House.currentGame.IsDealerWon)
            {
                House.bankValue -= House.currentValue;
                if (House.bankValue < 0)
                {
                    House.bankValue = 0;
                }
                House.currentValue = 0;
                await Alert("You Lose!", "Dealer wins.\nYou lose.\nGo Back and Select a new bet ammount", "OK");
                HitButtonEnable = false;
                StandButtonEnable = false;
            }

            UpdateBetValueLabel();
        }

        [RelayCommand]
        private async Task HitButton_Clicked()
        {
            if (House.currentGame == null)
                return;

            if (!House.currentGame.IsPlayerBust)
            {
                var card = House.currentGame.DealCardToPlayer();
                AddCardToStack(card, PlayerCards);
                await Task.Delay(500);
                CheckPlayerScore();


                UpdatePlayerValueLabel();
            }
            else
            {
                HitButtonEnable = false;
                StandButtonEnable = false;
                UpdatePlayerValueLabel();

            }
        }

        [RelayCommand]
        private async Task StandButton_Clicked()
        {
            if (House.currentGame == null)
                return;

            // Disable Hit and Stand buttons
            HitButtonEnable = false;
            StandButtonEnable = false;


            DisplayDealerCards(true); // Show all dealer's cards

            // Deal cards to the dealer until score is 17 or higher
            while (House.currentGame.DealerScore < 17)
            {
                var card = House.currentGame.DealCardToDealer();
                AddCardToStack(card, DealerCards, smallerSize: true);
                await Task.Delay(500);
            }

            UpdateDealerValueLabel();

            // Compare player's and dealer's hands
            CompareHands();
        }

        private async void CompareHands()
        {
            if (House.currentGame == null)
                return;

            int playerScore = House.currentGame.PlayerScore;
            int dealerScore = House.currentGame.DealerScore;

            if (House.currentGame.IsDealerBust || playerScore > dealerScore)
            {
                House.bankValue += House.currentValue * 2; // Player wins or dealer busts
                UpdateBankValue();
                await Alert("You Win!", $"Player: {playerScore}\nDealer: {dealerScore}\nGo Back and Select a new bet ammount", "OK");
            }
            else if (playerScore < dealerScore)
            {
                // Dealer wins
                // No change needed in bank value, currentValue is already subtracted from bank
                await Alert("You Lose!", $"Player: {playerScore}\nDealer: {dealerScore}\nGo Back and Select a new bet ammount", "OK");
            }
            else if (playerScore == 21 && dealerScore == 21)
            {
                UpdateBankValue();
                await Alert("You Win!", $"Player: {playerScore}\nDealer: {dealerScore}\nGo Back and Select a new bet ammount", "OK");
            }
            else
            {
                // Tie scenario
                House.bankValue += House.currentValue; // Add the current bet amount to the bank
                UpdateBankValue();
                await Alert("It's a Tie!", $"Player: {playerScore} Dealer: {dealerScore}It's a tie! Go Back and Select a new bet ammount", "OK");
            }

            House.currentValue = 0; // Reset bet amount
            UpdateBankValue(); // Update bank value label
            UpdateBetValueLabel();
        }

        private async void HitButton_()
        {
            if (House.currentGame == null)
                return;

            if (!House.currentGame.IsPlayerBust)
            {
                var card = House.currentGame.DealCardToPlayer();
                AddCardToStack(card, PlayerCards);
                await Task.Delay(500);
                CheckPlayerScore();
            }
            else
            {
                HitButtonEnable = false;
                StandButtonEnable = false;
            }

        }
        void UpdateBetValueLabel()
        {
            BetValueText = $"{House.currentValue}$";
        }

        void UpdateBankValue()
        {
            BankWinningsText = $"Bank: {House.bankValue}$";
        }

        async Task Alert(string title, string description, string button)
        {
            await Application.Current?.MainPage?.DisplayAlert(title, description, button);
        }

        [RelayCommand]
        private async Task BackButton_Clicked()
        {
            await Shell.Current.GoToAsync($"..", new ShellNavigationQueryParameters{ { "Update", true } });
        }
    }
}

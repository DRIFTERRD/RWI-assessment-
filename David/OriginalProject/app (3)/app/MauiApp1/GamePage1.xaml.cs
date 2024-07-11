using Microsoft.Maui.Controls;
using System;

namespace MauiApp1
{
    public partial class GamePage : ContentPage
    {

        private int currentValue = 0; // Track current value
        public int bankValue = 5000;
        private Stack<int> operationHistory = new Stack<int>(); // Track operations (positive for addition, negative for removal)

       public GamePage(int bankValue, int currentValue)
        {
            InitializeComponent();
            InitializeGame();
            
        }


        void InitializeGame()
        {
             //Show Deal button
            DealButton.IsVisible = true;

            // Show betting chips and +/- buttons
            ChipsLayout.IsVisible = true;
            PlusMinusLayout.IsVisible = true;

            // Reset current value and operation history
            currentValue = 0;
           
            operationHistory.Clear();

           UpdateSelectedValueLabel();
            UpdateBank_Winnings();

        }

        async void DealButton_Clicked(object sender, EventArgs e)
        {
             //Navigate to the new page (GameInProgressPage)
            await Navigation.PushAsync(new GameInProgressPage(ref currentValue,ref bankValue));
            

            // Optionally, you can reset the game state or perform other actions after navigating
            InitializeGame();
        }
        async void Reset_Clicked(object sender, EventArgs e)
        {
            //Navigate to the new page (GameInProgressPage)
            await Navigation.PushAsync(new GamePage( currentValue,  bankValue));


            // Optionally, you can reset the game state or perform other actions after navigating
            InitializeGame();
        }
        void PlusButton_Clicked(object sender, EventArgs e)

        {
            int step = GetStepValue(); // Get current step value (either from a chip or default)
            if (bankValue - step >= 0)
            {
              
                currentValue += step;
                bankValue -= step;
                operationHistory.Push(step); // Record addition operation

                UpdateSelectedValueLabel();
                UpdateBank_Winnings();
            }
        }

        void MinusButton_Clicked(object sender, EventArgs e)
        {
            if (operationHistory.Count > 0)
            {
                int lastOperation = operationHistory.Pop();
                currentValue -= lastOperation; // Reverse last operation (addition -> subtraction, subtraction -> addition)
                bankValue += lastOperation;
                UpdateSelectedValueLabel();
                UpdateBank_Winnings();
            }
        }

        void BettingChip_Clicked(object sender, EventArgs e)
        {
            ImageButton button = sender as ImageButton;
            if (button != null)
            {
                int chipValue = GetChipValue(button);

                if (bankValue - chipValue >= 0)
                {
                    currentValue += chipValue;
                    bankValue -= chipValue;
                    operationHistory.Push(chipValue); // Record chip addition

                    UpdateSelectedValueLabel();
                    UpdateBank_Winnings();
                }
            }
        }

        int GetChipValue(ImageButton button)
        {
           
            
            string source = button.Source.ToString();

            if (source.Contains("chip1.2.png"))
            {
                return 5;
            }
            else if (source.Contains("chip2.2.png"))
            {
                return 10;
            }
            else if (source.Contains("chip3.2.png"))
            {
                return 50;
            }
            else if (source.Contains("chip4.2.png"))
            {
                return 100;
            }
            else if (source.Contains("chip5.2.png"))
            {
                return 250;
          }
           else if (source.Contains("chip6.2.png"))
            {
                return 500;
            }
            else
            {
                return 0; // Default value if no match found
            }
        }
        

        int GetStepValue()
        {
            if (operationHistory.Count > 0)
            {
                return operationHistory.Peek(); // Use the last recorded operation as step value
           }
            else
            {
               return 5; // Default step value if no operations recorded yet
            }
        }

        void UpdateSelectedValueLabel()
        {
            SelectedValueLabel.Text = $"{currentValue}$";
        }
        void UpdateBank_Winnings()
        {
            Bank_Winnings.Text = $"{bankValue}$";
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateBank_Winnings(); // Ensure bankValue is updated when returning to this page
        }
    }
}

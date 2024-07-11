using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;
using MauiApp1.Views;

namespace MauiApp1.ViewModels
{
    public partial class GamePageViewModel : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            UpdateSelectedValueLabel();
            UpdateBank_Winnings();
            DealButtonEnable = true;
            ResetButtonEnable = true;
        }

        [ObservableProperty]
        private string _selectedText = "0$";

        [ObservableProperty]
        private string _bankText = "Bank: 5000$";

        [ObservableProperty]
        private bool _dealButtonEnable = true;

        [ObservableProperty]
        private bool _resetButtonEnable = true;

        [RelayCommand]
        public async Task DealButton_Clicked()
        {
            await Shell.Current.GoToAsync($"{nameof(GameInProgressPage)}", new ShellNavigationQueryParameters { { "Update", true } });
        }

        [RelayCommand]
        void Reset_Clicked()
        {
            House.currentValue = 0;
            House.bankValue = 5000;
            House.operationHistory.Clear(); 


            UpdateSelectedValueLabel();
            UpdateBank_Winnings();
        }

        [RelayCommand]
        void PlusButton_Clicked()
        {
            House.addStep(House.GetStepValue());
            UpdateSelectedValueLabel();
            UpdateBank_Winnings();
        }

        [RelayCommand]
        void MinusButton_Clicked()
        {
            House.substractStep();
            UpdateSelectedValueLabel();
            UpdateBank_Winnings();
        }

        [RelayCommand]
        void BettingChip1()

        {
            House.addStep(5);
            UpdateSelectedValueLabel();
            UpdateBank_Winnings();
        }

        [RelayCommand]
        void BettingChip2()
        {
            House.addStep(10);
            UpdateSelectedValueLabel();
            UpdateBank_Winnings();
        }
        [RelayCommand]
        void BettingChip3()
        {
            House.addStep(50);
            UpdateSelectedValueLabel();
            UpdateBank_Winnings();
        }
        [RelayCommand]
        void BettingChip4()
        {
            House.addStep(100);
            UpdateSelectedValueLabel();
            UpdateBank_Winnings();
        }

        [RelayCommand]
        void BettingChip5()
        {
            House.addStep(250);
            UpdateSelectedValueLabel();
            UpdateBank_Winnings();
        }

        [RelayCommand]
        void BettingChip6()
        {
            House.addStep(500);
            UpdateSelectedValueLabel();
            UpdateBank_Winnings();
        }


        void UpdateSelectedValueLabel()
        {
            SelectedText = $"{House.currentValue}$";
        }
        void UpdateBank_Winnings()
        {
            BankText = $"{House.bankValue}$";
        }
    }
}

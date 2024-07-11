using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Views;

namespace MauiApp1.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        [RelayCommand]
        public async Task Play_Button()
        {
            await Shell.Current.GoToAsync($"{nameof(GamePage)}");
        }
    }
}

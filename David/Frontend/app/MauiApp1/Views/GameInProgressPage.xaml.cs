using MauiApp1.ViewModels;

namespace MauiApp1.Views
{
    public partial class GameInProgressPage : ContentPage
    {
        public GameInProgressPage(GameInProgressPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
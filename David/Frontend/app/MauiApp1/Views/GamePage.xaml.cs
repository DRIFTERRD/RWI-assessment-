using MauiApp1.ViewModels;

namespace MauiApp1.Views
{
    public partial class GamePage : ContentPage
    {
        public GamePage(GamePageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}

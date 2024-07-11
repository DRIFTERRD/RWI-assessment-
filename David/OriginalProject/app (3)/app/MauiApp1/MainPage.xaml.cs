using Microsoft.Maui.Controls;
using System;

namespace MauiApp1
{
    public partial class MainPage : ContentPage

    {
        private int currentValue = 0; // Track current value
        private int bankValue = 5000;
        public MainPage()
        {
            InitializeComponent();

        }

       private async void PlayButton_Clicked(object sender, EventArgs e)
       {
           await Navigation.PushAsync(new GamePage(bankValue, currentValue));
        }
    }
}

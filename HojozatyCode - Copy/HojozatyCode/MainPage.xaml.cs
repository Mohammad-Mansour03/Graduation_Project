using Microsoft.Maui.Controls;

namespace HojozatyCode
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent(); // This wires up your XAML UI
        }

        private async void OnAddVenueClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Pages.SpaceTypeSelectionPage));
        }
    }
}

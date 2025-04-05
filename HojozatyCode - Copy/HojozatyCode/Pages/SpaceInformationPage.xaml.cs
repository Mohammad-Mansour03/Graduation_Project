using HojozatyCode.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace HojozatyCode.Pages
{
    public partial class SpaceInformationPage : ContentPage
    {
        public SpaceInformationPage(AddSpaceViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            var viewModel = BindingContext as AddSpaceViewModel;
            if (viewModel != null)
            {
                // Update ViewModel properties
                viewModel.Latitude = e.Location.Latitude;
                viewModel.Longitude = e.Location.Longitude;
                viewModel.SelectedLocation = $"Lat: {e.Location.Latitude}, Lng: {e.Location.Longitude}";

                // Clear existing pins
                VenueMap.Pins.Clear();

                // Add a new pin at the selected location
                var pin = new Pin
                {
                    Label = "Selected Location",
                    Address = "Drag to adjust location",
                    Location = e.Location,
                    Type = PinType.Place
                };

                VenueMap.Pins.Add(pin);
            }
        }
    }
}
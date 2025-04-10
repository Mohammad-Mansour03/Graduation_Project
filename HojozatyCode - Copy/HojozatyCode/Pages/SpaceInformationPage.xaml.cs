using HojozatyCode.Messages;
using HojozatyCode.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using CommunityToolkit.Mvvm.Messaging;

namespace HojozatyCode.Pages
{
    public partial class SpaceInformationPage : ContentPage
    {
        public SpaceInformationPage(AddSpaceViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            
            // Set initial map position (these coordinates are for Jordan)
            var initialLocation = new Location(31.9539, 35.9106);
            VenueMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                initialLocation,
                Distance.FromKilometers(400)));
                
            // Subscribe to the search location message using WeakReferenceMessenger
            WeakReferenceMessenger.Default.Register<LocationFoundMessage>(this, (r, message) =>
            {
                // Center the map on the found location
                Dispatcher.Dispatch(() => {
                    VenueMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                        message.Location,
                        Distance.FromKilometers(1)));
                        
                    // Clear existing pins
                    VenueMap.Pins.Clear();
                    
                    // Add a new pin at the searched location
                    var pin = new Pin
                    {
                        Label = "Selected Location",
                        Address = "Search result",
                        Location = message.Location,
                        Type = PinType.SearchResult
                    };
                    
                    VenueMap.Pins.Add(pin);
                });
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Unsubscribe when the page disappears
            WeakReferenceMessenger.Default.Unregister<LocationFoundMessage>(this);
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
                    Address = "Tap and hold to drag",
                    Location = e.Location,
                    Type = PinType.Place
                };

                VenueMap.Pins.Add(pin);
            }
        }
    }
}
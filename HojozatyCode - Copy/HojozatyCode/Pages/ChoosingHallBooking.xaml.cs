using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class ChoosingHallBooking : ContentPage
{
    public ChoosingHallBooking(BookingViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void FavoriteButton_Clicked(object sender, EventArgs e)
    {
        // Simple scaling animation for the "pop" effect
        await FavoriteButton.ScaleTo(1.3, 100, Easing.CubicIn); // Scale up
        await FavoriteButton.ScaleTo(1.0, 100, Easing.CubicOut); // Scale down

        // Optionally, add a fade effect (for example, a short flash on press)
        await FavoriteButton.FadeTo(0.7, 100);  // Fade to 70% opacity
        await FavoriteButton.FadeTo(1.0, 100);  // Fade back to 100% opacity

        // Toggle the visual state for "favorited" or "unfavorited"
        bool isFavorite = (BindingContext as BookingViewModel)?.IsFavorite ?? false; // Check the binding context property
        VisualStateManager.GoToState(FavoriteButton, isFavorite ? "Favorited" : "Unfavorited");
    }

   

}

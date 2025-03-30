using HojozatyCode.Models;
using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class AdminPanel: ContentPage
{
    public AdminPanel()
    {
        InitializeComponent();
        BindingContext = new AdminApprovalViewModel();
    }
    
    private async void OnApprovalButtonClicked(object sender, EventArgs e)
    {
        // Navigate to all venues list
        await Shell.Current.GoToAsync(nameof(AdminApprovalPage));
    }
    
    private async void OnReviewButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Venue venue)
        {
            // Navigate to specific venue review
            await Shell.Current.GoToAsync($"{nameof(AdminApprovalPage)}?venueId={venue.VenueId}");
        }
    }
}
using AndroidX.Lifecycle;
using HojozatyCode.Models;
using HojozatyCode.ViewModels;
using Java.Lang;

namespace HojozatyCode.Pages;

public partial class AdminPanel: ContentPage
{

    AdminApprovalViewModel _viewModel;
    public AdminPanel()
    {
        InitializeComponent();
        BindingContext = new AdminApprovalViewModel();
        _viewModel = (AdminApprovalViewModel)BindingContext;
    }

	protected override  void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.LoadPendingVenues();
	}

    //Event Handler to navigate me to the Admin Approval Page
	private async void OnApprovalButtonClicked(object sender, EventArgs e)
    {
        // Navigate to all venues list
        await Shell.Current.GoToAsync(nameof(AdminApprovalPage));
    }
    
    //Event Handler to navigate me to the Admin Approval Page with passing the venue id 
    private async void OnReviewButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Venue venue)
        {
            // Navigate to specific venue review
            await Shell.Current.GoToAsync($"{nameof(AdminApprovalPage)}?venueId={venue.VenueId}");
        }
    }
}
using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class EditBooking : ContentPage
{
	public EditBooking()
	{
		InitializeComponent();
		BindingContext = new EditBookingViewModel();
	}
}
using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class ReviewBooking : ContentPage
{
	public ReviewBooking(BookingViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
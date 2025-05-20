using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class MyBookings : ContentPage
{
	public MyBookings()
	{
		InitializeComponent();
		BindingContext = new MyBookingViewModel();
	}
}
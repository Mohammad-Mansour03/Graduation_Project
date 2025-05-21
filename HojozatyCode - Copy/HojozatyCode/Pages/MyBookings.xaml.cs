using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class MyBookings : ContentPage
{
	MyBookingViewModel viewModel;
	public MyBookings()
	{
		InitializeComponent();
		BindingContext = viewModel = new MyBookingViewModel();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		await viewModel.LoadBooking();
	}
}
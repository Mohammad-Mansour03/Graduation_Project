using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class ChoosingHallBooking : ContentPage
{
	public ChoosingHallBooking(BookingViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
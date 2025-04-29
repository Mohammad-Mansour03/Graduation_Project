using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class BookingCalendarPage : ContentPage
{
	public BookingCalendarPage(BookingViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
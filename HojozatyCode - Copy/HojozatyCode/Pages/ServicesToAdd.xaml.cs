using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class ServicesToAdd : ContentPage
{
	public ServicesToAdd(BookingViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
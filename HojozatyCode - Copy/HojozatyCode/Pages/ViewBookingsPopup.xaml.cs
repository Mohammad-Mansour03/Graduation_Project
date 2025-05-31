using HojozatyCode.Models;
using HojozatyCode.ViewModels;
using System.Collections.ObjectModel;

namespace HojozatyCode.Pages;

public partial class ViewBookingsPopup : ContentPage
{
	public ViewBookingsPopup(ObservableCollection<Booking> bookings)
	{
		InitializeComponent();
		BindingContext = new ViewBookingsPopupViewModel(bookings);
	}

	private async void ClosePopup(object sender, EventArgs e)
	{
		await Shell.Current.Navigation.PopModalAsync();
	}
}
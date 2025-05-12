using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class ReviewBooking : ContentPage
{
public ReviewBooking(BookingViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

private async void OnPayWithCardClicked(object sender, EventArgs e)
{
    var vm = BindingContext as BookingViewModel;
    string venueName = Uri.EscapeDataString(vm?.SelectedVenue?.VenueName ?? "Venue");
    string price = vm?.TotalPrice.ToString("F2") ?? "0.00";
    string baseStripeUrl = "https://buy.stripe.com/test_5kQ14n7nwcWDfU2g6x2wU00";
    string stripeUrl = $"{baseStripeUrl}?venue={venueName}&price={price}";
    await Navigation.PushAsync(new PaymentWebViewPage(stripeUrl));
}

}
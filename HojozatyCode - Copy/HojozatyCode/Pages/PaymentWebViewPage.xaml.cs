using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace HojozatyCode.Pages
{
    public partial class PaymentWebViewPage : ContentPage
    {
        public PaymentWebViewPage(string paymentUrl)
        {
            InitializeComponent();
            PaymentWebView.Source = paymentUrl;
            // Add overlay button
            var overlayButton = new Button
            {
                Text = "Continue",
                BackgroundColor = Color.FromArgb("#4F378A"),
                TextColor = Colors.White,
                CornerRadius = 25,
                FontSize = 18,
                WidthRequest = 180,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                Margin = new Thickness(0, 0, 0, 40),
                Opacity = 0.9
            };
            overlayButton.Clicked += async (s, e) =>
            {
                // Remove all pages until root, then push BookingSuccessPage
                Application.Current.MainPage = new NavigationPage(new BookingSuccessPage());
            };
            // Add overlay to the page
            var absoluteLayout = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutFlags(PaymentWebView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(PaymentWebView, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(overlayButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(overlayButton, new Rect(0.5, 0.95, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absoluteLayout.Children.Add(PaymentWebView);
            absoluteLayout.Children.Add(overlayButton);
            Content = absoluteLayout;
        }

        private async void PaymentWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            // Go directly to BookingSuccessPage if needed (for auto-detect, but overlay is primary)
            if (e.Url.Contains("success") || e.Url.Contains("payment_intent"))
            {
                await Navigation.PopAsync();
                await Navigation.PushAsync(new BookingSuccessPage());
            }
        }
    }
}

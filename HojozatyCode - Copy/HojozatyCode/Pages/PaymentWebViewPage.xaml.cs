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

            // Set the WebView source
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
                // Navigate to BookingSuccessPage, keep navigation stack intact
                await Navigation.PushAsync(new BookingSuccessPage());
            };

            // Compose the layout with AbsoluteLayout
            var absoluteLayout = new AbsoluteLayout();

            AbsoluteLayout.SetLayoutFlags(PaymentWebView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(PaymentWebView, new Rect(0, 0, 1, 1));

            AbsoluteLayout.SetLayoutFlags(overlayButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(overlayButton, new Rect(0.5, 0.95, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            absoluteLayout.Children.Add(PaymentWebView);
            absoluteLayout.Children.Add(overlayButton);

            Content = absoluteLayout;

            // Subscribe to navigation event
            PaymentWebView.Navigated += PaymentWebView_Navigated;
        }

        protected override bool OnBackButtonPressed()
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                Navigation.PopAsync();
                return true; // Indicate back press handled
            }
            return base.OnBackButtonPressed();
        }

        private async void PaymentWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.Url.Contains("success") || e.Url.Contains("payment_intent"))
            {
                // Navigate to BookingSuccessPage on successful payment
                await Navigation.PushAsync(new BookingSuccessPage());

                // Optionally remove this page from the stack if you don't want user to go back here
                Navigation.RemovePage(this);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Unsubscribe event to prevent potential crashes
            PaymentWebView.Navigated -= PaymentWebView_Navigated;
        }
    }
}

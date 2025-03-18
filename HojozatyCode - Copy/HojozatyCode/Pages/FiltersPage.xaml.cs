using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages
{
    public partial class FiltersPage : ContentPage
    {
        public FiltersPage()
        {
            InitializeComponent();
            BindingContext = new LogInViewModel();
        }

        // Event handler for when the slider value changes
        private void PriceSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            // Update the label to display the slider value, converting it to an integer and appending " JD"
            PriceValueLabel.Text = $"{(int)e.NewValue} JD";
        }
    }
}

using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages
{
    public partial class VenueListings : ContentPage
    {
        public VenueListings()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
        }
    }
}
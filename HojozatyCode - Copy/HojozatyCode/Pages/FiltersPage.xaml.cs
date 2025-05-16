using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages
{
    public partial class FiltersPage : ContentPage
    {
        public FiltersPage()
        {
            InitializeComponent();
            BindingContext = new FilterVenueViewModel();
        }

    }
}

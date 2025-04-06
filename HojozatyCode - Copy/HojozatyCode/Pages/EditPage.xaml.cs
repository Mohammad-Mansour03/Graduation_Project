using HojozatyCode.ViewModels;
using Microsoft.Maui.Controls;

namespace HojozatyCode.Pages
{
    public partial class EditVenuePage : ContentPage
    {
        public EditVenuePage(object venueToEdit = null)
        {
            InitializeComponent();
            BindingContext = new EditVenueViewModel(venueToEdit);
        }
    }
}

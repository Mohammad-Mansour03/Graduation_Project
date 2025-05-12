using Microsoft.Maui.Controls;
using HojozatyCode.ViewModels;
namespace HojozatyCode.Pages
{
    public partial class BookingSuccessPage : ContentPage
    {
        // public BookingSuccessPage(BookingViewModel vm)
        // {
        //     InitializeComponent();
        //     BindingContext = vm;
        // }

        // Add a parameterless constructor
        public BookingSuccessPage()
        {
            InitializeComponent();
            BindingContext = new BookingViewModel();
        }
    }
}

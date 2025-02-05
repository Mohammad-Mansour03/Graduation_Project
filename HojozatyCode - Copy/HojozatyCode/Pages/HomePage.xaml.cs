using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
        }
    }
}
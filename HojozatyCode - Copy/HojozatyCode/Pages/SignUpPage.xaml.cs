using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
            BindingContext = new SignUpViewModel();
        }
    }
}
using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages
{
    public partial class LogInPage : ContentPage
    {
        public LogInPage()
        {
            InitializeComponent();
            BindingContext = new LogInViewModel();
        }
    }
}
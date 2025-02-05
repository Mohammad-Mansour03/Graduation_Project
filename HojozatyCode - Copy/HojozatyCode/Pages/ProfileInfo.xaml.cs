using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages
{
    public partial class ProfileInfo : ContentPage
    {
        public ProfileInfo()
        {
            InitializeComponent();
            BindingContext = new ProfileInfoViewModel();
        }
    }
}
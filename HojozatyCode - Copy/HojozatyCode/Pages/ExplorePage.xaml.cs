using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class ExplorePage : ContentPage
{
    public ExplorePage()
    {
        InitializeComponent();
        BindingContext = new HomeViewModel();
    }
}
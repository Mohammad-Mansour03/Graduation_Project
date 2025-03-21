using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class AdminPanel: ContentPage
{
    public AdminPanel()
    {
        InitializeComponent();
        BindingContext = new AccountPageViewModel();
    }
}
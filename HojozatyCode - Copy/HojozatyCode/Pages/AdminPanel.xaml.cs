using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class AdminPanel : ContentPage
{
    public AdminPanel(OwnerVenuesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
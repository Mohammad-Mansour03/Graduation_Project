using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class SuccessPage : ContentPage
{
    public SuccessPage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
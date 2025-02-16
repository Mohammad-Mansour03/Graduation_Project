using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class SuccessPage : ContentPage
{
    public SuccessPage(AddSpaceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class SpacePolicies : ContentPage
{
    public SpacePolicies(AddSpaceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
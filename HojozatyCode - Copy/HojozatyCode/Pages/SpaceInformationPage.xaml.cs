using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class SpaceInformationPage : ContentPage
{
    public SpaceInformationPage(AddSpaceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class SpaceTypeSelectionPage : ContentPage
{
    public SpaceTypeSelectionPage(AddSpaceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class SpaceTypeSelectionPage : ContentPage
{
    private readonly AddSpaceViewModel _viewModel;

    public SpaceTypeSelectionPage(AddSpaceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.ResetViewState();
    }
}
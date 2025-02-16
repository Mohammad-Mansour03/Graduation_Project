using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class ServicesPage : ContentPage
{
    public ServicesPage(AddSpaceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
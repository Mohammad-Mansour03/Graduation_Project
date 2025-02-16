using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class ReviewPage : ContentPage
{
    public ReviewPage(AddSpaceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
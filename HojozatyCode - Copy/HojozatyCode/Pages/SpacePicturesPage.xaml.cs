using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class SpacePicturesPage : ContentPage
{
    public SpacePicturesPage(AddSpaceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
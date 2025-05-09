using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class FavouritePage : ContentPage
{
    public FavouritePage(FavoriteViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
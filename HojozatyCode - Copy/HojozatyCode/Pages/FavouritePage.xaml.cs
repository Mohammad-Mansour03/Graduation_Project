using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class FavouritePage : ContentPage
{
	FavoriteViewModel _viewModel;
    public FavouritePage(FavoriteViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

	private async Task LoadFavouriteSafe() 
	{
		await _viewModel.LoadFavoritesAsync();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		_ = LoadFavouriteSafe();
	}
}
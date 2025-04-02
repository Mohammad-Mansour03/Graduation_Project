using HojozatyCode.ViewModels;
namespace HojozatyCode.Pages;

public partial class MySpace : ContentPage
{
	private readonly MySpaceViewModel _viewModel;

	public MySpace()
	{
		InitializeComponent();
		_viewModel = (MySpaceViewModel)BindingContext;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadVenuesAsync();
	}


}
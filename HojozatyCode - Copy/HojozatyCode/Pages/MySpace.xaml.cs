using HojozatyCode.ViewModels;
namespace HojozatyCode.Pages;

public partial class MySpace : ContentPage
{
	private readonly MySpaceViewModel _viewModel;

	public MySpace()
	{
		InitializeComponent();
		_viewModel = new MySpaceViewModel();
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadVenuesAsync();
	}


}
using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages
{
    public partial class HomePage : ContentPage
    {
		private readonly HomeViewModel _viewModel;
		public HomePage()
        {
            InitializeComponent();
			_viewModel = new HomeViewModel();
			BindingContext = _viewModel;
		}


		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await _viewModel.LoadVenues();
		}
	}
}
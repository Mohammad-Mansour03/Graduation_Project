using HojozatyCode.ViewModels;
namespace HojozatyCode.Pages;

public partial class EditPage: ContentPage
{
    private readonly MySpaceViewModel _viewModel;

    public EditPage()
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
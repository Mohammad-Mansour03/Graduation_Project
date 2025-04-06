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

<<<<<<< HEAD
    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();

    //    // Optional: load any needed initial data
    //    await _viewModel.LoadInitialDataAsync();
    //}
}
=======
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadVenuesAsync();
    }


}
>>>>>>> 4ea658f25d12c24cbdd2a7f9b3fbec2f163a8c8e

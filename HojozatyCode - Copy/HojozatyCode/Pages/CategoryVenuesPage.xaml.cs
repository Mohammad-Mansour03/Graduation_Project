using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class CategoryVenuesPage : ContentPage
{
    public CategoryVenuesPage(CategoryVenuesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
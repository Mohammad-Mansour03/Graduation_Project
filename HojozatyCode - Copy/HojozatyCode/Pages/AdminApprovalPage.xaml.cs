using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class AdminApprovalPage : ContentPage
{
    public AdminApprovalPage(AdminApprovalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
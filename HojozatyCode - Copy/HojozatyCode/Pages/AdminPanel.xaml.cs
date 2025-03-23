using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class AdminPanel: ContentPage
{
    public AdminPanel()
    {
        InitializeComponent();
        BindingContext = new AdminApprovalViewModel();
    }
    
    private async void OnApprovalButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AdminApprovalPage));
    }
}
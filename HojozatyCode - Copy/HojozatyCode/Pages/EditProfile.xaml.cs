using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

public partial class EditProfile : ContentPage
{
	public EditProfile()
	{
		InitializeComponent();
		BindingContext = new EditProfileViewModel();
	}
}
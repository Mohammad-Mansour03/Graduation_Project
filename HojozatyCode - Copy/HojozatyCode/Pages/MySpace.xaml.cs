using HojozatyCode.ViewModels;
namespace HojozatyCode.Pages;

public partial class MySpace : ContentPage
{
	public MySpace()
	{
		InitializeComponent();
		BindingContext = new MySpaceViewModel();
	}
}
using HojozatyCode.Models;
using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages;

[QueryProperty(nameof(FilterData), "FilterData")]
public partial class CategoryVenuesPage : ContentPage
{
	private readonly CategoryVenuesViewModel viewModel;
	public CategoryVenuesPage(CategoryVenuesViewModel vm)
    {
        InitializeComponent();
        BindingContext = viewModel = vm;
    }
	private VenueFilter filterData;
	public VenueFilter FilterData
	{
		get => filterData;
		set
		{
			filterData = value;
			if (filterData != null)
			{
				viewModel.ApplyFilterCommand.Execute(filterData);
			}
		}
	}
}
using HojozatyCode.ViewModels;

namespace HojozatyCode.Pages
{
    public partial class FiltersPage : ContentPage
    {
        public FiltersPage()
        {
            InitializeComponent();
            BindingContext = new FilterVenueViewModel();
        }
		private void OnCategoryCheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (BindingContext is FilterVenueViewModel viewModel && sender is CheckBox checkBox && checkBox.BindingContext is string category)
			{
				if (e.Value)
				{
					if (!viewModel.SelectedCategories.Contains(category))
						viewModel.SelectedCategories.Add(category);
				}
				else
				{
					if (viewModel.SelectedCategories.Contains(category))
						viewModel.SelectedCategories.Remove(category);
				}
			}
		}

	}
}

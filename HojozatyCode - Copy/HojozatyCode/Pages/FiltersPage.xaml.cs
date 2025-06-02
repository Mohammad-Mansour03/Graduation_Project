using HojozatyCode.ViewModels;
using Microsoft.Maui.Controls;

namespace HojozatyCode.Pages
{
    public partial class FiltersPage : ContentPage
    {
        public FiltersPage()
        {
            InitializeComponent();
            BindingContext = new FilterVenueViewModel(); // Use your real ViewModel
        }

        private void OnCategoryCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (BindingContext is FilterVenueViewModel vm &&
                sender is CheckBox checkBox &&
                checkBox.BindingContext is string category)
            {
                if (e.Value)
                {
                    if (!vm.SelectedCategories.Contains(category))
                        vm.SelectedCategories.Add(category);
                }
                else
                {
                    if (vm.SelectedCategories.Contains(category))
                        vm.SelectedCategories.Remove(category);
                }
            }
        }
    }
}

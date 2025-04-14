using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System.Collections.ObjectModel;

namespace HojozatyCode.ViewModels
{
    [QueryProperty("Category", "Category")]
    public partial class CategoryVenuesViewModel : ObservableObject
    {
        //Properety to store the sub categories
        [ObservableProperty]
        private List<Subcategory> subCategories = new();

        //Properety to store the Main Category
        [ObservableProperty]
        private string category;

		public ObservableCollection<CitieisEnum> Cities { get; set; } = new ObservableCollection<CitieisEnum>((CitieisEnum[])Enum.
            GetValues(typeof(CitieisEnum)));

		[ObservableProperty]
		private CitieisEnum selectedCity;

		//Properety to store list of Venues related to that categroy
		[ObservableProperty]
        private ObservableCollection<Venue> venues = new();
         


        //Method apply when the user choose category
        partial void OnCategoryChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Task.Run(async () => await LoadVenuesForCategory(value));
            }
        }

        //Method to load the Correctv subcategories and the correct venues
        private async Task LoadVenuesForCategory(string categoryName)
        {
            //Store the all subcategories with their related photos
            var spaceCategories = new Dictionary<string, List<Subcategory>>
            {
                { "Wedding", new List<Subcategory>
                    {
                        new Subcategory { Name = "Halls", ImageUrl = "halls.png" },
                        new Subcategory { Name = "Farms", ImageUrl = "farms.jpg" },
                        new Subcategory { Name = "Hotels", ImageUrl = "hotel1.jpg" },
                        new Subcategory { Name = "Outdoors", ImageUrl = "outdoors.jpg" }
                    }
                },
                { "Entertainment", new List<Subcategory>
                    {
                        new Subcategory { Name = "Farms", ImageUrl = "farms.jpg" },
                        new Subcategory { Name = "Adventure Spots", ImageUrl = "adventure.png" },
                        new Subcategory { Name = "WorkShops", ImageUrl = "workshop.png" }
                    }
                },
                { "Meeting", new List<Subcategory>
                    {
                        new Subcategory { Name = "ClassRooms/Office Spaces", ImageUrl = "office.png" },
                        new Subcategory { Name = "Farms", ImageUrl = "farms.jpg" },
                        new Subcategory { Name = "Outdoor Space", ImageUrl = "outdoors.jpg" },
                        new Subcategory { Name = "Majls", ImageUrl = "majls.png" }
                    }
                },
                { "Funeral", new List<Subcategory>
                    {
                        new Subcategory { Name = "Diwan", ImageUrl = "diwan.png" },
                        new Subcategory { Name = "Dedicated Funeral Halls", ImageUrl = "funeral.png" }
                    }
                },
                { "Photography", new List<Subcategory>
                    {
                        new Subcategory { Name = "Photography Studios", ImageUrl = "photo.png" },
                        new Subcategory { Name = "Outdoor Photography Spaces", ImageUrl = "photoout.png" },
                        new Subcategory { Name = "Product Photography Spaces", ImageUrl = "product.png" }
                    }
                },
                { "Sport", new List<Subcategory>
                    {
                        new Subcategory { Name = "Stadium", ImageUrl = "stadium.png" }
                    }
                },
                { "Cultural Events", new List<Subcategory>
                    {
                        new Subcategory { Name = "Farms", ImageUrl = "farms.jpg" },
                        new Subcategory { Name = "Majls", ImageUrl = "majls.png" },
                        new Subcategory { Name = "Cultural Evening Venues", ImageUrl = "cultural.png" },
                        new Subcategory { Name = "Theaters and Cultural Halls", ImageUrl = "theater.png" }
                    }
                }
            };

            try
            {
                //Store the Correct sub categories
                SubCategories = spaceCategories.ContainsKey(categoryName) ? spaceCategories[categoryName] : new List<Subcategory>();

                //Returen the all venues related to this category
                var client = SupabaseConfig.SupabaseClient;
                var venuesResult = await client
                    .From<Venue>()
                    .Where(v => v.Type == categoryName)
                    .Get();

                Venues.Clear();

                //Store the all venues return from supabase to store it inside the Venue Observable properety
                foreach (var venue in venuesResult.Models)
                {
                    Venues.Add(venue);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load venues: {ex.Message}", "OK");
            }
        }

		    //Navigation Command to navigate me to the filter page
		    [RelayCommand]
		private async Task GoToFiltersPage()
		{
            if (!string.IsNullOrEmpty(Category))
			{
				await Shell.Current.GoToAsync($"{nameof(Pages.FiltersPage)}?category={Category}");
			}
		}




	}

}

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
        [ObservableProperty]
        private List<Subcategory> subCategories = new List<Subcategory>();

        [RelayCommand]
        private async Task GoToFiltersPage()
        {
            await Shell.Current.GoToAsync(nameof(Pages.FiltersPage));
        }

        [ObservableProperty]
        private string category;

        [ObservableProperty]
        private string location;

        [ObservableProperty]
        private ObservableCollection<Venue> venues;

        public CategoryVenuesViewModel()
        {
            Venues = new ObservableCollection<Venue>();
        }

        // Change this to partial method
        partial void OnCategoryChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Task.Run(async () => await LoadVenuesForCategory(value));
            }
        }

        private async Task LoadVenuesForCategory(string categoryName)
        {
			var spaceCategories = new Dictionary<string, List<Subcategory>>
			{
				{ "Wedding", new List<Subcategory>
					{
						new Subcategory { Name = "Halls" , ImageUrl = "farm_place.jpg" },
						new Subcategory { Name = "Farms" , ImageUrl = "farm_place.jpg" },
						new Subcategory { Name = "Hotels" , ImageUrl = "farm_place.jpg"  },
						new Subcategory { Name = "Outdoors" , ImageUrl = "farm_place.jpg"  }
					}
				},

				{ "Entertainment", new List<Subcategory>
					{
						new Subcategory { Name = "Farms" , ImageUrl = "farm_place.jpg"  },
						new Subcategory { Name = "Adventure Spots" , ImageUrl = "farm_place.jpg"},
						new Subcategory { Name = "WorkShops" , ImageUrl = "farm_place.jpg"}
					}
				},

				{ "Meeting", new List<Subcategory>
					{
						new Subcategory { Name = "ClassRooms/Office Spaces" , ImageUrl = "farm_place.jpg"},
						new Subcategory { Name = "Farms" , ImageUrl = "farm_place.jpg"},
						new Subcategory { Name = "Outdoor Space" , ImageUrl = "farm_place.jpg"},
						new Subcategory { Name = "Majls" , ImageUrl = "farm_place.jpg"}
					}
				},
				{ "Funeral", new List<Subcategory>
					{
						new Subcategory { Name = "Diwan" , ImageUrl = "farm_place.jpg"},
						new Subcategory { Name = "Dedicated Funeral Halls" , ImageUrl = "farm_place.jpg"}
					}
				},
				{ "Photography", new List<Subcategory>
					{
						new Subcategory { Name = "Photography Studios" , ImageUrl = "farm_place.jpg"},
						new Subcategory { Name = "Outdoor Photography Spaces" , ImageUrl = "farm_place.jpg"},
						new Subcategory { Name = "Product Photography Spaces" , ImageUrl = "farm_place.jpg"}	
					}
				},
				{ "Sport", new List<Subcategory>
					{
						new Subcategory { Name = "Stadium" , ImageUrl = "farm_place.jpg"}
					}
				},
				{ "Cultural Events", new List<Subcategory>
					{
						new Subcategory { Name = "Farms" , ImageUrl = "farm_place.jpg"},
						new Subcategory { Name = "Majls" , ImageUrl = "farm_place.jpg"},
						new Subcategory { Name = "Cultural Evening Venues" , ImageUrl = "farm_place.jpg"},
						new Subcategory { Name = "Theaters and Cultural Halls" , ImageUrl = "farm_place.jpg"}
					}
				}
			};

			try
            {
				// Update SubCategories using the ObservableProperty pattern
				SubCategories = spaceCategories.ContainsKey(Category) ? spaceCategories[Category] : new List<Subcategory>();

				var client = SupabaseConfig.SupabaseClient;
                var venuesResult = await client
                    .From<Venue>()
                    .Where(v => v.Type == categoryName)
                    .Get();

                Venues.Clear();
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
    }
}
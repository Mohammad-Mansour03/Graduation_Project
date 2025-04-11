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
						new Subcategory { Name = "Halls" , ImageUrl = "halls.png" },
						new Subcategory { Name = "Farms" , ImageUrl = "farms.jpg" },
						new Subcategory { Name = "Hotels" , ImageUrl = "hotel1.jpg"  },
						new Subcategory { Name = "Outdoors" , ImageUrl = "outdoors.jpg"  }
					}
				},

				{ "Entertainment", new List<Subcategory>
					{
						new Subcategory { Name = "Farms" , ImageUrl = "farms.jpg"  },
						new Subcategory { Name = "Adventure Spots" , ImageUrl = "adventure.png"},
						new Subcategory { Name = "WorkShops" , ImageUrl = "workshop.png"}
					}
				},

				{ "Meeting", new List<Subcategory>
					{
						new Subcategory { Name = "ClassRooms/Office Spaces" , ImageUrl = "office.png"},
						new Subcategory { Name = "Farms" , ImageUrl = "farms.jpg"},
						new Subcategory { Name = "Outdoor Space" , ImageUrl = "outdoors.jpg"},
						new Subcategory { Name = "Majls" , ImageUrl = "majls.png"}
					}
				},
				{ "Funeral", new List<Subcategory>
					{
						new Subcategory { Name = "Diwan" , ImageUrl = "diwan.png "},
						new Subcategory { Name = "Dedicated Funeral Halls" , ImageUrl = "funeral.png"}
					}
				},
				{ "Photography", new List<Subcategory>
					{
						new Subcategory { Name = "Photography Studios" , ImageUrl = "photo.png"},
						new Subcategory { Name = "Outdoor Photography Spaces" , ImageUrl = "photoout.png"},
						new Subcategory { Name = "Product Photography Spaces" , ImageUrl = "product.png}	
					}
				},
				{ "Sport", new List<Subcategory>
					{
						new Subcategory { Name = "Stadium" , ImageUrl = "stadium.png"}
					}
				},
				{ "Cultural Events", new List<Subcategory>
					{
						new Subcategory { Name = "Farms" , ImageUrl = "farms.jpg"},
						new Subcategory { Name = "Majls" , ImageUrl = "majls.png"},
						new Subcategory { Name = "Cultural Evening Venues" , ImageUrl = "cultural.png"},
						new Subcategory { Name = "Theaters and Cultural Halls" , ImageUrl = "theater.png"}
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
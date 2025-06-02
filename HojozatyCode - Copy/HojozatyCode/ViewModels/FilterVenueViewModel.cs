using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatyCode.ViewModels
{
	[QueryProperty(nameof(SpaceType), "category")]
	public partial class FilterVenueViewModel : ObservableObject
	{
		private readonly Dictionary<string, List<string>> SpaceTypeCategories = new()
		{
			{ "Wedding", new List<string> { "Halls", "Farms", "Hotels", "Outdoor Space" } },
			{ "Entertainment", new List<string> { "Farms", "Adventure Spots", "WorkShops" } },
			{ "Meeting", new List<string> { "ClassRooms/Office Spaces", "Farms", "Outdoor Space", "Majls" } },
			{ "Funeral", new List<string> { "Diwan", "Dedicated Funeral Halls" } },
			{ "Photography", new List<string> { "Photography Studios", "Outdoor Photography Spaces", "Product Photography Spaces" } },
			{ "Sports", new List<string> { "Staduim" } },
			{ "Cultural Events", new List<string> { "Farms", "Majls", "Cultrual Evening Venues", "Theaters and Cultural halls" } }
		};
		public ObservableCollection<string> AvailableCategories { get; } = new();
		public ObservableCollection<string> SelectedCategories { get; } = new();


		//Properties Related to the Filter
		[ObservableProperty]
		private double minPrice;

		[ObservableProperty]
		private double maxPrice;

		[ObservableProperty]
		private int minCapacity;

		[ObservableProperty]
		private int maxCapacity;

		[ObservableProperty]
		private CitieisEnum filterCity;

		[ObservableProperty]
		private string errorMessage;

		// Automatically populates AvailableCategories when SpaceType is set
		[ObservableProperty]
		private string spaceType;


		//Collection to store the all cities
		public ObservableCollection<CitieisEnum> FiltersCities { get; set; } =
			new ObservableCollection<CitieisEnum>((CitieisEnum[])Enum.
			GetValues(typeof(CitieisEnum)));


		partial void OnSpaceTypeChanged(string value)
		{
			LoadAvailableCategories(value);
		}

		private void LoadAvailableCategories(string type)
		{
			AvailableCategories.Clear();

			if (!string.IsNullOrWhiteSpace(type) && SpaceTypeCategories.TryGetValue(type, out var categories))
			{
				foreach (var category in categories)
				{
					AvailableCategories.Add(category);
				}
			}
			else
			{
				ErrorMessage = $"No categories found for space type: {type}";
			}
		}


		[RelayCommand]
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync(nameof(Pages.CategoryVenuesPage));
        }

		//Method to Store the Filter Data That user entered it
        [RelayCommand]
		public async Task ApplyFilters()
		{
			if (MinPrice < 0) 
			{
				ErrorMessage = "The Min Price must be 0 or greater";
				MinPrice = 0;
				return;
			}
			
			if (MaxPrice < 0) 
			{
				ErrorMessage = "The Max Price must be 0 or greater";
				MaxPrice = 0;
				return;
			}	
			
			if (MinCapacity < 0) 
			{
				ErrorMessage = "The Min Capacity must be 0 or greater";
				MinCapacity = 0;
				return;
			}	
			
			if (MaxCapacity < 0) 
			{
				ErrorMessage = "The Max Capacity must be 0 or greater";
				MinPrice = 0;
				return;
			}	
			
			var filter = new VenueFilter
			{
				MinPrice = MinPrice,
				MaxPrice = MaxPrice,
				MinCapacity = MinCapacity,
				MaxCapacity = MaxCapacity,
				FilterCity = FilterCity,
				SelectedCategories = SelectedCategories.ToList()
			};

			// We will move to the previous page with filtered data
			await Shell.Current.GoToAsync("..", new Dictionary<string, object>
			{
				{ "FilterData", filter }
			});
		}

		//Clear The All Data
		[RelayCommand]
		public void ClearFilters() 
		{
			MinPrice = MaxPrice = MinCapacity = MaxCapacity = 0;
		}

	}
}

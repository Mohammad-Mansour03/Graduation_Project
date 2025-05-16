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
	public partial class FilterVenueViewModel : ObservableObject
	{

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

		//Collection to store the all cities
		public ObservableCollection<CitieisEnum> FiltersCities { get; set; } =
			new ObservableCollection<CitieisEnum>((CitieisEnum[])Enum.
			GetValues(typeof(CitieisEnum)));

	

		//public void ApplyQueryAttributes(IDictionary<string, object> query)
		//{
		//	if (query.TryGetValue("category", out var cat))
		//	{
		//		string category = cat?.ToString() ?? "";

		//		List<SpaceType> types = category switch
		//		{
		//			"Wedding" => new()
		//		{
		//			new SpaceType { Name = "Halls" },
		//			new SpaceType { Name = "Farms" },
		//			new SpaceType { Name = "Hotels" },
		//			new SpaceType { Name = "Outdoors"}
		//		},
		//			"Entertainment" => new()
		//		{
		//			new SpaceType { Name = "Farms" },
		//			new SpaceType { Name = "Adventure Spots" },
		//			new SpaceType { Name = "WorkShops" },
		//		},
		//			"Meeting" => new()
		//		{
		//			new SpaceType { Name = "ClassRooms/Office Spaces" },
		//			new SpaceType { Name = "Farms" },
		//			new SpaceType { Name = "Outdoor Space" },
		//			new SpaceType { Name = "Majls" },
		//		},
		//			"Funeral" => new()
		//		{
		//			new SpaceType { Name = "Diwan" },
		//			new SpaceType { Name = "Dedicated Funeral Halls"  },
					
		//		},
		//			"Photography" => new()
		//		{
		//			new SpaceType { Name = "Photography Studios" },
		//			new SpaceType { Name = "Outdoor Photography Spaces" },
		//			new SpaceType { Name = "Product Photography Spaces" },
		//		},
		//			"Sport" => new()
		//		{
		//			new SpaceType { Name = "Staduim" },
				
		//		},
		//			"Cultural Events" => new()
		//		{
		//			new SpaceType { Name = "Farms" },
		//			new SpaceType { Name = "Majls" },
		//			new SpaceType { Name = "Cultural Evening Venues" },
		//			new SpaceType { Name = "Theaters and Cultural Halls" },
		//		},
		//			_ => new() 
		//			{
		//				new SpaceType{ Name = "No Category Value"}
		//			}
		//		};

		//		// Update on the UI thread for immediate refresh
		//		MainThread.BeginInvokeOnMainThread(() => {
		//			SpaceTypes = new ObservableCollection<SpaceType>(types);
		//		});
				
		//		// Add this to verify the collection is populated
		//		Shell.Current.DisplayAlert("Prompt",$"{SpaceTypes?.Count}","OK");
		//	}
		//}


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

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
	public partial class FilterVenueViewModel : ObservableObject, IQueryAttributable
	{
		[ObservableProperty]
		ObservableCollection<SpaceType> spaceTypes = new();

		// ⬅️ الأنواع اللي المستخدم اختارها
		[ObservableProperty]
		private ObservableCollection<string> selectedSpaceTypes = new();

		//// ⬅️ السعر الأقصى المختار من السلايدر
		//[ObservableProperty]
		//private double maxPrice = 5000;

		// ⬅️ النتيجة بعد التصفية
		[ObservableProperty]
		private ObservableCollection<Venue> filteredVenues = new();



		// خصائص مرتبطة بالـ UI للفلترة
		[ObservableProperty]
		private double minPrice;

		[ObservableProperty]
		private double maxPrice;

		[ObservableProperty]
		private int minCapacity;

		[ObservableProperty]
		private int maxCapacity;


		// ⬅️ الحضور المختار
		[ObservableProperty]
		private string selectedAttendees = "Any";

		public void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			if (query.TryGetValue("category", out var cat))
			{
				string category = cat?.ToString() ?? "";

				List<SpaceType> types = category switch
				{
					"Wedding" => new()
				{
					new SpaceType { Name = "Halls" },
					new SpaceType { Name = "Farms" },
					new SpaceType { Name = "Hotels" },
					new SpaceType { Name = "Outdoors"}
				},
					"Entertainment" => new()
				{
					new SpaceType { Name = "Farms" },
					new SpaceType { Name = "Adventure Spots" },
					new SpaceType { Name = "WorkShops" },
				},
					"Meeting" => new()
				{
					new SpaceType { Name = "ClassRooms/Office Spaces" },
					new SpaceType { Name = "Farms" },
					new SpaceType { Name = "Outdoor Space" },
					new SpaceType { Name = "Majls" },
				},
					"Funeral" => new()
				{
					new SpaceType { Name = "Diwan" },
					new SpaceType { Name = "Dedicated Funeral Halls"  },
					
				},
					"Photography" => new()
				{
					new SpaceType { Name = "Photography Studios" },
					new SpaceType { Name = "Outdoor Photography Spaces" },
					new SpaceType { Name = "Product Photography Spaces" },
				},
					"Sport" => new()
				{
					new SpaceType { Name = "Staduim" },
				
				},
					"Cultural Events" => new()
				{
					new SpaceType { Name = "Farms" },
					new SpaceType { Name = "Majls" },
					new SpaceType { Name = "Cultural Evening Venues" },
					new SpaceType { Name = "Theaters and Cultural Halls" },
				},
					_ => new() 
					{
						new SpaceType{ Name = "No Category Value"}
					}
				};

				// Update on the UI thread for immediate refresh
				MainThread.BeginInvokeOnMainThread(() => {
					SpaceTypes = new ObservableCollection<SpaceType>(types);
				});
				
				// Add this to verify the collection is populated
				Shell.Current.DisplayAlert("Prompt",$"{SpaceTypes?.Count}","OK");
			}
		}

		[RelayCommand]
		public async Task ApplyFilters()
		{
			//	var client = SupabaseConfig.SupabaseClient;

			//	var venues = await client.From<Venue>().Get();

			//	var filtered = venues.Models
			//		.Where(v =>
			//			v.InitialPrice <= MaxPrice &&
			//			(SelectedAttendees == "Any" ||
			//			 (SelectedAttendees == "50 - 100" && v.Capacity >= 50 && v.Capacity <= 100) ||
			//			 (SelectedAttendees == "100 - 200" && v.Capacity >= 100 && v.Capacity <= 200)) &&
			//			(SelectedSpaceTypes.Count == 0 || SelectedSpaceTypes.Contains(v.Type))
			//		)
			//		.ToList();

			//	FilteredVenues = new ObservableCollection<Venue>(filtered);
			//}

			await Shell.Current.DisplayAlert("Prompt", "Inside the apply filter method", "OK");
			var filter = new VenueFilter
			{
				MinPrice = MinPrice,
				MaxPrice = MaxPrice,
				MinCapacity = MinCapacity,
				MaxCapacity = MaxCapacity
			};

			// نرجع للصفحة السابقة (CategoryVenuePage) ونرسل بيانات الفلتر معها
			await Shell.Current.GoToAsync("..", new Dictionary<string, object>
			{
				{ "FilterData", filter }
			});
		}

	}
}

using CommunityToolkit.Mvvm.ComponentModel;
using HojozatyCode.Models;
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
					new SpaceType { Name = "Diwan" , IsSelected = "Yes" },
					new SpaceType { Name = "Dedicated Funeral Halls" , IsSelected = "No" },
					
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

				SpaceTypes = new ObservableCollection<SpaceType>(types);

				Shell.Current.DisplayAlert("Prompt",$"{SpaceTypes?.Count}","OK");
			}
		}


	}
}

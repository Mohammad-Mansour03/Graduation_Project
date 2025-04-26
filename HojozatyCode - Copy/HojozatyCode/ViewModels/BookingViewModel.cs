using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HojozatyCode.Models;
using HojozatyCode.Services;
using Microsoft.Maui.Devices;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;


namespace HojozatyCode.ViewModels
{

	public partial class BookingViewModel : ObservableObject, IQueryAttributable
	{

		[ObservableProperty]
		private string venueIdRaw;

		[ObservableProperty]
		private Guid venueId;

		// Selected venue for booking
		[ObservableProperty]
		private Venue selectedVenue;

		[ObservableProperty]
		private ObservableCollection<HostRules> hostRulesVenue = new();

		[ObservableProperty]
		private ObservableCollection<ServiceItem> servicesVenue = new ObservableCollection<ServiceItem>();

		[ObservableProperty]
		private string errorMessage;

		// Constructor - private to enforce singleton pattern
		public BookingViewModel()
		{
			// Initialize if needed
		}
		public async void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			// نحصل على البيانات المرسلة باسم "SelectedVenue"
			if (query.TryGetValue("SelectedVenue", out var venueData))
			{
				SelectedVenue = venueData as Venue;

				if (SelectedVenue != null)
				{
					await LoadHostRules();
					await LoadServices();
				}
			}
		}



		private async Task LoadHostRules()
		{
			try
			{
				if (SelectedVenue == null || SelectedVenue.VenueId == Guid.Empty)
				{
					ErrorMessage = "Invalid Venue ID";
					return;
				}

				//Get Host Rules Id's related to this Venue 
				var hostRulesVenues = await SupabaseConfig.SupabaseClient
						   .From<HostRulesVenues>() // هذا يمثل الجدول الوسيط
						   .Where(x => x.VenueId == SelectedVenue.VenueId)
						   .Get();

				var hostRuleIds = hostRulesVenues.Models.Select(x => x.HostRuleId).ToList();

				if (hostRuleIds.Count == 0)
				{
					await Shell.Current.DisplayAlert("Prompt", $"The Count of Host rules was {hostRuleIds.Count}", "OK");
					HostRulesVenue = new ObservableCollection<HostRules>();
					return;
				}

				// Step 2: Get the HostRules based on these IDs
				foreach (var hostRuleId in hostRuleIds)
				{
					var hostRulesForId = await SupabaseConfig.SupabaseClient
									.From<HostRules>()
									.Where(x => x.HostRuleId == hostRuleId)
									.Get();

					HostRulesVenue.Add(hostRulesForId.Model);
				}

			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"There was an {ex} Exception", "OK");
			}
		}

		private async Task LoadServices()
		{
			try
			{
				if (SelectedVenue == null || SelectedVenue.VenueId == Guid.Empty)
				{
					ErrorMessage = "Invalid Venue ID";
					return;
				}

				//Get Host Rules Id's related to this Venue 
				var servicesVenues = await SupabaseConfig.SupabaseClient
						   .From<VenueServices>() // هذا يمثل الجدول الوسيط
						   .Where(x => x.VenueId == SelectedVenue.VenueId)
						   .Get();

				var serviceVenueId = servicesVenues.Models.ToList();

				if (serviceVenueId.Count == 0)
				{
					await Shell.Current.DisplayAlert("Prompt", $"The Count of Host rules was {serviceVenueId.Count}", "OK");
					HostRulesVenue = new ObservableCollection<HostRules>();
					return;
				}


				var serviceDisplayList = new ObservableCollection<ServiceItem>();

				// Step 2: Get the HostRules based on these IDs
				foreach (var serviceId in serviceVenueId)
				{
					var serviceForId = await SupabaseConfig.SupabaseClient
									.From<Service>()
									.Where(x => x.ServiceId == serviceId.ServiceId)
									.Get();

					if (serviceForId.Model != null)
					{
						serviceDisplayList.Add(new ServiceItem
						{
							Name = serviceForId.Model.ServiceName,
							Description = serviceForId.Model.Description,
							Price = serviceId.PricePerUnit // نأخذ السعر من الجدول الوسيط
						});
					}
				}
				ServicesVenue = serviceDisplayList;
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", $"There was an {ex} Exception", "OK");
			}
		}
	}
}
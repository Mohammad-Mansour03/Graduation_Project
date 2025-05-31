using CommunityToolkit.Mvvm.ComponentModel;
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
	public partial class ViewBookingsPopupViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<Booking> bookings;

		[ObservableProperty]
		private ObservableCollection<UserBooking> userBookings = new();

		public ViewBookingsPopupViewModel(ObservableCollection<Booking> bookings)
		{
			Bookings = bookings;

			 _ = LoadUserBookings();	
		}

		private async Task LoadUserBookings() 
		{
			try
			{
				// Step 1: استخراج كل UserId من الحجوزات بدون تكرار
				var userIds = Bookings.Select(b => b.UserId).Distinct().ToList();

				// Step 2: جلب كل المستخدمين دفعة واحدة
				var userTasks = userIds.Select(async userId =>
				{
					var response = await SupabaseConfig.SupabaseClient
						.From<User>()
						.Filter("user_id", Supabase.Postgrest.Constants.Operator.Equals, userId.ToString())
						.Single();

					return response;
				});

				var users = await Task.WhenAll(userTasks);

				var userDict = users
					.Where(u => u != null)
					.ToDictionary(u => u.UserIdC, u => u);

				UserBookings.Clear();

				foreach (var book in Bookings)
				{
					userDict.TryGetValue(book.UserId, out var user);
					var fullName = user != null ? $"{user.FirstNameC} {user.LastNameC}" : "غير معروف";

					UserBookings.Add(new UserBooking
					{
						CustomerName = fullName,
						StartDateTime = book.StartDateTime,
						EndDateTime = book.EndDateTime
					});
				}


			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error",$"Error loading user bookings: {ex.Message}","OK");
			}
		}

	}
}

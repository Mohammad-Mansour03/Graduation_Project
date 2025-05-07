using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatyCode.Models
{
	public partial class ServiceItem : ObservableObject
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }		
		public Guid ServiceId { get; set; }

		[ObservableProperty]
		private int quantity = 0;

		[RelayCommand]
		void DecreasedQuantity() 
		{
			if (Quantity > 0)
				Quantity--;
			else 
				Quantity = 0;
		}
		
		[RelayCommand]
		void IncreaseQuantity() 
		{
			Quantity++;
		}


	}
}

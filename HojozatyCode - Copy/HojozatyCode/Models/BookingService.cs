using Supabase.Postgrest.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HojozatyCode.Models
{
	[Table("Booking_Service")]  // Ensure this matches the actual Supabase table name
	public class BookingService : Supabase.Postgrest.Models.BaseModel
	{

		// Foreign key to Venues table
		[Column("booking_id")]
		public Guid BookingId { get; set; }

		// Foreign key to Services table
		[Column("service_id")]
		public Guid ServiceId { get; set; }

		// Price per unit
		[Column("total_price")]
		public double TotalPrice { get; set; }
		
		// Price per unit
		[Column("quantity")]
		public int Quantity { get; set; }

		//To store the Service name
		[IgnoreDataMember]
		public string Name { get; set; }

	}
}
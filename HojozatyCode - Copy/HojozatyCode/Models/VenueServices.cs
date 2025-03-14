using Supabase.Postgrest.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HojozatyCode.Models
{
	[Table("Venues_Service")]  // Ensure this matches the actual Supabase table name
	public class VenueServices : Supabase.Postgrest.Models.BaseModel
	{
		//store the venue id
		[PrimaryKey("venue_id", false)]
		public Guid VenueId { get; set; } = Guid.NewGuid(); // Default to gen_random_uuid()	
		
		//store the service id
		[PrimaryKey("service_id", false)]
		public Guid ServiceId { get; set; } = Guid.NewGuid(); // Default to gen_random_uuid()

		//Store the venue type
		[Column("price_per_unit")]
		public string PricePerUnit { get; set; }
	}
}
using Supabase.Postgrest.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HojozatyCode.Models
{
	[Table("Services")]  // Ensure this matches the actual Supabase table name
	public class Service : Supabase.Postgrest.Models.BaseModel
	{
		//store the venue id
		[PrimaryKey("service_id", false)]
		public Guid ServiceId { get; set; } = Guid.NewGuid(); // Default to gen_random_uuid()

		//Store the venue name
		[Required(ErrorMessage = "Venue Name is required")]
		[Column("service_name")]
		public string ServiceName { get; set; }

		//Store the description for the venue 
		[Column("description")]
		public string Description { get; set; }


	

	}
}
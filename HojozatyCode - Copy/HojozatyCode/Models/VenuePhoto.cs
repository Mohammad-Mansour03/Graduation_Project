using Supabase.Postgrest.Attributes;
using System;

namespace HojozatyCode.Models
{
    [Table("Venue_Photos")]  // Ensure this matches the actual Supabase table name
    public class VenuePhoto : Supabase.Postgrest.Models.BaseModel
    {
        [PrimaryKey("photo_id", false)]
        public Guid PhotoId { get; set; } = Guid.NewGuid(); // Default to gen_random_uuid()

        [Column("venue_id")]
        public Guid VenueId { get; set; } // Foreign key relation to Venues.venue_id

        [Column("photo_url")]
        public string PhotoUrl { get; set; } // URL to the photo in the bucket
    }
}
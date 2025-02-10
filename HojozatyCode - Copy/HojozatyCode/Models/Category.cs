using Supabase.Postgrest.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HojozatyCode.Models
{
    [Table("Categories")]  // Ensure this matches the actual Supabase table name
    public class Category : Supabase.Postgrest.Models.BaseModel
    {
        [PrimaryKey("category_id", false)]
        public Guid CategoryId { get; set; } = Guid.NewGuid(); // Default to gen_random_uuid()

        [Column("venue_id")]
        public Guid? VenueId { get; set; } // Foreign key relation to Venue.venue_id

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}
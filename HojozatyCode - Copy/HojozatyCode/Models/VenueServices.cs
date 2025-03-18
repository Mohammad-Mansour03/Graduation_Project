using Supabase.Postgrest.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HojozatyCode.Models
{
    [Table("Venues_Service")]  // Ensure this matches the actual Supabase table name
    public class VenueServices : Supabase.Postgrest.Models.BaseModel
    {

        // Foreign key to Venues table
        [Column("venue_id")]
        public Guid VenueId { get; set; }
        
        // Foreign key to Services table
        [Column("service_id")]
        public Guid ServiceId { get; set; }

        // Price per unit
        [Column("price_per_unit")]
        public double PricePerUnit { get; set; }
    }
}
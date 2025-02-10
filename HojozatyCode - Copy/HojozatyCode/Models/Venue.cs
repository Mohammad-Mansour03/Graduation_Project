using Supabase.Postgrest.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HojozatyCode.Models
{
    [Table("Venues")]  // Ensure this matches the actual Supabase table name
    public class Venue : Supabase.Postgrest.Models.BaseModel
    {
        [PrimaryKey("venue_id", false)]
        public Guid VenueId { get; set; } = Guid.NewGuid(); // Default to gen_random_uuid()

        [Required(ErrorMessage = "Owner ID is required")]
        [Column("owner_id")]
        public Guid OwnerId { get; set; } // Foreign key relation to Profile.user_id

        [Required(ErrorMessage = "Venue Name is required")]
        [Column("venue_name")]
        public string VenueName { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        [Column("capacity")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [Column("location")]
        public string Location { get; set; }

        [Column("venue_contact_phone")]
        public string VenueContactPhone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column("venue_email")]
        public string VenueEmail { get; set; }

        [Column("initial_price")]
        public double InitialPrice { get; set; }
    }
}
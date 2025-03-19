using Supabase.Postgrest.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HojozatyCode.Models
{
    [Table("Venues")]  // Ensure this matches the actual Supabase table name
    public class Venue : Supabase.Postgrest.Models.BaseModel
    {
        //store the venue id
        [PrimaryKey("venue_id", false)]
        public Guid VenueId { get; set; } = Guid.NewGuid(); // Default to gen_random_uuid()

        //store the user id (User who has this venue)
        [Required(ErrorMessage = "Owner ID is required")]
        [Column("owner_id")]
        public Guid OwnerId { get; set; } // Foreign key relation to Profile.user_id

        //Store the venue name
        [Required(ErrorMessage = "Venue Name is required")]
        [Column("venue_name")]
        public string VenueName { get; set; }

        //Store the description for the venue 
        [Column("description")]
        public string Description { get; set; }

        //Store the venue type
        [Column("type")]
        public string Type { get; set; }

        //Store what is the type for this venue
        [Required(ErrorMessage = "Capacity is required")]
        [Column("capacity")]
        public int Capacity { get; set; }

        //Store the location for this venue
        [Required(ErrorMessage = "Location is required")]
        [Column("location")]
        public string Location { get; set; }

        //Store the venue contact phone
        [Column("venue_contact_phone")]
        public string VenueContactPhone { get; set; }

        //Store the venue email address
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column("venue_email")]
        public string VenueEmail { get; set; }

        //Store the venue initial price
        [Column("initial_price")]
        public double InitialPrice { get; set; }

        //To store the all images for the venue
        [Column("image_url")]
        public string ImageUrl { get; set; } // URL to the venue image in the bucket

        // Add status field
        [Column("status")]
        public string Status { get; set; }

        // Property to get the list of image URLs with null safety
        // public List<string> ImageUrls 
        // {
        //     get
        //     {
        //         if (string.IsNullOrEmpty(ImageUrl))
        //             return new List<string>();
                    
        //         // Split the URLs and filter out any empty or null entries
        //         return ImageUrl.Split(',')
        //             .Where(url => !string.IsNullOrWhiteSpace(url))
        //             .ToList();
        //     }
        // }
    }
}
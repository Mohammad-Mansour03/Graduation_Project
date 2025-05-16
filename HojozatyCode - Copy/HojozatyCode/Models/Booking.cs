using Supabase.Postgrest.Attributes;
using System;

namespace HojozatyCode.Models
{
    [Table("Bookings")]
    public class Booking : Supabase.Postgrest.Models.BaseModel
    {
        [PrimaryKey("booking_id", false)]
        public Guid BookingId { get; set; }
        
        [Column("user_id")]
        public Guid UserId { get; set; }
        
        [Column("venue_id")]
        public Guid VenueId { get; set; }
        
        [Column("start_datetime")]
        public DateTime StartDateTime { get; set; }
        
        [Column("end_datetime")]
        public DateTime EndDateTime { get; set; }
        
        
        [Column("total_price")]
        public double TotalPrice { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
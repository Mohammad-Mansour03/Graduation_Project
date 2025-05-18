using Supabase.Postgrest.Attributes;
using System;

namespace HojozatyCode.Models
{
    [Table("user_favorites")]
    public class UserFavorite : Supabase.Postgrest.Models.BaseModel
    {

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("venue_id")]
        public Guid VenueId { get; set; }
    }
}
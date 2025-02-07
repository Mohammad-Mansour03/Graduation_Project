using Supabase.Postgrest.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HojozatyCode.Models
{
    [Table("Profile")]  // Ensure this matches the actual Supabase table name
    public class User : Supabase.Postgrest.Models.BaseModel
    {
        [PrimaryKey("user_id", false)]
        public Guid UserIdC { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Column("first_name")]
        public string FirstNameC { get; set; }

        [Column("middle_name")]
        public string MiddleNameC { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Column("last_name")]
        public string LastNameC { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column("email")]
        public string EmailC { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Column("phone")]
        public string PhoneC { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        //[Column("password")]
        //public string Password { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120")]
        [Column("age")]
        public int AgeC { get; set; }

        [Column("user_type")]
        public string UserTypeC { get; set; }

        [Column("date_created")]
        public DateTime DateCreatedC { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [Column("gender")]
        public string GenderC { get; set; }

        // Override Equals for Supabase compatibility
        //public override bool Equals(object obj)
        //{
        //    return obj is User user && Id == user.Id;
        //}

        //public override int GetHashCode()
        //{
        //    return HashCode.Combine(Id);
        //}
    }
}
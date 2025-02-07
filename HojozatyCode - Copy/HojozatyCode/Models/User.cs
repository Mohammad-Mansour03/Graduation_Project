using Supabase.Postgrest.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HojozatyCode.Models
{
    [Table("Users")]  // Ensure this matches the actual Supabase table name
    public class User : Supabase.Postgrest.Models.BaseModel
    {
        [PrimaryKey("id", false)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("middle_name")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Column("last_name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column("email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Column("phone")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Column("password")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120")]
        [Column("age")]
        public int Age { get; set; }

        [Column("user_type")]
        public string? UserType { get; set; }

        [Column("date_created")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [Column("gender")]
        public string? Gender { get; set; }

        // Override Equals for Supabase compatibility
        public override bool Equals(object? obj)
        {
            return obj is User user && Id == user.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
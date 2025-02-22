using Supabase.Postgrest.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HojozatyCode.Models
{
    [Table("Profile")]  // Ensure this matches the actual Supabase table name
    public class User : Supabase.Postgrest.Models.BaseModel
    {
     
        //Store the user id
        [PrimaryKey("user_id", false)]
        public Guid UserIdC { get; set; }

       //Store the user first name
        [Required(ErrorMessage = "First Name is required")]
        [Column("first_name")]
        public string FirstNameC { get; set; }

        //store the user middle name
        [Column("middle_name")]
        public string MiddleNameC { get; set; }

        //store the user last name
        [Required(ErrorMessage = "Last Name is required")]
        [Column("last_name")]
        public string LastNameC { get; set; }

        //store the user email
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column("email")]
        public string EmailC { get; set; }

        //store the user phone
        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Column("phone")]
        public string PhoneC { get; set; }

        //store the user age
        [Required(ErrorMessage = "Age is required")]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120")]
        [Column("age")]
        public int AgeC { get; set; }

        //store the user type
        [Column("user_type")]
        public string UserTypeC { get; set; }

        //store when the account was created 
        [Column("date_created")]
        public DateTime DateCreatedC { get; set; }

        //store the user gender
        [Required(ErrorMessage = "Gender is required")]
        [Column("gender")]
        public string GenderC { get; set; }


    }
}
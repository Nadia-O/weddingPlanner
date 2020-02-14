using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace WeddingPlanner.Models
{
    public class User
    {
        public class StrongPasswordAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string check;
                if(value is string)
                {
                    check = (string)value;
                    Regex VARIABLE = new Regex ("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[#?!@$%^&*-]).{8,20}$");
                    Match password = VARIABLE.Match(check);
                    if(password.Success)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult("Password must be 8-20 characters, contain at least one upper case, one lower case, one number, and one special character.");
                    }
                }
                else
                {
                    return new ValidationResult("Password must be 8-20 characters, contain at least one upper case, one lower case, one number, and one special character.");
                }
            }
        }
        [Key]
        public int UserId {get; set;}

        [Required(ErrorMessage="First name required.")]
        [MinLength(3,ErrorMessage="Must be at least 3 characters.")]
        [Display(Name="First Name")]
        public string FirstName {get; set;}

        [Required(ErrorMessage="Last name required.")]
        [MinLength(3,ErrorMessage="Must be at least 3 characters.")]
        [Display(Name="Last Name")]
        public string LastName {get; set;}

        [Required]
        [EmailAddress]
        public string Email {get; set;}

        [Required]
        [StrongPassword]
        [DataType(DataType.Password)]
        public string Password {get; set;}

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage="Passwords don't match.")]
        [Display(Name="Confirm Password")]
        public string ConfirmPassword {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        // user can plan many weddings
        public List <Wedding> PlannedWeddings {get; set;}

        // user can go to many weddings
        public List <RSVP> GoingWeddings {get; set;}
    }
}
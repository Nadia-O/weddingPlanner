using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class FutureDateAttribute : ValidationAttribute
        {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime check;
            if(value is DateTime)
            {
                check = (DateTime)value;
            }
            else
            {
                return new ValidationResult("Invalid Date");
            }
            if(check < DateTime.Now)
            {
                return new ValidationResult("Your wedding date must be in the future!");
            }
            return ValidationResult.Success;
        }
    }
    public class Wedding
    {
        [Key]
        [Required]
        public int WeddingId {get; set;}

        [Required]
        [FutureDate]
        public DateTime Date {get; set;}

        [Required]
        public string WedderOne {get; set;}

        [Required]
        public string WedderTwo {get; set;}

        [Required]
        public string Address {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
        public int UserId {get; set;}

        // users can plan AND attend...

        // one to many - wedding can only be planned by ONE user
        public User Planner {get; set;}

        // many to many - wedding can be attended by many users / guests
        public List <RSVP> GuestList {get; set;}
    }
}
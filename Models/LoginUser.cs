using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="Email required.")]
        [EmailAddress]
        [Display(Name="Email")]
        public string LoginEmail {get; set;}

        [Required(ErrorMessage="Password required.")]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string LoginPassword {get; set;}
    }
}
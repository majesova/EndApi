using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EndApi.Models
{
    public class AddUserDto
    {
        [Required]
        [StringLength(300)]
        public string Name { get; set; } 
        public DateTime? BirthDate { get; set; } 
        [StringLength(10)]
        public string Initial { get; set; } 
        [StringLength(14)]
        public string Unicode { get; set; } 
        [StringLength(300)]
        [EmailAddress]
        public string Email { get; set; } 
        [Required]
        public bool Followup { get; set; } //If true, the user es followed by creator
        [Required]
        public bool HasAccount { get; set; }// if true the AppUser is created
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string Password { get; set; }
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
        
    }
}
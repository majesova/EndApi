using System;
using System.ComponentModel.DataAnnotations;

namespace EndApi.Models
{
    public class AddUserDto
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        [StringLength(10)]
        public string Initial { get; set; }
        [StringLength(8)]
        public string Unicode { get; set; }
        [StringLength(300)]
        public string Email { get; set; }
        [Required]
        public bool Follow { get; set; } //If true, the user es followed by creator
        public string Password { get; set; }
        public bool HasAccount { get; set; }
    }
}
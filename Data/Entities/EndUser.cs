using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System;
using System.Collections.Generic;

namespace EndApi.Data
{
    public class EndUser
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        [StringLength(10)]
        public string Initial { get; set; }
        [StringLength(14)]
        [MaxLength(14)]
        public string Unicode { get; set; }
        [StringLength(300)]
        public string Email { get; set; }
        public List<Following> Followings { get; set; }
    }
}
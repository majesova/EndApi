using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EndApi.Data
{
    [Table("FoodPictures")]
    public class FoodPicture
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string FriendlName { get; set; }
        [Required]
        public string Extension { get; set; }
        [Required]
        public string MIMEType { get; set; }
        public Food Food { get; set; }
        public string FoodId { get; set; }
    }
}
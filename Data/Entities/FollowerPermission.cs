using System.ComponentModel.DataAnnotations;

namespace EndApi.Data
{
    public class FollowerPermission
    {
        [Key]
        [Required]
        public string Key { get; set; }  
        [Required] 
        public string Name { get; set; }
        [Required]
        public bool isActive { get; set; }
    }
}
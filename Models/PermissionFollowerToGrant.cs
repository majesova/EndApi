using System.ComponentModel.DataAnnotations;

namespace EndApi.Models{
    public class PermissionFollowerToGrant
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Value { get; set; }
        [Required]
        public int Order { get; set; }
    }
}
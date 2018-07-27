using System.ComponentModel.DataAnnotations;

namespace EndApi.Data
{
    public class GrantedFollowerPermission
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Value { get; set; }
        [Required]
        public string FollowingId { get; set; }
        public Following Following { get; set; }
        [Required]
        public int Order { get; set; }

    }
}
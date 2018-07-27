using System.ComponentModel.DataAnnotations;

namespace EndApi.Models
{
    public class FollowDto
    {
        [Required]
        public string UserId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System;
namespace EndApi.Data
{
    public class Following
    {
        [Key]
        public string Id { get; set;}
        [Required] 
        public DateTime? CreatedAt { get; set; }
        public EndUser FollowedUser { get; set; }
        public string FollowedUserId { get; set; }
        public EndUser FollowedBy { get; set; }
        public string FollowedById { get; set; }
    }
}
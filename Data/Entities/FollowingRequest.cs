using System;
using System.ComponentModel.DataAnnotations;

namespace EndApi.Data
{
    public class FollowingRequest
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public DateTime? CreatedAt { get; set; }
        [Required]
        public string RequesterId { get; set; }
        [Required]
        public string FollowedId { get; set; }
        public EndUser Requester { get; set; }
        public EndUser Followed { get; set; }
        [Required]
        public FollowingRequestStatus Status { get; set; }
    }
}
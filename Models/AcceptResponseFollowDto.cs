using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EndApi.Models {
    public class AcceptResponseFollowDto
    {
        
        public string Id { get; set;}
        [Required] 
        public DateTime? CreatedAt { get; set; }
        public string FollowedUserId { get; set; }
        public string FollowedById { get; set; }
        public List<PermissionFollowerToGrant> Permissions {get; set;}
    }

}
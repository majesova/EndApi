using System;
using System.Collections.Generic;

namespace EndApi.Models
{
    public class FollowToAcceptDto
    {
        public string FollowingId { get; set; }
        public string FollowerName { get; set; }
        public DateTime RequestDate { get; set; }
        public List<PermissionFollowerToGrant> Permissions {get; set;}
    }
}
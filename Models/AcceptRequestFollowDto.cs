using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EndApi.Models {
    public class AcceptRequestFollowDto
    {
        public string FollowingRequestId { get; set; }
        public List<PermissionFollowerToGrant> Permissions {get; set;}
    }

    


}
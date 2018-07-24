using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EndApi.Models.Security
{
    public class AuthenticatedUserClaim
    {
        [Required()]
        public string ClaimType { get; set; }

        [Required()]
        public string ClaimValue { get; set; }
    }
}
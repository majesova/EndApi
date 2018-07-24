using System.Collections.Generic;

namespace EndApi.Models.Security
{
    public class AuthenticatedUser
    {
        public AuthenticatedUser() : base()
        {
            UserName = "Not authorized";
            BearerToken = string.Empty;
        }
        public string UserName { get; set; }
        public string BearerToken { get; set; }
        public bool IsAuthenticated {get;set;}
        public List<AuthenticatedUserClaim> Claims { get; set; }
    }
}
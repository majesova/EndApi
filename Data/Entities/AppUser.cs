using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EndApi.Data {

    public class AppUser:IdentityUser
    {
        [PersonalData]
        public string EndUserId { get; set; }
    }
}
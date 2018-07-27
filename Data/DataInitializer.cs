using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EndApi.Data
{
    public class DataInitializer
    {
        private EndContext _context;
        private RoleManager<AppRole> _roleManager;


        public DataInitializer(EndContext endContext, RoleManager<AppRole> roleManager)
        {
            _context = endContext;
            _roleManager = roleManager;
        }

        public async Task Seed(){
        string adminRoleName="Admin";
        string userRoleName="User";
           if(!_context.Roles.Any(x=>x.Name==adminRoleName)){
                AppRole adminRole =new AppRole{Name=adminRoleName};
                await _roleManager.CreateAsync(adminRole);
                await _roleManager.AddClaimAsync(adminRole,new Claim("CAN_ACCESS_USERS","true"));
                await _roleManager.AddClaimAsync(adminRole,new Claim("CAN_ADD_USER","true"));
                await _roleManager.AddClaimAsync(adminRole,new Claim("CAN_MODIFY_USER","true"));
                await _roleManager.AddClaimAsync(adminRole,new Claim("CAN_DELETE_USER","true"));
           }
           if(!_context.Roles.Any(x=>x.Name==userRoleName)){
                AppRole userRole =new AppRole{Name=userRoleName};
                await _roleManager.CreateAsync(userRole);
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_REQUEST_FOLLOW","true"));
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_REMOVE_FOLLOW","true"));
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_ACCESS_FOLOWREQUEST","true"));
           }
            

        }
        
    }
}
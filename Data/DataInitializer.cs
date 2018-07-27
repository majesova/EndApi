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

            #region Roles
        string adminRoleName="Admin";
        string userRoleName="User";
           if(!_context.Roles.Any(x=>x.Name==adminRoleName)){
                AppRole adminRole =new AppRole{Name=adminRoleName};
                await _roleManager.CreateAsync(adminRole);
                //USERS
                await _roleManager.AddClaimAsync(adminRole,new Claim("CAN_ACCESS_USERS","true"));
                await _roleManager.AddClaimAsync(adminRole,new Claim("CAN_ADD_USER","true"));
                await _roleManager.AddClaimAsync(adminRole,new Claim("CAN_MODIFY_USER","true"));
                await _roleManager.AddClaimAsync(adminRole,new Claim("CAN_DELETE_USER","true"));
           }
           if(!_context.Roles.Any(x=>x.Name==userRoleName)){
                AppRole userRole =new AppRole{Name=userRoleName};
                await _roleManager.CreateAsync(userRole);
                //FOLLOWS
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_REQUEST_FOLLOW","true"));
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_REMOVE_FOLLOW","true"));
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_ACCESS_FOLOWREQUEST","true"));
                //PLANS
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_ACCESS_PLAN","true"));
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_ADD_PLAN","true"));
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_MODIFY_PLAN","true"));
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_REMOVE_PLAN","true"));
                //REVISIONS
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_ACCESS_REVISION","true"));
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_ADD_REVISION","true"));
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_MODIFY_REVISION","true"));
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_REMOVE_REVISION","true"));
                //PROFILE
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_ACCESS_PROFILE","true"));
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_MODIFY_PROFILE","true"));
                //SEARCHING
                await _roleManager.AddClaimAsync(userRole,new Claim("CAN_SEARCH_USERS","true"));
           }
            
    #endregion

            #region FollowerPermissions
             if(!_context.FollowerPermissions.Any(x=>x.Key=="CAN_ACCESS_MYPROFILE")){
                _context.FollowerPermissions.Add(new FollowerPermission{Name="Puede ver mis revisiones",Key="CAN_ACCESS_MYPROFILE",isActive=true, Order=1});
            }
            if(!_context.FollowerPermissions.Any(x=>x.Key=="CAN_ACCESS_MYPLANS")){
                _context.FollowerPermissions.Add(new FollowerPermission{Name="Puede ver mis planes",Key="CAN_ACCESS_MYPLANS",isActive=true, Order = 2});
            }
            if(!_context.FollowerPermissions.Any(x=>x.Key=="CAN_ADDME_PLAN")){
                _context.FollowerPermissions.Add(new FollowerPermission{Name="Puede asignarme planes",Key="CAN_ADDME_PLAN",isActive=true, Order = 3});
            }
            if(!_context.FollowerPermissions.Any(x=>x.Key=="CAN_ACCESS_MYREVISIONS")){
                _context.FollowerPermissions.Add(new FollowerPermission{Name="Puede ver mis revisiones",Key="CAN_ACCESS_MYREVISIONS",isActive=true , Order = 4});
            }
            if(!_context.FollowerPermissions.Any(x=>x.Key=="CAN_ADDME_REVISION")){
                _context.FollowerPermissions.Add(new FollowerPermission{Name="Puede agregarme revisiones",Key="CAN_ADDME_REVISION",isActive=true, Order = 5});
            }
            _context.SaveChanges();
            #endregion 
        }
        
    }
}
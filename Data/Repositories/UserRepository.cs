using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EndApi.Data
{
    public class UserRepository
    {
        private readonly EndContext _context;
        private readonly UserManager<AppUser> _userManager;
        public UserRepository(EndContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public EndUser Create(EndUser user){
            _context.EndUsers.Add(user);
            return user;
        }
        public EndUser FindById(params object[] id){
            return _context.EndUsers.Find(id);
        }
        public AppUser CreateAppUser(EndUser endUser, string password){
            AppUser user = new AppUser{EndUserId = endUser.Id, UserName = endUser.Email, EmailConfirmed = true};
            _userManager.CreateAsync(user, password);
            return user;
        }

        public void AddFollow(string userOwnerId, Following following){
            var user = _context.EndUsers.Include(x=>x.Followings).FirstOrDefault(x=>x.Id==userOwnerId);
            user.Followings.Add(following);
        }
       
       public EndUser FindByEmail(string email){
           var users = _context.EndUsers.Where(x=>x.Email == email);
           if(users.Count()>0)
                return users.SingleOrDefault();
           return null;
       }
    }
}
using Microsoft.AspNetCore.Identity;

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
            var user = FindById(userOwnerId);
            user.Followings.Add(following);
        }
        public void SaveChanges(){
            _context.SaveChanges();
        }
    }
}
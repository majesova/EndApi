using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EndApi.Data;
using EndApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EndApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {   
        private UserRepository _userRepository;
        private UserManager<AppUser> _userManager;
        public UsersController(UserRepository userRepository, UserManager<AppUser> userManager)
        {
         _userRepository = userRepository;  
         _userManager = userManager; 
        }
        // POST api/values
        [HttpPost("api/users/create")]
        public async Task<IActionResult> AddUserAsync([FromBody] AddUserDto model)
        { var enduser = new EndUser();
            if(ModelState.IsValid){
                  enduser = new EndUser{
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    BirthDate =model.BirthDate,
                    Initial = model.Initial,
                    Unicode = model.Unicode,
                    Email = model.Email,
                };
                _userRepository.Create(enduser); //create user

                if(model.Follow){
                    AppUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    Following follow = new Following{
                        Id = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.Now,
                        FollowedUserId = enduser.Id,
                        FollowedById= currentUser.EndUserId};
                    _userRepository.AddFollow(currentUser.EndUserId, follow);//Add follow
                }
                
                if(model.HasAccount){
                    if(string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                    {
                     ModelState.AddModelError("Email","Email y Password son requeridos para crear la cuenta");   
                     return BadRequest(ModelState);
                    }
                    _userRepository.CreateAppUser(enduser,model.Password);//create account
                }
                _userRepository.SaveChanges();
            }
            return Ok(enduser);
        }


    }
}

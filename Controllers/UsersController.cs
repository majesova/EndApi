using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EndApi.Data;
using EndApi.Models;
using EndApi.Models.Exceptions;
using EndApi.Models.Generators;
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
        private EndContext _endContext;
        public UsersController(UserRepository userRepository, UserManager<AppUser> userManager, EndContext endContext)
        {
         _userRepository = userRepository;  
         _userManager = userManager; 
         _endContext  = endContext;
        }
        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddUserDto model)
        { 

            if(!ModelState.IsValid){
                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Hay errores de validación",ModelState ));   
            }
            using (var transaction = await _endContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var enduser = new EndUser();
                    var endUserId = Guid.NewGuid().ToString();
                    //create account
                    if(model.HasAccount) {
                        AppUser existingUser = await _userManager.FindByEmailAsync(model.Email.Trim());
                        if(existingUser!=null){
                           ModelState.AddModelError("Email","El email está ocupado por otro usuario");
                                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Validaciones",ModelState)); 
                        }
                        if(string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                            {
                                ModelState.AddModelError("Email","Email y Password son requeridos para crear la cuenta");
                                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Validaciones",ModelState)); 
                            }
                            if(model.Password.Trim().CompareTo(model.ConfirmPassword.Trim())!=0){
                                ModelState.AddModelError("Password","Password no coincide con la confirmación");
                                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Validaciones",ModelState)); 
                            }
                            var user = new AppUser {UserName = model.Email.Trim(), Email = model.Email.Trim(),EndUserId = endUserId};
                            var result = await _userManager.CreateAsync(user, model.Password.Trim());
                            _endContext.SaveChanges();//Save user temp
                             if (result.Succeeded){
                                //Add intoRole USERS
                                    await _userManager.AddToRoleAsync(user,EndConstants.RoleNameForUsers);
                                    _endContext.SaveChanges();//Add role to user temp
                                }else{
                                    transaction.Rollback();
                                    var errors = result.Errors.Select(x=>x.Description).ToList();
                                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Identity validation errors", errors));
                            }
                        }

                    //create user
                    enduser = new EndUser{
                        Id = endUserId,
                        Name = model.Name,
                        BirthDate =model.BirthDate,
                        Initial = model.Initial,
                        Unicode = UnicodeGenerator.GetUnicode(),
                        Email = model.Email }; //Can repeat email in endUsers no in accounts
                    _userRepository.Create(enduser); //create user
                    _endContext.SaveChanges();
                    //follow
                    if(model.Followup){
                        //change to request follow
                        AppUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                        Following follow = new Following{
                            Id = Guid.NewGuid().ToString(),
                            CreatedAt = DateTime.Now,
                            FollowedUserId = enduser.Id,
                            FollowedById= currentUser.EndUserId};
                        _userRepository.AddFollow(currentUser.EndUserId, follow);//Add follow
                        _endContext.SaveChanges();
                    }
                    transaction.Commit();
                    return Ok(enduser);
                    }
                catch(Exception ex){
                    transaction.Rollback();
                    return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Exception,"Exception",ex));
                }   
            }
        }




        /*
        [HttpPost]
        public async Task<IActionResult> Follow([FromBody]FollowDto model){
            if(!ModelState.IsValid){
                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Hay errores de validación",ModelState ));   
            }
            try{
                AppUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                if(_userRepository.FindById(model.UserId)==null){
                    return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Usuario no existe",new List<string>(){$"El usuario con Id {model.UserId} no existe"}));  
                }
                if(_userRepository.GetFollowed(currentUser.Id, model.UserId)!=null){
                  return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Follow existente",new List<string>(){$"El usuario con Id {model.UserId} ya es seguido por este usuario"}));  
                }
                Following follow = new Following{
                            Id = Guid.NewGuid().ToString(),
                            CreatedAt = DateTime.Now,
                            FollowedUserId = model.UserId,
                            FollowedById= currentUser.EndUserId};
                        _userRepository.AddFollow(currentUser.EndUserId, follow);//Add follow
                        _endContext.SaveChanges();
                
                return Ok(follow);
            }catch(Exception ex){
                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Exception,"Exception",ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UnFollow([FromBody]FollowDto model){
            if(!ModelState.IsValid){
                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Hay errores de validación",ModelState ));   
            }
            try{
                AppUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                var following = _userRepository.GetFollowed(currentUser.Id, model.UserId);
                if(following==null){
                  return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Follow existente",new List<string>(){$"El usuario con Id {model.UserId} no es seguido por este usuario"}));  
                }
                _userRepository.RemoveFollow(following);
                _endContext.SaveChanges();
                return Ok(following);
            }catch(Exception ex){
                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Exception,"Exception",ex));
            }
        }
 */

    }
}

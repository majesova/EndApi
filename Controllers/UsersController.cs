using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EndApi.Data;
using EndApi.Factories;
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

        private readonly FollowerPermissionRepository _followerPermissionRepository;

        private readonly FollowingRequestRepository _followingRequestRepository;
        public UsersController(UserRepository userRepository, UserManager<AppUser> userManager, 
        EndContext endContext, FollowerPermissionRepository followerPermissionRepository,
        FollowingRequestRepository followingRequestRepository)
        {
         _userRepository = userRepository;  
         _userManager = userManager; 
         _endContext  = endContext;
         _followerPermissionRepository = followerPermissionRepository;
         _followingRequestRepository = followingRequestRepository;
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
                        AppUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                        var permissionsFollowingStock = _followerPermissionRepository.GetStockPermissions();
                        var followingId = Guid.NewGuid().ToString();
                        Following follow = new Following{
                            Id = followingId,
                            CreatedAt = DateTime.Now,
                            FollowedUserId = enduser.Id,
                            FollowedById= currentUser.EndUserId,
                            GrantedPermissions = GrantedFollowerPermissionFactory.Create(permissionsFollowingStock, true, followingId)};

                        _userRepository.AddFollow(currentUser.EndUserId, follow);//Add followRequest
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


        
        [HttpPost]
        public async Task<IActionResult> RequestFollow([FromBody]FollowDto model){
            if(!ModelState.IsValid){
                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Hay errores de validación",ModelState ));   
            }
            try{
                AppUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                if(model.UserId == currentUser.EndUserId){
                    return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Usuario no existe",new List<string>(){$"Un usuario no se puede seguir a sí mismo"}));  
                }
                if(_userRepository.FindById(model.UserId)==null){
                    return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Usuario no existe",new List<string>(){$"El usuario con Id {model.UserId} no existe"}));  
                }
                if(_userRepository.GetFollowed(currentUser.EndUserId, model.UserId)!=null){
                  return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Follow existente",new List<string>(){$"El usuario con Id {model.UserId} ya es seguido por este usuario"}));  
                }
                if(_followingRequestRepository.GetFollowingRequest(currentUser.EndUserId, model.UserId)!=null){
                  return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Follow Request existente",new List<string>(){$"Ya se le ha enviad un request al usuario con Id {model.UserId}"}));  
                }
                FollowingRequest followRequest = new FollowingRequest{
                            Id = Guid.NewGuid().ToString(),
                            CreatedAt = DateTime.Now,
                            FollowedId = model.UserId,
                            RequesterId= currentUser.EndUserId,
                            Status = FollowingRequestStatus.New};
                _followingRequestRepository.Create(followRequest);
                _endContext.SaveChanges();
                
                return Ok(followRequest);
            }catch(Exception ex){
                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Exception,"Exception",ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptRequestFollow([FromBody]AcceptRequestFollowDto model){
            if(!ModelState.IsValid){
                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Hay errores de validación",ModelState ));   
            }
            if(model.Permissions==null){
                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Hay errores de validación",new List<string>(){$"No se recibieron los permisos"} ));   
            }
            try{
                AppUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                var requestFollow = _followingRequestRepository.FindById(model.FollowingRequestId);
                if(requestFollow==null){
                  return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Request no existente",new List<string>(){$"No existe el request follow"}));  
                }
                if(requestFollow.FollowedId!=currentUser.EndUserId){
                  return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"El request no le pertenece",new List<string>(){$"Este request no pertenece al usuario"}));  
                }
                requestFollow.Status = FollowingRequestStatus.Approved;
                _followingRequestRepository.Update(requestFollow);

                var followingId = Guid.NewGuid().ToString();
                        Following follow = new Following{
                            Id = followingId,
                            CreatedAt = DateTime.Now,
                            FollowedUserId = requestFollow.FollowedId,
                            FollowedById= requestFollow.RequesterId,
                            GrantedPermissions = GrantedFollowerPermissionFactory.Create(model.Permissions, followingId)};
                        _userRepository.AddFollow(currentUser.EndUserId, follow);//Add followRequest
                        _endContext.SaveChanges();

                var followResult = new AcceptResponseFollowDto();
                followResult.Id = follow.Id;
                followResult.FollowedById = follow.FollowedById;
                followResult.CreatedAt = follow.CreatedAt;
                follow.FollowedUserId = follow.FollowedUserId;
                return Ok(followResult);
            }catch(Exception ex){
                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Exception,"Exception",ex));
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetRequestsFollow(int? status=null){
            try{
                 AppUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                 var requests = _followingRequestRepository.GetRequestsByFollowedId(currentUser.EndUserId,status);
                 return Ok(requests);
            }catch(Exception ex){
                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Exception,"Exception",ex));
            }
        }

        [HttpGet("{followingId}")]
        public async Task<ActionResult> GetRequestToFollow(string followingId){
             try{
                 AppUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                 var followingRequest = _followingRequestRepository.FindById(followingId);
                 if(followingRequest == null){
                    return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"Request no existente",new List<string>(){$"No existe el request follow"}));  
                 }
                 if(followingRequest.FollowedId != currentUser.EndUserId){
                     return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Validation,"No se puede obtener",new List<string>(){$"Este request no es para la cuenta actual"}));  
                 }
                 FollowToAcceptDto result = new FollowToAcceptDto();
                 result.FollowerName = followingRequest.Requester.Name;
                 result.FollowingId = followingRequest.Id;
                 result.Permissions  = new List<PermissionFollowerToGrant>();
                 var permissions = _followerPermissionRepository.GetStockPermissions();
                 foreach(var perm in permissions){
                     var toGrant = new PermissionFollowerToGrant();
                     toGrant.Name = perm.Name;
                     toGrant.Key = perm.Key;
                     toGrant.Value = true;
                     toGrant.Order = perm.Order;
                     result.Permissions.Add(toGrant);
                 }
                 return Ok(result);
            }catch(Exception ex){
                return BadRequest(new ManagedErrorResponse(ManagedErrorCode.Exception,"Exception",ex));
            }
        }


    }
}

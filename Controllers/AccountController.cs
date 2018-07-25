using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EndApi.Data;
using EndApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using EndApi.Models.Security;
using System.Transactions;

namespace EndApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly UserRepository _userRepository;

         private JwtSettings _jwtSettings;
         private readonly EndContext _endContext;
        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            UserRepository userRepository,
            JwtSettings jwtSettings,
            RoleManager<AppRole> roleManager,
            EndContext endContext
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration =     configuration;
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
            _roleManager = roleManager;
            _endContext = endContext;
        }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {   
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        if(string.Compare(model.Password.Trim(),model.ConfirmPassword.Trim())!=0){
            ModelState.AddModelError("Password","El password y su confirmación no coinciden");
            return BadRequest(ModelState);
        }
        using (var transaction = await _endContext.Database.BeginTransactionAsync())
        {
            try{
                var endUserId = Guid.NewGuid().ToString();
                var user = new AppUser {UserName = model.Email.Trim(), Email = model.Email.Trim(),EndUserId = endUserId};
                _userRepository.Create(new EndUser {Id=user.EndUserId, Email = model.Email.Trim(), Name = model.Name.Trim()});
                var result = await _userManager.CreateAsync(user, model.Password.Trim());
                SecurityManager mgr = new SecurityManager(_jwtSettings, _userManager, _roleManager);
                if (result.Succeeded){
                    _userRepository.SaveChanges();
                    transaction.Commit();
                    await _signInManager.SignInAsync(user, false);
                    var authUser = await mgr.BuildAuthenticatedUserObject(user);
                    return Ok(authUser);
                }else{
                    var errors = result.Errors.Select(x=>x.Description);
                    return BadRequest(errors);
                }
                }catch(Exception ex){
                    transaction.Rollback();
                    return BadRequest(ex);
                }
        }
       
            
        

    }

        [HttpPost]
    public async Task<ActionResult> Login([FromBody] LoginDto model)
    {
        SecurityManager mgr = new SecurityManager(_jwtSettings, _userManager, _roleManager);
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        
        if (result.Succeeded)
        {
            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
            var authUser = await mgr.BuildAuthenticatedUserObject(appUser);
            return Ok(authUser);
        }else{
            return BadRequest();
        }
        
        throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
    }

    /*private string GenerateJwtToken(string email, AppUser user)
        {
            //Standard claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            //Custom claims
            claims.Add(new Claim("endUserId",user.EndUserId));
            claims.Add(new Claim("isAuthenticated", "true"));

            //Creating jwt token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.DaysToExpiration),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/

        
    }
}

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

namespace EndApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly UserRepository _userRepository;
        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            UserRepository userRepository
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration =     configuration;
            _userRepository = userRepository;
        }

    [HttpPost]
    public async Task<ActionResult> Register([FromBody] RegisterDto model)
    {
        var endUserId = Guid.NewGuid().ToString();
        var user = new AppUser
        {
            UserName = model.Email, 
            Email = model.Email,
            EndUserId = endUserId
        };
        
        
        _userRepository.Create(new EndUser{Id=user.EndUserId,Email =model.Email,Name = model.Email });
        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (result.Succeeded){
            _userRepository.SaveChanges();
            await _signInManager.SignInAsync(user, false);
            var token= await GenerateJwtToken(model.Email, user);
            return Ok(token);
        }else{
            var errors = result.Errors.Select(x=>x.Description);
            return BadRequest(errors);
        }
        
        throw new ApplicationException("UNKNOWN_ERROR");
    }

        [HttpPost]
    public async Task<ActionResult> Login([FromBody] LoginDto model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        
        if (result.Succeeded)
        {
            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
            var token = await GenerateJwtToken(model.Email, appUser);
            return Ok(token);
        }else{
            return BadRequest();
        }
        
        throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
    }



    private async Task<object> GenerateJwtToken(string email, AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("endUserId",user.EndUserId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}

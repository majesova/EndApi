using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EndApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace EndApi.Models.Security{
    public class SecurityManager
    {
        private JwtSettings _jwtSettings;
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        public SecurityManager(JwtSettings jwtSettings, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AuthenticatedUser> BuildAuthenticatedUserObject(AppUser appUser){
            AuthenticatedUser authUser = new AuthenticatedUser();
            List<AuthenticatedUserClaim> claims = new List<AuthenticatedUserClaim>();
            authUser.UserName = appUser.UserName;
            authUser.IsAuthenticated = true;
            authUser.Claims = await GetClaimsOfUser(appUser);
            authUser.BearerToken =  BuildJwtToken(appUser, authUser.Claims);      
            return authUser;
        }

        protected  string BuildJwtToken(AppUser appUser, List<AuthenticatedUserClaim> customClaims){

            //Standard claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id)
            };
            //Custom claims
            if(customClaims != null){
                foreach (var customClaim in customClaims)
                    {
                        claims.Add(new Claim(customClaim.ClaimType, customClaim.ClaimValue));
                    }
                }
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
        }

        protected async Task<List<AuthenticatedUserClaim>> GetClaimsOfUser(AppUser appUser){
            
            var roles = await _userManager.GetRolesAsync(appUser);
            var claimsAuth = new List<AuthenticatedUserClaim>();
            foreach (var rol in roles)
            {
                var userRole = await _roleManager.FindByNameAsync(rol);
                var claimsOfRole = await _roleManager.GetClaimsAsync(userRole);
                foreach(var claim in claimsOfRole){
                    claimsAuth.Add(new AuthenticatedUserClaim{ClaimType = claim.Type, ClaimValue = claim.Value });
                }
            }
            //Personal claims
            var claims = await _userManager.GetClaimsAsync(appUser);
            foreach(var claim in claims) {
                claimsAuth.Add(new AuthenticatedUserClaim{ClaimType = claim.Type, ClaimValue = claim.Value });
            }
            return claimsAuth;
        }
        

    }
}
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityDTos;

namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager ,
                                       IConfiguration _configuration) : IAuthenticationService
    {
        public async Task<UserDTo> LoginAsync(LoginDTo loginDTo)
        {
            // Check If Email Is Exists
            var User = await _userManager.FindByEmailAsync(loginDTo.Email);
            if (User is null)
                throw new UserNotFoundException(loginDTo.Email);
            // Check Password
            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, loginDTo.Password);
            if(IsPasswordValid)   // Return UserDto
                return new UserDTo()
                {
                    DisplayName = User.DisplayName,
                    Email = User.Email,
                    Token = await CreateTokenAsync(User)
                };
            else
                throw new UnauthorizedException();
        }

        public async Task<UserDTo> RegisterAsync(RegisterDTo registerDTo)
        {
            // Mapping Register Dto => Application User
            var User = new ApplicationUser()
            {
                DisplayName = registerDTo.DisplayName,
                Email = registerDTo.Email,
                PhoneNumber = registerDTo.PhoneNumber,
                UserName = registerDTo.UserName
            };

            // Create User [Application User]
            var Result = await _userManager.CreateAsync(User, registerDTo.Password);
            if (Result.Succeeded)
            {
                // Return UserDto
                return new UserDTo()
                {
                    DisplayName = User.DisplayName,
                    Email = User.Email,
                    Token = await CreateTokenAsync(User)
                };
            }
            else
            {
                var Errors = Result.Errors.Select(E => E.Description).ToList();
                // Throw BadRequest Exceptions
                throw new BadRequestException(Errors);
            }
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var Claims = new List<Claim>()
            {
                new (ClaimTypes.Email, user.Email!),
                new (ClaimTypes.Name, user.UserName!),
                new (ClaimTypes.NameIdentifier, user.Id!),
            };
            var Roles = await _userManager.GetRolesAsync(user);

            foreach (var role in Roles)
                Claims.Add(new Claim(ClaimTypes.Role, role));

            var SecretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Creds = new SigningCredentials(Key , SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken(
                issuer : _configuration["JWTOptions:Issuer"],
                audience : _configuration["JWTOptions:Audience"],
                claims : Claims,
                expires : DateTime.Now.AddHours(2),
                signingCredentials : Creds);

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}

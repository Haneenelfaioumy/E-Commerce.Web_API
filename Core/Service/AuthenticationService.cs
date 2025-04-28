using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityDTos;

namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager) : IAuthenticationService
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
                    Token = CreateTokenAsync(User)
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
                    Token = CreateTokenAsync(User)
                };
            }
            else
            {
                var Errors = Result.Errors.Select(E => E.Description).ToList();
                // Throw BadRequest Exceptions
                throw new BadRequestException(Errors);
            }
        }

        private static string CreateTokenAsync(ApplicationUser user)
        {
            return "TOKEN - TODO";
        }
    }
}

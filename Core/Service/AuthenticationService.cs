using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityDTos;

namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager ,
                                       IConfiguration _configuration ,
                                       IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string Email)
        {
            var User = await _userManager.FindByEmailAsync(Email);
            return User is not null;
        }

        public async Task<UserDTo> GetCurrentUserAsync(string Email)
        {
            var User = await _userManager.FindByEmailAsync(Email) ?? throw new UserNotFoundException(Email);
            return new UserDTo()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await CreateTokenAsync(User)
            };
        }

        public async Task<AddressDTo> GetCurrentUserAddressAsync(string Email)
        {
            var User = await _userManager.Users
                      .Include(U => U.Address)
                      .FirstOrDefaultAsync(U => U.Email == Email) ?? throw new UserNotFoundException(Email);
            if (User.Address is not null)
                return _mapper.Map<Address, AddressDTo>(User.Address);
            else
                throw new AddressNotFoundException(User.UserName);
        }

        public async Task<AddressDTo> UpdateCurrentUserAddressAsync(string Email, AddressDTo addressDTo)
        {
            var User = await _userManager.Users
                      .Include(U => U.Address)
                      .FirstOrDefaultAsync(U => U.Email == Email) ?? throw new UserNotFoundException(Email);
            if(User.Address is not null) // Update 
            {
                User.Address.FirstName = addressDTo.FirstName;
                User.Address.LastName = addressDTo.LastName;
                User.Address.Street = addressDTo.Street;
                User.Address.City = addressDTo.City;
                User.Address.Country = addressDTo.Country;
            }
            else // Add New User
            {
                User.Address = _mapper.Map<AddressDTo, Address>(addressDTo);
            }
            await _userManager.UpdateAsync(User);
            return _mapper.Map<AddressDTo>(User.Address);
        }

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

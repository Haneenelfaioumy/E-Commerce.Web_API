using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTransferObjects.IdentityDTos;

namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {
        // Login
        // Take Email and Password Then Return Token ,  Email and DisplayName
        Task<UserDTo> LoginAsync(LoginDTo loginDTo);

        // Regiser
        // Take Email , Password  , UserName , Display Name And Phone Number
        // Then Return Token , Email and Display Name
        Task<UserDTo> RegisterAsync(RegisterDTo registerDTo);

        // Check Email
        // Take Email Then Return boolean 
        Task<bool> CheckEmailAsync(string Email);

        // Get Current User Address 
        // Take Email Then Return Address of Current Logged in User
        Task<AddressDTo> GetCurrentUserAddressAsync(string Email);

        // Update Current User Address 
        // Take Updated Address and Email Then Return Address after Update
        Task<AddressDTo> UpdateCurrentUserAddressAsync(string Email , AddressDTo addressDTo);

        // Get Current User 
        // Take Email Then Return Token , Email and Display Name
        Task<UserDTo> GetCurrentUserAsync(string Email);
    }
}

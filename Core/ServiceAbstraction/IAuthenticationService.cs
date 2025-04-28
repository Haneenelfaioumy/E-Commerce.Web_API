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
    }
}

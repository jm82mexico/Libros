using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tienda.Application.Models.Identity;

namespace Tienda.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<AuthResponse> Login (AuthRequest request);
        Task<RegistrationResponse> Register (RegistrationRequest request);
    }
}
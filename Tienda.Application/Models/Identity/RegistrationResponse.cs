using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda.Application.Models.Identity
{
    public class RegistrationResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email  { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
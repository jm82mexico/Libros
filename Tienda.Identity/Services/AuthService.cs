using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tienda.Application.Constants;
using Tienda.Application.Contracts.Identity;
using Tienda.Application.Models.Identity;
using Tienda.Identity.Model;

namespace Tienda.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser>? _userManager;
        private readonly SignInManager<ApplicationUser>? _signInManager;
        private readonly JwtSettings? _jwtSettings;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            var user = _userManager!.Users.SingleOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                throw new Exception($"El usuario con el correo {request.Email} no existe");
            }

            var result = await _signInManager!.PasswordSignInAsync(user, request.Password, false, false);

            if (!result.Succeeded)
            {
                throw new Exception("Credenciales incorrectas");
            }

            var token = await GenerateToken(user);
            var authResponse = new AuthResponse
            {
                Email = user.Email,
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = user.UserName,
            };

            return authResponse;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var existingUser = _userManager!.Users.SingleOrDefault(u => u.UserName == request.Username);
            if (existingUser != null)
            {
                throw new Exception($"El usuario  {request.Username} ya existe");
            }

            var existingEmail = _userManager!.Users.SingleOrDefault(u => u.Email == request.Email);
            if (existingEmail != null)
            {
                throw new Exception($"El correo {request.Email} ya esta en uso");
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Username,
                Nombre = request.Nombre,
                Apellidos = request.Apellidos,
                EmailConfirmed = true
            };

            var result = await _userManager!.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new Exception($"No se pudo crear el usuario {request.Username}");
            }
            await _userManager!.AddToRoleAsync(user, "USER");    
            var token = await GenerateToken(user);

            var registrationResponse = new RegistrationResponse
            {
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.Id,
                Username = user.UserName,
            };

            return registrationResponse;
            

        }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager!.GetClaimsAsync(user);
            var roles = await _userManager!.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomClaimTypes.uid, user.Id),
            }.Union(userClaims).Union(roleClaims);

            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings!.Key));
            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            
            return jwtSecurityToken;
        }

    }
}



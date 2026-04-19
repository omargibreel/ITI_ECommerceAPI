using AutoMapper;
using Common.GeneralResuls;
using Common.Result;
using Common.Settings;
using Ecommerce.BLL.DTOs.Auth;
using Ecommerce.BLL.Services.Interfaces;
using Ecommerce.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services.Classes
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtSetting _jwtSettings;

        public AuthService(UserManager<AppUser> userManager, IMapper mapper, IOptions<JwtSetting> jwtOptions)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<Result<AuthDTO>> RegisterAsync(RegisterDTO dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return Result<AuthDTO>.Fail(Error.Conflict("Email already registered."));

            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
                return Result<AuthDTO>.Fail(errors);
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!addToRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                var roleErrors = addToRoleResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
                return Result<AuthDTO>.Fail(roleErrors);
            }

            return Result<AuthDTO>.Success(await GenerateTokenAsync(user));
        }

        public async Task<Result<AuthDTO>> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return Result<AuthDTO>.Fail(Error.Validation("Invalid email or password."));
            }
                
            return Result<AuthDTO>.Success(await GenerateTokenAsync(user));
        }

        private async Task<AuthDTO> GenerateTokenAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryInDays);
            
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiry,
                signingCredentials: creds);
                
            return new AuthDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email!,
                FullName = $"{user.FirstName} {user.LastName}",
                Expiry = expiry
            };
        }
    }
}
using Mehrzad.SmartCampus.Backend.API.Infrastructure;
using Mehrzad.SmartCampus.Backend.Core.Exceptions;
using Mehrzad.SmartCampus.Backend.Infrastructure.Database;
using Mehrzad.SmartCampus.Backend.Security.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;

namespace Api.Admin.Handlers
{
    public class LoginHandler
    {
        private readonly SmartCampusDB _db;
        private readonly IPasswordService _passwordService;
        private readonly IJwtTokenGenerator _jwtGenerator;

        public LoginHandler(SmartCampusDB db, IPasswordService passwordService, IJwtTokenGenerator jwtGenerator)
        {
            _db = db;
            _passwordService = passwordService;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<(bool RequiresMfa, LoginResponseDto? Response)> HandleLoginAsync(LoginDto dto)
        {

            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(dto.Email))
                errors["Email"] = new[] { "Email is required." };
            else if (!Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errors["Email"] = new[] { "Invalid email format." };

            if (string.IsNullOrWhiteSpace(dto.Password))
                errors["Password"] = new[] { "Password is required." };

            if (errors.Any())
                throw new ValidationException(errors);


            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user is null || !_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
                throw new AuthenticationException("No user found");

            if (user.Role.ToString() == "Admin")
            {
                var otp = new Random().Next(100000, 999999).ToString();
                user.PendingOtp = otp;
                user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
                await _db.SaveChangesAsync();

                // TODO: Send OTP via email/SMS
                Console.WriteLine($"[DEV MFA] OTP for {user.Email}: {otp}");
                // For now we just log the OTP
                return (true, null);
            }

            var token = _jwtGenerator.GenerateToken(user);
            return (false, new LoginResponseDto
            {
                Token = token,
                Role = user.Role.ToString(),
                RequiresMfa = false
            });
        }

        public async Task<LoginResponseDto?> VerifyMfaAsync(MfaDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user is null || user.PendingOtp != dto.Otp || user.OtpExpiry < DateTime.UtcNow)
                return null;

            user.PendingOtp = null;
            user.OtpExpiry = null;
            await _db.SaveChangesAsync();
            var token = _jwtGenerator.GenerateToken(user);
            return new LoginResponseDto
            {
                RequiresMfa = false,
                Token = token,
                Role = user.Role.ToString()
            };
        }
    }
}
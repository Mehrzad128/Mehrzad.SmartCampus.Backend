using Mehrzad.SmartCampus.Backend.API.Infrastructure;
using Mehrzad.SmartCampus.Backend.Infrastructure.Database;
using Mehrzad.SmartCampus.Backend.Security.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System;

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

        public async Task<(bool RequiresMfa, string? Token)> HandleLoginAsync(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user is null || !_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
                return (false, null);

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
            return (false, token);
        }

        public async Task<string?> VerifyMfaAsync(MfaDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user is null || user.PendingOtp != dto.Otp || user.OtpExpiry < DateTime.UtcNow)
                return null;

            user.PendingOtp = null;
            user.OtpExpiry = null;
            await _db.SaveChangesAsync();

            return _jwtGenerator.GenerateToken(user);
        }
    }
}
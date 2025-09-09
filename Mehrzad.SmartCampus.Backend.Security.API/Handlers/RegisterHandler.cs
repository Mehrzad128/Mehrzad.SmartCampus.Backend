using Mehrzad.SmartCampus.Backend.API.Infrastructure;
using Mehrzad.SmartCampus.Backend.Core.Entities;
using Mehrzad.SmartCampus.Backend.Infrastructure.Database;
using Mehrzad.SmartCampus.Backend.Security.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System;

namespace Mehrzad.SmartCampus.Backend.Security.API.Handlers
{
    public class RegisterHandler
    {
        private readonly SmartCampusDB _db;
        private readonly IPasswordService _passwordService;
        private readonly IJwtTokenGenerator _jwtGenerator;
        private readonly string _adminInviteCode;

        public RegisterHandler(
            SmartCampusDB db,
            IPasswordService passwordService,
            IJwtTokenGenerator jwtGenerator,
            IConfiguration config)
        {
            _db = db;
            _passwordService = passwordService;
            _jwtGenerator = jwtGenerator;
            _adminInviteCode = config["Auth:AdminInviteCode"] ?? "";
        }

        public async Task<string?> HandleAsync(RegisterDto dto, CancellationToken ct = default)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            // 1. Reject if email already exists
            if (await _db.Users.AnyAsync(u => u.Email == email, ct))
                return null;

            // 2. Validate role
            var allowedRoles = new[] { "Student", "Faculty", "Admin" };
            if (!allowedRoles.Contains(dto.Role.ToString(), StringComparer.OrdinalIgnoreCase))
                return null;

            // 3. If Admin, check invite code
            if (dto.Role.ToString() == "Admin" && dto.InviteCode != _adminInviteCode)
                return null;

            // 4. Create User entity
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = email,
                Name = dto.Name,
                PasswordHash = _passwordService.HashPassword(dto.Password),
                Role = dto.Role
            };
            _db.Users.Add(user);

            // 5. Student profile
            if (dto.Role.ToString() == "Student")
            {
                if (string.IsNullOrWhiteSpace(dto.StudentId))
                    return null;

                _db.Students.Add(new Student
                {
                    UserId = user.UserId,
                    StudentId = dto.StudentId
                });
            }

            // 6. Faculty profile
            else if (dto.Role.ToString() == "Faculty")
            {
                if (string.IsNullOrWhiteSpace(dto.FacultyId) || string.IsNullOrWhiteSpace(dto.Department))
                    return null;

                _db.Faculties.Add(new Faculty
                {
                    UserId = user.UserId,
                    FacultyId = dto.FacultyId,
                    Department = dto.Department
                });
            }


            await _db.SaveChangesAsync(ct);

            // 7. Issue JWT
            return _jwtGenerator.GenerateToken(user);
        }
    }
}

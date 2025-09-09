using Mehrzad.SmartCampus.Backend.API.Infrastructure;
using Mehrzad.SmartCampus.Backend.Core.Entities;
using Mehrzad.SmartCampus.Backend.Infrastructure.Database;
using Mehrzad.SmartCampus.Backend.Students.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace Mehrzad.SmartCampus.Backend.Students.API.Handlers
{
    public class StudentCrudHandler
    {
        private readonly SmartCampusDB _db;
        private readonly IPasswordService _pw;

        public StudentCrudHandler(SmartCampusDB db, IPasswordService pw)
        {
            _db = db;
            _pw = pw;
        }

        // LIST
        public async Task<List<ReadStudentDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Students
                .Include(s => s.User)
                .Select(s => new ReadStudentDto(
                s.StudentId,
                s.User.UserId,
                s.User.Name,
                s.User.Email,
                s.EnrollmentDate))
                .ToListAsync(ct);
        }

        // CREATE
        public async Task<Guid> CreateAsync(CreateStudentDto dto, CancellationToken ct = default)
        {
            var email = dto.Email.Trim().ToLowerInvariant();
            if (await _db.Users.AnyAsync(u => u.Email == email, ct))
                throw new InvalidOperationException("Email already in use");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = email,
                PasswordHash = _pw.HashPassword(dto.Password),
                Name = dto.Name,
                Role = 0
            };
            _db.Users.Add(user);

            _db.Students.Add(new Student
            {
                UserId = user.UserId,
                StudentId = dto.StudentId
            });

            await _db.SaveChangesAsync(ct);
            return user.UserId;
        }

        // READ (role-aware)
        public async Task<Student?> GetAsync(Guid id, ClaimsPrincipal currentUser, CancellationToken ct = default)
        {
            var role = currentUser.FindFirstValue(ClaimTypes.Role);
            var currentUserId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

            // Students can only view themselves
            if (role == "Student" && currentUserId != id.ToString())
                return null;

            return await _db.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == id, ct);
        }

        // UPDATE
        public async Task<bool> UpdateAsync(Guid id, UpdateStudentDto dto, CancellationToken ct = default)
        {
            var student = await _db.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == id, ct);

            if (student is null) return false;

            student.StudentId = dto.StudentId;
            student.User.Email = dto.Email.Trim().ToLowerInvariant();

            if (!string.IsNullOrWhiteSpace(dto.Password))
                student.User.PasswordHash = _pw.HashPassword(dto.Password);

            await _db.SaveChangesAsync(ct);
            return true;
        }

        // DELETE
        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var student = await _db.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == id, ct);

            if (student is null) return false;

            _db.Users.Remove(student.User); // cascade deletes Student
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }

}

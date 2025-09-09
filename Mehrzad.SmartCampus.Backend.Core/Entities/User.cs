using Mehrzad.SmartCampus.Backend.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class User 
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public required string Name { get; set; } 
        public required string Email { get; set; } 
        public required string PasswordHash { get; set; }
        public UserRole Role { get; set; }

        // Navigation
        public Student? StudentProfile { get; set; }
        public Faculty? FacultyProfile { get; set; }

        // MFA fields (for now storing in DB)
        public string? PendingOtp { get; set; }
        public DateTime? OtpExpiry { get; set; }

    }
}

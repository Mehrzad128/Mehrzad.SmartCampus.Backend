using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Student
    {
        public required string StudentId { get; set; } 
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        // Foreign Key
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Navigation
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }

}


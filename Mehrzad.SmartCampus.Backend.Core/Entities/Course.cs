using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Course
    {
        public Guid CourseId { get; set; } = Guid.NewGuid();
        public required string Title { get; set; } 
        public string? Description { get; set; }
        public int Credits { get; set; }

        // Foreign Key
        public required string FacultyId { get; set; }
        public required Faculty Faculty { get; set; } 

        // Navigation
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}

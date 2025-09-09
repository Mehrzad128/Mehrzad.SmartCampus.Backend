using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Enrollment
    {
        public Guid EnrollmentId { get; set; } = Guid.NewGuid();
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        // Foreign Keys
        public required string StudentId { get; set; }
        public required Student Student { get; set; }

        public Guid CourseId { get; set; }
        public required Course Course { get; set; } 
    }
}

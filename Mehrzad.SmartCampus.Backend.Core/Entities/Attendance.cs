using Mehrzad.SmartCampus.Backend.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Attendance
    {
        public Guid AttendanceId { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; }
        public string Status { get; set; } = null!; // Present, Absent, Late

        // Foreign Keys
        public required string StudentId { get; set; }
        public required Student Student { get; set; } 

        public required Guid CourseId { get; set; }
        public required Course Course { get; set; } 
    }
}

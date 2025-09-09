using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Faculty
    {
        public required string FacultyId { get; set; } 
        public required string Department { get; set; } 

        // Foreign Key
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Navigation
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Event> EventsOrganized { get; set; } = new List<Event>();
    }
}

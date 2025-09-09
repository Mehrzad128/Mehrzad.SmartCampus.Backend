using Mehrzad.SmartCampus.Backend.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Booking
    {
        public Guid BookingId { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; }
        public required string TimeSlot { get; set; } 
        public required Status Status { get; set; } // Pending, Approved, Rejected

        // Foreign Keys
        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public required string FacultyId { get; set; }
        public Faculty Faculty { get; set; } = null!;
    }
}

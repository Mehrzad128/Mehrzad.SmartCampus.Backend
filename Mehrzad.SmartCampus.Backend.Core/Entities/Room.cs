using Mehrzad.SmartCampus.Backend.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Room
    {
        public Guid RoomId { get; set; } = Guid.NewGuid();
        public required string Name { get; set; } 
        public int Capacity { get; set; }
        public RoomType Type { get; set; }  // Classroom, Lab, Auditorium

        // Navigation
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

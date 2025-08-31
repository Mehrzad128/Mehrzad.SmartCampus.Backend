using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Booking : Entity
    {
        public required Room Room { get; set; }
        public required Faculty Faculty { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        //Change it to enum array of available time slots
        public required string TimeSlot { get; set; }
    }
}

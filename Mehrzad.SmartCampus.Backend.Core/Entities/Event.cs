using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Event
    {
        public Guid EventId { get; set; }
        public required string Title { get; set; } 
        public string? Description { get; set; } 
        public DateTime Date { get; set; }

        // Foreign Key
        public Guid OrganizerId { get; set; }
        public required Faculty Organizer { get; set; } 
    }
}

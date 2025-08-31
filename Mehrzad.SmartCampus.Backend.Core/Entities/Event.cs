using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Event : Entity
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public required Faculty Organizer { get; set; }
    }
}

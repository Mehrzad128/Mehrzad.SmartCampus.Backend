using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Course : Entity
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Credits { get; set; }
        public required Faculty Faculty { get; set; }
    }
}

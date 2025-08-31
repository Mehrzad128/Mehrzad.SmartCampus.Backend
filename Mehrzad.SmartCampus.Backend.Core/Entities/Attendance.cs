using Mehrzad.SmartCampus.Backend.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Attendance : Entity
    {
        public required Student Student { get; set; }
        public required Course Course { get; set; }
        public required DateTime Date { get; set; }
        public Status Status { get; set; }
    }
}

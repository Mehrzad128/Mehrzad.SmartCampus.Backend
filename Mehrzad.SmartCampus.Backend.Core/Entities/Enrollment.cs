using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Enrollment : Entity
    {
        public required Student Student { get; set; }
        public required Course Course { get; set; }
        public DateTime EnrolmentDate { get; set; } = DateTime.Now ;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Student : Entity
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public DateTime InrolmentDate { get; set; } = DateTime.Now;
    }
}

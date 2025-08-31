using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.Core.Entities
{
    public class Admin : Entity
    {
        public required string Password { get; set; }
        public string? Name { get; set; }
        public required string Email { get; set; }
    }
}

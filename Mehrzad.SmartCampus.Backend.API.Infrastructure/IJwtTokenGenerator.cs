using Mehrzad.SmartCampus.Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.API.Infrastructure
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}

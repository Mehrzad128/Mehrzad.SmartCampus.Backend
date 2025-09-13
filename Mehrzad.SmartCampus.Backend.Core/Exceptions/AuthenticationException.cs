using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mehrzad.SmartCampus.Backend.Core.Exceptions
{
    public class AuthenticationException : AppException
    {
        public AuthenticationException(string message)
            : base(message, 401) { }
    }
}

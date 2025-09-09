using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrzad.SmartCampus.Backend.API.Infrastructure
{
    public class MicrosoftPasswordService : IPasswordService
    {
        private readonly PasswordHasher<object> _hasher;

        public MicrosoftPasswordService()
        {
            _hasher = new PasswordHasher<object>();
        }

        public string HashPassword(string password)
        {
            // We pass a dummy user object because PasswordHasher<T> requires it
            return _hasher.HashPassword(new object(), password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var result = _hasher.VerifyHashedPassword(new object(), hashedPassword, password);
            return result == PasswordVerificationResult.Success ||
                   result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}

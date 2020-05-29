using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw5.Models;

namespace Cw5.Services
{
    public interface IJWTAuthorizationService
    {
        public JWTResponse Login(LoginRequest request);
        public JWTResponse RefreshToken(RefreshRequest refresh);
    }
}

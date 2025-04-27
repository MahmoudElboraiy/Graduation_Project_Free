using Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityManager.Services.JwtAuthentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user, string role, bool longExpires = false);
    }
}

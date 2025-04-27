using ErrorOr;
using IdentityManager.Services.Authentication.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityManager.Services.Authentication.Queries
{
    public record UserLoginQuery(string Email, string Password)
     : IRequest<ErrorOr<AuthenticationResponse>>;
}

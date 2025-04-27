using ErrorOr;
using IdentityManager.Services.Authentication.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityManager.Services.Authentication.Commands
{
    public record UserRegisterCommand
    (
        string FullName,
        string Email,
        string Password,
        string ConfirmPassword
    ):IRequest<ErrorOr<AuthenticationResponse>>;
}

using Domain.DErrors;
using Domain.Domain;
using ErrorOr;
using IdentityManager.Services.Authentication.Common;
using IdentityManager.Services.JwtAuthentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityManager.Services.Authentication.Queries
{
    public class UserLoginQueryHandler : IRequestHandler<UserLoginQuery, ErrorOr<AuthenticationResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator JwtTokenGenerator;

        public UserLoginQueryHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator
        )
        {
            _userManager = userManager;
            JwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<ErrorOr<AuthenticationResponse>> Handle(UserLoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(
             u => u.Email == request.Email,
             cancellationToken: cancellationToken
         );
            if (user == null)
            {
                return DomainErrors.Authentication.InvalidCredentials();
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                return DomainErrors.Authentication.InvalidCredentials();
            }

            var role = await _userManager.GetRolesAsync(user);

            var jwtToken = JwtTokenGenerator.GenerateToken(user, role.First());

            return new AuthenticationResponse(jwtToken, role.First());
        }
    }
}

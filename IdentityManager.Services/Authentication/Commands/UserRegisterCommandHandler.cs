
using Domain.DErrors;
using Domain.Domain;
using Domain.Enums;
using ErrorOr;
using IdentityManager.Services.Authentication.Common;
using IdentityManager.Services.JwtAuthentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityManager.Services.Authentication.Commands
{
    public class UserRegisterCommandHandler
         : IRequestHandler<UserRegisterCommand, ErrorOr<AuthenticationResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator JwtTokenGenerator;
        public UserRegisterCommandHandler(UserManager<ApplicationUser> userManager,
                                         IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            JwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<AuthenticationResponse>> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
               FullName =request.FullName,
               Email = request.Email,
               UserName =request.Email
            };
            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists!=null)
            {
                return DomainErrors.Authentication.DuplicateEmail(user.Email);
            }
            var result = await _userManager.CreateAsync(user, request.Password);


            if (!result.Succeeded)
            {
                return DomainErrors.Authentication.InvalidCredentials();
            }
            await _userManager.AddToRoleAsync(user, Roles.User.ToString());

            var jwtToken = JwtTokenGenerator.GenerateToken(user, Roles.User.ToString());

            return new AuthenticationResponse(jwtToken, Roles.User.ToString());
        }
    }
}

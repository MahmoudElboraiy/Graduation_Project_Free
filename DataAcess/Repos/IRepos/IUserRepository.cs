﻿using Domain.Domain;
using Domain.DTOs.Auth;
using Domain.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Repos.IRepos
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<bool> IsUniqueUserName(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO);
        Task<ApplicationUser> GetUserByID(string userID);
        Task<bool> UpdateAsync(ApplicationUser user);

    }
}

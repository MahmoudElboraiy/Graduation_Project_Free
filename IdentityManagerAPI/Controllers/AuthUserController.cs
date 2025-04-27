using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DataAcess.Repos.IRepos;
using IdentityManager.Services.ControllerService.IControllerService;
using IdentityManager.Services.Authentication.Commands;
using MediatR;
using IdentityManager.Services.Authentication.Queries;
using Domain.DTOs.Auth;


namespace IdentityManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISender _mediator;
        public AuthUserController(IAuthService authService, ISender mediator)
        {
            _authService = authService;
            _mediator = mediator;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var result = await _authService.LoginAsync(loginRequestDTO);
            return Ok(result);
        }
        [HttpPost("login2")]
        public async Task<IActionResult> Login([FromBody] UserLoginQuery query)
        {
            var result = await _mediator.Send(query);
            return result.Match<IActionResult>(Ok, BadRequest);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var result = await _authService.RegisterAsync(registerRequestDTO);
            return Ok(result);
        }
        [HttpPost("register2")]
        public async Task<IActionResult> Register([FromBody] UserRegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Match<IActionResult>(Ok, BadRequest);
        }
    }
}

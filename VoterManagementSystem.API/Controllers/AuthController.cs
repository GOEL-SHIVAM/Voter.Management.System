using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VoterManagementSystem.Application.DTOs.Auth;
using VoterManagementSystem.Application.Interfaces;

namespace VoterManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(
        IAuthService authService,
        IValidator<LoginRequestDto> loginValidator,
        IValidator<VoterRegisterDto> registerValidator) : BaseApiController
    {
        private readonly IAuthService _authService = authService;
        private readonly IValidator<LoginRequestDto> _loginValidator = loginValidator;
        private readonly IValidator<VoterRegisterDto> _registerValidator = registerValidator;

        [HttpPost("admin-login")]
        public async Task<ActionResult<LoginResponseDto>> AdminLogin([FromBody] LoginRequestDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _loginValidator,
                () => _authService.AdminLoginAsync(request)
            );
        }

        [HttpPost("voter-login")]
        public async Task<ActionResult<LoginResponseDto>> VoterLogin([FromBody] LoginRequestDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _loginValidator,
                () => _authService.VoterLoginAsync(request)
            );
        }

        [HttpPost("voter-register")]
        public async Task<ActionResult<LoginResponseDto>> VoterRegister([FromBody] VoterRegisterDto request)
        {
            return await ValidateAndExecuteAsync(
                request,
                _registerValidator,
                () => _authService.VoterRegisterAsync(request)
            );
        }
    }
}
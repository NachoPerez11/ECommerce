using ECommerce.Api.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;


namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;


        public AuthController(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if(existingUser != null)
            {
                return BadRequest(new{ message = "Usuario ya registrado." });
            }
                
            var passHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User(request.Email,request.Name, passHash);
            user.Role = request.Role;
            await _userRepository.AddAsync(user);
            return Ok(new{ message = "Usuario registrado exitosamente." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if(user == null)
            {
                return Unauthorized(new{ message = "Credenciales inválidas." });
            }

            var validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if(!validPassword)
            {
                return Unauthorized(new{ message = "Credenciales inválidas." });
            }
            var token = _tokenService.GenerateToken(user.Id, user.Email, user.Role);
            return Ok(new { token, role=user.Role, email=user.Email });
        }
    }
}
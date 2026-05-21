using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Api.DTOs;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound(new { Message = "Usuario no encontrado." });
        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        // Validar si el email ya existe
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null) return BadRequest(new { Message = "El email ya está registrado." });

        // En un caso real acá se encripta la contraseña, acá simulamos el hash simple
        var passwordHash = "HASH_" + dto.Password; 

        var user = new User(dto.Email, dto.Name, passwordHash);
        await _userRepository.AddAsync(user);

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }
}
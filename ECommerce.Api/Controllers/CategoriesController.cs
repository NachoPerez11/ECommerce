using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Api.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var response = categories.Select(c => new CategoryResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            CreatedAt = c.CreatedAt
        });
        return Ok(response);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return NotFound(new { Message = "Categoría no encontrada." });

        var response = new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        };
        return Ok(response);
    }

    [Authorize(Roles="Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        try
        {
            var category = new Category(dto.Name, dto.Description);
            
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            var response = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [Authorize(Roles="Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return NotFound(new { Message = "Categoría no encontrada." });

        await _categoryRepository.DeleteAsync(category.Id);
        await _categoryRepository.SaveChangesAsync();
        return NoContent();
    }
}
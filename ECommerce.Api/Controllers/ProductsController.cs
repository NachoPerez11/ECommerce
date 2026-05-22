using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Api.DTOs;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productRepository.GetAllAsync();
        return Ok(products);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return NotFound(new { Message = $"Producto con ID {id} no encontrado." });

        return Ok(product);
    }

    [Authorize(Roles="Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        try
        {
            var product = new Product(dto.Name, dto.Description, dto.Price, dto.Stock);

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
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
        await _productRepository.DeleteAsync(id);
        await _productRepository.SaveChangesAsync();
        return NoContent();
    }
}
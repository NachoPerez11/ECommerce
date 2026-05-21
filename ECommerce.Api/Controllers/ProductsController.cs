using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Api.DTOs;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    // Inyectamos el repositorio que configuramos en la capa de infraestructura
    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productRepository.GetAllAsync();
        return Ok(products);
    }

    // GET: api/products/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return NotFound(new { Message = $"Producto con ID {id} no encontrado." });

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        try
        {
            // Instanciamos el producto ejecutando las validaciones de las reglas de negocio del Dominio
            var product = new Product(dto.Name, dto.Description, dto.Price, dto.Stock);

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            // Retorna un código 201 Created y la URL para consultar el producto nuevo
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (ArgumentException ex)
        {
            // Captura las excepciones de negocio (ej: precio negativo, sin nombre) y devuelve un 400 Bad Request
            return BadRequest(new { Message = ex.Message });
        }
    }
}
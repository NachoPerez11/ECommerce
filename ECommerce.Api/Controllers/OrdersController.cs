using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Api.DTOs;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id);
        if (order == null) return NotFound(new { Message = "Orden no encontrada." });
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        try
        {
            var order = new Order(dto.UserId);

            foreach (var itemDto in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null) 
                    return BadRequest(new { Message = $"El producto con ID {itemDto.ProductId} no existe." });

                // Agrega el item y descuenta el stock usando las reglas del Dominio
                order.AddItem(product, itemDto.Quantity);
                
                // Actualizamos el stock modificado en el repositorio de productos
                _productRepository.Update(product);
            }

            await _orderRepository.AddAsync(order);
            await _productRepository.SaveChangesAsync(); // Guarda los cambios de stock globales

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
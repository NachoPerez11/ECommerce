using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Infrastructure.Repositories;

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

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id);
        if (order == null) return NotFound(new { Message = "Orden no encontrada." });
        return Ok(order);
    }

    [Authorize]
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
                order.AddItem(product, itemDto.Quantity);
                await _productRepository.UpdateAsync(product);
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

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id);
        if (order == null) return NotFound(new { Message = "Orden no encontrada." });
        await _orderRepository.DeleteAsync(order.Id);
        await _orderRepository.SaveChangesAsync();
        return NoContent();
    }
}
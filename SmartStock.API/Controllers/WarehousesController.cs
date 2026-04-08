using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Services;

namespace SmartStock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehousesController(IWarehouseService warehouseService) => _warehouseService = warehouseService;

    [HttpGet]
    public async Task<ActionResult<List<WarehouseDto>>> GetAll([FromQuery] bool includeInactive = false)
    {
        var warehouses = await _warehouseService.GetAllAsync(includeInactive);
        return Ok(warehouses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WarehouseDto>> GetById(int id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse == null) return NotFound();
        return Ok(warehouse);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<ActionResult<WarehouseDto>> Create([FromBody] WarehouseCreateDto dto)
    {
        var warehouse = await _warehouseService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = warehouse.Id }, warehouse);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<ActionResult<WarehouseDto>> Update(int id, [FromBody] WarehouseCreateDto dto)
    {
        var warehouse = await _warehouseService.UpdateAsync(id, dto);
        return Ok(warehouse);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _warehouseService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/restore")]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<ActionResult<WarehouseDto>> Restore(int id)
    {
        var warehouse = await _warehouseService.RestoreAsync(id);
        return Ok(warehouse);
    }
}

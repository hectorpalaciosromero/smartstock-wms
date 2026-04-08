using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Services;

namespace SmartStock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService) => _supplierService = supplierService;

    [HttpGet]
    public async Task<ActionResult<List<SupplierDto>>> GetAll([FromQuery] bool includeInactive = false)
    {
        var suppliers = await _supplierService.GetAllAsync(includeInactive);
        return Ok(suppliers);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<ActionResult<SupplierDto>> Create([FromBody] SupplierCreateDto dto)
    {
        var supplier = await _supplierService.CreateAsync(dto);
        return Ok(supplier);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<ActionResult<SupplierDto>> Update(int id, [FromBody] SupplierCreateDto dto)
    {
        var supplier = await _supplierService.UpdateAsync(id, dto);
        return Ok(supplier);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _supplierService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/restore")]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<ActionResult<SupplierDto>> Restore(int id)
    {
        try
        {
            var supplier = await _supplierService.RestoreAsync(id);
            return Ok(supplier);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Services;
using System.Security.Claims;

namespace SmartStock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MovementsController : ControllerBase
{
    private readonly IMovementService _movementService;

    public MovementsController(IMovementService movementService) => _movementService = movementService;

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<MovementDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? productId = null,
        [FromQuery] int? warehouseId = null,
        [FromQuery] string? movementType = null)
    {
        var result = await _movementService.GetAllAsync(page, pageSize, productId, warehouseId, movementType);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Almacenero,Supervisor")]
    public async Task<ActionResult<MovementDto>> Create([FromBody] MovementCreateDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("uid")?.Value
            ?? throw new UnauthorizedAccessException("User identity could not be resolved.");

        var movement = await _movementService.CreateAsync(dto, userId);
        return Ok(movement);
    }

    /// <summary>Export movements to CSV</summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportCsv(
        [FromQuery] int? warehouseId = null,
        [FromQuery] string? movementType = null)
    {
        var csv = await _movementService.ExportToCsvAsync(warehouseId, movementType);
        var dateStr = DateTime.UtcNow.ToString("yyyy-MM-dd");
        return File(csv, "text/csv;charset=utf-8", $"Movimientos_SmartStock_{dateStr}.csv");
    }
}

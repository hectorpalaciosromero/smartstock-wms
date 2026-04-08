using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Services;
using System.Security.Claims;

namespace SmartStock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScanController : ControllerBase
{
    private readonly IScanService _scanService;

    public ScanController(IScanService scanService) => _scanService = scanService;

    /// <summary>Process a barcode/QR scan and resolve to product</summary>
    [HttpPost]
    public async Task<ActionResult<ScanResultDto>> ProcessScan([FromBody] ScanCreateDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("uid")?.Value
            ?? throw new UnauthorizedAccessException("User identity could not be resolved.");

        var result = await _scanService.ProcessScanAsync(dto, userId);
        return Ok(result);
    }

    /// <summary>Get recent scans for the current user</summary>
    [HttpGet("recent")]
    public async Task<ActionResult<List<ScanResultDto>>> GetRecentScans([FromQuery] int count = 20)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("uid")?.Value
            ?? throw new UnauthorizedAccessException("User identity could not be resolved.");

        var scans = await _scanService.GetRecentScansAsync(userId, count);
        return Ok(scans);
    }
}

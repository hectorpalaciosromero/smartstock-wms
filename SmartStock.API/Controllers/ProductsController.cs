using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Services;

namespace SmartStock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService) => _productService = productService;

    /// <summary>Get all products with pagination and filtering</summary>
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<ProductListDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        [FromQuery] string? search = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] bool? lowStock = null,
        [FromQuery] bool includeInactive = false)
    {
        var result = await _productService.GetAllAsync(page, pageSize, search, categoryId, lowStock, includeInactive);
        return Ok(result);
    }

    /// <summary>Get a product by ID</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    /// <summary>Get a product by barcode or QR code</summary>
    [HttpGet("barcode/{barcode}")]
    public async Task<ActionResult<ProductDto>> GetByBarcode(string barcode)
    {
        var product = await _productService.GetByBarcodeAsync(barcode);
        if (product == null) return NotFound(new { message = $"No product found with barcode/QR: {barcode}" });
        return Ok(product);
    }

    /// <summary>Create a new product</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Almacenero,Supervisor")]
    public async Task<ActionResult<ProductDto>> Create([FromBody] ProductCreateDto dto)
    {
        var product = await _productService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>Update a product</summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Almacenero,Supervisor")]
    public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] ProductUpdateDto dto)
    {
        var product = await _productService.UpdateAsync(id, dto);
        return Ok(product);
    }

    /// <summary>Soft delete a product</summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    /// <summary>Restore a soft-deleted product</summary>
    [HttpPut("{id}/restore")]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<ActionResult<ProductDto>> Restore(int id)
    {
        try
        {
            var product = await _productService.RestoreAsync(id);
            return Ok(product);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>Export filtered products to CSV</summary>
    [HttpGet("export")]
    public async Task<IActionResult> Export(
        [FromQuery] string? search = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] bool? lowStock = null)
    {
        var csvBytes = await _productService.ExportToCsvAsync(search, categoryId, lowStock);
        var filename = $"Inventario_SmartStock_{DateTime.Now:yyyyMMdd_HHmm}.csv";
        return File(csvBytes, "text/csv", filename);
    }

    /// <summary>Execute batch action on products</summary>
    [HttpPost("batch")]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<IActionResult> ExecuteBatchAction([FromBody] BatchActionDto dto)
    {
        if (dto.ProductIds == null || !dto.ProductIds.Any())
            return BadRequest("Se requiere al menos un ID de producto.");

        var count = await _productService.ExecuteBatchActionAsync(dto.ProductIds, dto.Action);
        return Ok(new { message = $"Acción {dto.Action} ejecutada en {count} productos.", count });
    }
}

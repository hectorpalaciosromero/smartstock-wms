using Microsoft.EntityFrameworkCore;
using SmartStock.API.Data;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Models.Entities;

namespace SmartStock.API.Services;

public interface IProductService
{
    Task<PaginatedResponse<ProductListDto>> GetAllAsync(int page, int pageSize, string? search, int? categoryId, bool? lowStock, bool includeInactive = false);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto?> GetByBarcodeAsync(string barcode);
    Task<ProductDto> CreateAsync(ProductCreateDto dto);
    Task<ProductDto> UpdateAsync(int id, ProductUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<byte[]> ExportToCsvAsync(string? search, int? categoryId, bool? lowStock);
    Task<ProductDto> RestoreAsync(int id);
    Task<int> ExecuteBatchActionAsync(List<int> ids, string action);
}

public class ProductService : IProductService
{
    private readonly AppDbContext _db;
    private readonly ILogger<ProductService> _logger;

    public ProductService(AppDbContext db, ILogger<ProductService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<PaginatedResponse<ProductListDto>> GetAllAsync(int page, int pageSize, string? search, int? categoryId, bool? lowStock, bool includeInactive = false)
    {
        var query = _db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => includeInactive || p.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search) || (p.SKU != null && p.SKU.Contains(search)) || (p.Barcode != null && p.Barcode.Contains(search)));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (lowStock == true)
            query = query.Where(p => p.CurrentStock <= p.MinStock);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductListDto(
                p.Id, p.Name, p.SKU, p.Barcode, p.Description, p.Price,
                p.CurrentStock, p.MinStock, p.ImageUrl,
                p.CategoryId, p.Category.Name, p.SupplierId, p.CurrentStock <= p.MinStock, p.IsActive
            ))
            .ToListAsync();

        return new PaginatedResponse<ProductListDto>(
            items, totalCount, page, pageSize,
            (int)Math.Ceiling(totalCount / (double)pageSize)
        );
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        return await _db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.Id == id)
            .Select(p => new ProductDto(
                p.Id, p.Name, p.SKU, p.Barcode, p.QRCode, p.Description,
                p.Price, p.CurrentStock, p.MinStock, p.ImageUrl, p.AICategory,
                p.CategoryId, p.Category.Name, p.SupplierId, p.Supplier != null ? p.Supplier.CompanyName : null,
                p.IsActive, p.CreatedAt, p.UpdatedAt
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<ProductDto?> GetByBarcodeAsync(string barcode)
    {
        return await _db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.Barcode == barcode || p.QRCode == barcode)
            .Select(p => new ProductDto(
                p.Id, p.Name, p.SKU, p.Barcode, p.QRCode, p.Description,
                p.Price, p.CurrentStock, p.MinStock, p.ImageUrl, p.AICategory,
                p.CategoryId, p.Category.Name, p.SupplierId, p.Supplier != null ? p.Supplier.CompanyName : null,
                p.IsActive, p.CreatedAt, p.UpdatedAt
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            SKU = dto.SKU,
            Barcode = dto.Barcode,
            Description = dto.Description,
            Price = dto.Price,
            CurrentStock = dto.CurrentStock,
            MinStock = dto.MinStock,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId,
            SupplierId = dto.SupplierId,
            QRCode = $"SMARTSTOCK-{DateTime.UtcNow.Ticks}"
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Product created: {Name} (ID: {Id}, SKU: {SKU})", product.Name, product.Id, product.SKU);

        return (await GetByIdAsync(product.Id))!;
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var product = await _db.Products.FindAsync(id)
            ?? throw new KeyNotFoundException($"Product {id} not found");

        product.Name = dto.Name;
        product.SKU = dto.SKU;
        product.Barcode = dto.Barcode;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.MinStock = dto.MinStock;
        product.ImageUrl = dto.ImageUrl;
        product.CategoryId = dto.CategoryId;
        product.SupplierId = dto.SupplierId;
        product.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        _logger.LogInformation("Product updated: {Name} (ID: {Id})", product.Name, id);

        return (await GetByIdAsync(product.Id))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return false;

        product.IsActive = false;
        await _db.SaveChangesAsync();

        _logger.LogInformation("Product soft-deleted: ID {Id}", id);
        return true;
    }

    public async Task<byte[]> ExportToCsvAsync(string? search, int? categoryId, bool? lowStock)
    {
        var query = _db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search) || (p.SKU != null && p.SKU.Contains(search)) || (p.Barcode != null && p.Barcode.Contains(search)));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (lowStock == true)
            query = query.Where(p => p.CurrentStock <= p.MinStock);

        // Fetch all matching without pagination
        var products = await query.OrderBy(p => p.Name).ToListAsync();

        var csv = new System.Text.StringBuilder();
        // Spanish standard headers require semicolon (;) so Excel LATAM parses columns automatically
        csv.AppendLine("Nombre;SKU;Código de Barras;Categoría;Proveedor;Stock Actual;Stock Mínimo;Precio (S/.)");
        
        foreach(var p in products)
        {
            // Escape quotes and commas
            var name = $"\"{p.Name.Replace("\"", "\"\"")}\"";
            var cat = p.Category != null ? $"\"{p.Category.Name.Replace("\"", "\"\"")}\"" : "";
            var sup = p.Supplier != null ? $"\"{p.Supplier.CompanyName.Replace("\"", "\"\"")}\"" : "";
            
            // Format to ensure decimal goes via string correctly without breaking CSV
            var priceString = p.Price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);

            csv.AppendLine($"{name};{p.SKU};{p.Barcode};{cat};{sup};{p.CurrentStock};{p.MinStock};{priceString}");
        }

        // Must prepend UTF8 BOM (Byte Order Mark) so MS Excel opens foreign chars (á, ñ) flawlessly
        var bom = System.Text.Encoding.UTF8.GetPreamble();
        var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        return bom.Concat(bytes).ToArray();
    }

    public async Task<ProductDto> RestoreAsync(int id)
    {
        var product = await _db.Products.FindAsync(id)
            ?? throw new KeyNotFoundException($"Product {id} not found");

        product.IsActive = true;
        product.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        _logger.LogInformation("Product restored: {Name} (ID: {Id})", product.Name, id);
        return (await GetByIdAsync(product.Id))!;
    }

    public async Task<int> ExecuteBatchActionAsync(List<int> ids, string action)
    {
        var products = await _db.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
        if (!products.Any()) return 0;

        foreach (var product in products)
        {
            if (action.Equals("Archive", StringComparison.OrdinalIgnoreCase))
                product.IsActive = false;
            else if (action.Equals("Restore", StringComparison.OrdinalIgnoreCase))
                product.IsActive = true;
            
            product.UpdatedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("Batch action '{Action}' executed on {Count} products", action, products.Count);
        return products.Count;
    }
}

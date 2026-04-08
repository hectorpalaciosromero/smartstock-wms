using Microsoft.EntityFrameworkCore;
using SmartStock.API.Data;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Models.Entities;

namespace SmartStock.API.Services;

public interface IScanService
{
    Task<ScanResultDto> ProcessScanAsync(ScanCreateDto dto, string userId);
    Task<List<ScanResultDto>> GetRecentScansAsync(string userId, int count = 20);
}

public class ScanService : IScanService
{
    private readonly AppDbContext _db;
    private readonly IProductService _productService;
    private readonly ILogger<ScanService> _logger;

    public ScanService(AppDbContext db, IProductService productService, ILogger<ScanService> logger)
    {
        _db = db;
        _productService = productService;
        _logger = logger;
    }

    public async Task<ScanResultDto> ProcessScanAsync(ScanCreateDto dto, string userId)
    {
        // Try to find product by barcode or QR code
        var product = await _productService.GetByBarcodeAsync(dto.RawData);

        var scanLog = new ScanLog
        {
            UserId = userId,
            ProductId = product != null ? (int?)product.Id : null,
            ScanType = dto.ScanType,
            RawData = dto.RawData,
            DeviceInfo = dto.DeviceInfo,
            WasResolved = product != null
        };

        _db.ScanLogs.Add(scanLog);
        await _db.SaveChangesAsync();

        _logger.LogInformation(
            "Scan processed: Type={ScanType}, Resolved={Resolved}, RawData={RawData}, UserId={UserId}",
            dto.ScanType, product != null, dto.RawData, userId);

        return new ScanResultDto(
            scanLog.Id,
            product != null,
            product,
            scanLog.ScanType,
            scanLog.RawData,
            scanLog.ScannedAt
        );
    }

    public async Task<List<ScanResultDto>> GetRecentScansAsync(string userId, int count = 20)
    {
        var scans = await _db.ScanLogs
            .AsNoTracking()
            .Include(s => s.Product)
                .ThenInclude(p => p!.Category)
            .Include(s => s.Product)
                .ThenInclude(p => p!.Supplier)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.ScannedAt)
            .Take(count)
            .ToListAsync();

        return scans.Select(s => new ScanResultDto(
            s.Id,
            s.WasResolved,
            s.Product != null ? new ProductDto(
                s.Product.Id, s.Product.Name, s.Product.SKU, s.Product.Barcode,
                s.Product.QRCode, s.Product.Description, s.Product.Price,
                s.Product.CurrentStock, s.Product.MinStock, s.Product.ImageUrl,
                s.Product.AICategory, s.Product.CategoryId, s.Product.Category.Name,
                s.Product.SupplierId, s.Product.Supplier?.CompanyName,
                s.Product.IsActive, s.Product.CreatedAt, s.Product.UpdatedAt
            ) : null,
            s.ScanType,
            s.RawData,
            s.ScannedAt
        )).ToList();
    }
}

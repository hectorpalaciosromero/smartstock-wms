using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SmartStock.API.Data;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Models.Entities;

namespace SmartStock.API.Services;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _db;
    private readonly ILogger<DashboardService> _logger;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "Dashboard_Data";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(30);

    public DashboardService(AppDbContext db, ILogger<DashboardService> logger, IMemoryCache cache)
    {
        _db = db;
        _logger = logger;
        _cache = cache;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        if (_cache.TryGetValue(CacheKey, out DashboardDto? cached) && cached != null)
        {
            _logger.LogDebug("Dashboard served from cache");
            return cached;
        }

        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);
        var weekAgo = today.AddDays(-6);

        var totalProducts = await _db.Products.AsNoTracking().CountAsync(p => p.IsActive);
        var totalCategories = await _db.Categories.AsNoTracking().CountAsync(c => c.IsActive);
        var totalWarehouses = await _db.Warehouses.AsNoTracking().CountAsync(w => w.IsActive);
        var lowStockCount = await _db.Products.AsNoTracking().CountAsync(p => p.IsActive && p.CurrentStock <= p.MinStock && p.CurrentStock > 0);
        var outOfStockCount = await _db.Products.AsNoTracking().CountAsync(p => p.IsActive && p.CurrentStock == 0);
        var todayMovements = await _db.StockMovements.AsNoTracking().CountAsync(m => m.CreatedAt >= today && m.CreatedAt < tomorrow);

        // Calculate total value in memory to avoid SQL translation issues
        var activeProducts = await _db.Products.AsNoTracking()
            .Where(p => p.IsActive)
            .Select(p => new { p.Price, p.CurrentStock })
            .ToListAsync();
        var totalValue = activeProducts.Sum(p => p.Price * p.CurrentStock);

        // Stock by category
        var stockByCategoryRaw = await _db.Categories
            .AsNoTracking()
            .Where(c => c.IsActive)
            .Select(c => new
            {
                c.Name,
                TotalProducts = c.Products.Count(p => p.IsActive),
                TotalStock    = c.Products.Where(p => p.IsActive).Sum(p => (int?)p.CurrentStock) ?? 0
            })
            .ToListAsync();

        var stockByCategory = stockByCategoryRaw
            .OrderByDescending(c => c.TotalStock)
            .Select(c => new CategoryStockDto(c.Name, c.TotalProducts, c.TotalStock))
            .ToList();

        var recentMovementsRaw = await _db.StockMovements
            .AsNoTracking()
            .OrderByDescending(m => m.CreatedAt)
            .Take(10)
            .Select(m => new {
                m.Id, m.ProductId, ProductName = m.Product.Name, ProductSKU = m.Product.SKU,
                m.WarehouseId, WarehouseName = m.Warehouse.Name, m.UserId, UserName = m.User.FullName,
                m.MovementType, m.Quantity, m.ScanMethod, m.Notes, m.CreatedAt
            })
            .ToListAsync();

        var recentMovements = recentMovementsRaw
            .Select(m => new MovementDto(
                m.Id, m.ProductId, m.ProductName, m.ProductSKU,
                m.WarehouseId, m.WarehouseName, m.UserId, m.UserName,
                m.MovementType.ToString(), m.Quantity, m.ScanMethod.ToString(),
                m.Notes, m.CreatedAt
            ))
            .ToList();

        var lowStockProducts = await _db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.CurrentStock <= p.MinStock)
            .OrderBy(p => p.CurrentStock)
            .Take(10)
            .Select(p => new ProductListDto(
                p.Id, p.Name, p.SKU, p.Barcode, p.Description, p.Price,
                p.CurrentStock, p.MinStock, p.ImageUrl,
                p.CategoryId, p.Category.Name, p.SupplierId, true, p.IsActive
            ))
            .ToListAsync();

        // Movement trend last 7 days
        var rawMovementsData = await _db.StockMovements
            .AsNoTracking()
            .Where(m => m.CreatedAt >= weekAgo && m.CreatedAt < tomorrow)
            .Select(m => new { m.CreatedAt, m.MovementType })
            .ToListAsync();

        var movementsRaw = rawMovementsData
            .GroupBy(m => new { Date = m.CreatedAt.Date, m.MovementType })
            .Select(g => new
            {
                g.Key.Date,
                g.Key.MovementType,
                Count = g.Count()
            })
            .ToList();

        var movementTrend = new List<DailyMovementDto>();
        for (int i = 6; i >= 0; i--)
        {
            var day = today.AddDays(-i);
            var entries = movementsRaw
                .Where(m => m.Date == day && m.MovementType == MovementType.Entry)
                .Sum(m => m.Count);
            var exits = movementsRaw
                .Where(m => m.Date == day && m.MovementType == MovementType.Exit)
                .Sum(m => m.Count);
            movementTrend.Add(new DailyMovementDto(day.ToString("dd/MM"), entries, exits));
        }

        _logger.LogDebug("Dashboard loaded from DB: {Products} products, {LowStock} low stock", totalProducts, lowStockCount);

        var result = new DashboardDto(
            totalProducts, totalCategories, totalWarehouses,
            lowStockCount, outOfStockCount, todayMovements, totalValue,
            stockByCategory, recentMovements, lowStockProducts, movementTrend
        );

        // Cache for 30 seconds to reduce DB load on rapid page refreshes
        _cache.Set(CacheKey, result, CacheDuration);

        return result;
    }
}

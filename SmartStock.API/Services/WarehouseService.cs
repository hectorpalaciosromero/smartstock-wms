using Microsoft.EntityFrameworkCore;
using SmartStock.API.Data;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Models.Entities;

namespace SmartStock.API.Services;

public interface IWarehouseService
{
    Task<List<WarehouseDto>> GetAllAsync(bool includeInactive = false);
    Task<WarehouseDto?> GetByIdAsync(int id);
    Task<WarehouseDto> CreateAsync(WarehouseCreateDto dto);
    Task<WarehouseDto> UpdateAsync(int id, WarehouseCreateDto dto);
    Task DeleteAsync(int id);
    Task<WarehouseDto> RestoreAsync(int id);
}

public class WarehouseService : IWarehouseService
{
    private readonly AppDbContext _db;
    private readonly ILogger<WarehouseService> _logger;

    public WarehouseService(AppDbContext db, ILogger<WarehouseService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<List<WarehouseDto>> GetAllAsync(bool includeInactive = false)
    {
        return await _db.Warehouses
            .AsNoTracking()
            .Where(w => includeInactive || w.IsActive)
            .OrderBy(w => w.Name)
            .Select(w => new WarehouseDto(
                w.Id, w.Name, w.Location, w.Code, w.IsActive,
                w.StockMovements.Count
            ))
            .ToListAsync();
    }

    public async Task<WarehouseDto?> GetByIdAsync(int id)
    {
        return await _db.Warehouses
            .AsNoTracking()
            .Where(w => w.Id == id)
            .Select(w => new WarehouseDto(
                w.Id, w.Name, w.Location, w.Code, w.IsActive,
                w.StockMovements.Count
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<WarehouseDto> CreateAsync(WarehouseCreateDto dto)
    {
        var existing = await _db.Warehouses.FirstOrDefaultAsync(w => w.Code == dto.Code);
        if (existing != null)
            throw new ArgumentException($"El código de almacén '{dto.Code}' ya está en uso.");

        var warehouse = new Warehouse
        {
            Name = dto.Name,
            Location = dto.Location,
            Code = dto.Code
        };

        _db.Warehouses.Add(warehouse);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Warehouse created: {Name} ({Code})", warehouse.Name, warehouse.Code);

        return new WarehouseDto(warehouse.Id, warehouse.Name, warehouse.Location, warehouse.Code, warehouse.IsActive, 0);
    }

    public async Task<WarehouseDto> UpdateAsync(int id, WarehouseCreateDto dto)
    {
        var warehouse = await _db.Warehouses.FindAsync(id)
            ?? throw new KeyNotFoundException($"Warehouse {id} not found");

        var existing = await _db.Warehouses.FirstOrDefaultAsync(w => w.Code == dto.Code && w.Id != id);
        if (existing != null)
            throw new ArgumentException($"El código de almacén '{dto.Code}' ya está en uso.");

        warehouse.Name = dto.Name;
        warehouse.Location = dto.Location;
        warehouse.Code = dto.Code;

        await _db.SaveChangesAsync();
        _logger.LogInformation("Warehouse updated: {Name} (ID: {Id})", warehouse.Name, id);

        return new WarehouseDto(warehouse.Id, warehouse.Name, warehouse.Location, warehouse.Code, warehouse.IsActive,
            await _db.StockMovements.CountAsync(m => m.WarehouseId == id));
    }

    public async Task DeleteAsync(int id)
    {
        var warehouse = await _db.Warehouses.FindAsync(id)
            ?? throw new KeyNotFoundException($"Warehouse {id} not found");

        warehouse.IsActive = false;
        await _db.SaveChangesAsync();
        _logger.LogInformation("Warehouse soft-deleted: ID {Id}", id);
    }

    public async Task<WarehouseDto> RestoreAsync(int id)
    {
        var warehouse = await _db.Warehouses.FindAsync(id)
            ?? throw new KeyNotFoundException($"Warehouse {id} not found");

        warehouse.IsActive = true;
        await _db.SaveChangesAsync();
        _logger.LogInformation("Warehouse restored: {Name} (ID: {Id})", warehouse.Name, id);

        return new WarehouseDto(warehouse.Id, warehouse.Name, warehouse.Location, warehouse.Code, warehouse.IsActive,
            await _db.StockMovements.CountAsync(m => m.WarehouseId == id));
    }
}

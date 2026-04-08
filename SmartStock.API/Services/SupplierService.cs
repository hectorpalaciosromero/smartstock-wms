using Microsoft.EntityFrameworkCore;
using SmartStock.API.Data;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Models.Entities;

namespace SmartStock.API.Services;

public interface ISupplierService
{
    Task<List<SupplierDto>> GetAllAsync(bool includeInactive = false);
    Task<SupplierDto> CreateAsync(SupplierCreateDto dto);
    Task<SupplierDto> UpdateAsync(int id, SupplierCreateDto dto);
    Task DeleteAsync(int id);
    Task<SupplierDto> RestoreAsync(int id);
}

public class SupplierService : ISupplierService
{
    private readonly AppDbContext _db;
    private readonly ILogger<SupplierService> _logger;

    public SupplierService(AppDbContext db, ILogger<SupplierService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<List<SupplierDto>> GetAllAsync(bool includeInactive = false)
    {
        return await _db.Suppliers
            .AsNoTracking()
            .Where(s => includeInactive || s.IsActive)
            .OrderBy(s => s.CompanyName)
            .Select(s => new SupplierDto(
                s.Id, s.CompanyName, s.ContactName, s.Phone, s.Email, s.Address,
                s.IsActive, s.Products.Count(p => p.IsActive)
            ))
            .ToListAsync();
    }

    public async Task<SupplierDto> CreateAsync(SupplierCreateDto dto)
    {
        var supplier = new Supplier
        {
            CompanyName = dto.CompanyName,
            ContactName = dto.ContactName,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address
        };

        _db.Suppliers.Add(supplier);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Supplier created: {Name} (ID: {Id})", supplier.CompanyName, supplier.Id);

        return new SupplierDto(supplier.Id, supplier.CompanyName, supplier.ContactName,
            supplier.Phone, supplier.Email, supplier.Address, supplier.IsActive, 0);
    }

    public async Task<SupplierDto> UpdateAsync(int id, SupplierCreateDto dto)
    {
        var supplier = await _db.Suppliers.FindAsync(id)
            ?? throw new KeyNotFoundException($"Supplier {id} not found");

        supplier.CompanyName = dto.CompanyName;
        supplier.ContactName = dto.ContactName;
        supplier.Phone = dto.Phone;
        supplier.Email = dto.Email;
        supplier.Address = dto.Address;

        await _db.SaveChangesAsync();
        _logger.LogInformation("Supplier updated: {Name} (ID: {Id})", supplier.CompanyName, id);

        return new SupplierDto(supplier.Id, supplier.CompanyName, supplier.ContactName,
            supplier.Phone, supplier.Email, supplier.Address, supplier.IsActive,
            await _db.Products.CountAsync(p => p.SupplierId == id && p.IsActive));
    }

    public async Task DeleteAsync(int id)
    {
        var supplier = await _db.Suppliers.FindAsync(id)
            ?? throw new KeyNotFoundException($"Supplier {id} not found");

        supplier.IsActive = false;
        await _db.SaveChangesAsync();
        _logger.LogInformation("Supplier soft-deleted: ID {Id}", id);
    }

    public async Task<SupplierDto> RestoreAsync(int id)
    {
        var supplier = await _db.Suppliers.FindAsync(id)
            ?? throw new KeyNotFoundException($"Supplier {id} not found");

        supplier.IsActive = true;
        await _db.SaveChangesAsync();
        _logger.LogInformation("Supplier restored: {Name} (ID: {Id})", supplier.CompanyName, id);

        return new SupplierDto(supplier.Id, supplier.CompanyName, supplier.ContactName,
            supplier.Phone, supplier.Email, supplier.Address, supplier.IsActive,
            await _db.Products.CountAsync(p => p.SupplierId == id && p.IsActive));
    }
}

using Microsoft.EntityFrameworkCore;
using SmartStock.API.Data;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Models.Entities;

namespace SmartStock.API.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync(bool includeInactive = false);
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<CategoryDto> CreateAsync(CategoryCreateDto dto);
    Task UpdateAsync(int id, CategoryCreateDto dto);
    Task DeleteAsync(int id);
    Task<CategoryDto> RestoreAsync(int id);
}

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(AppDbContext db, ILogger<CategoryService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<List<CategoryDto>> GetAllAsync(bool includeInactive = false)
    {
        return await _db.Categories
            .AsNoTracking()
            .Where(c => includeInactive || c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto(
                c.Id, c.Name, c.Description, c.ImageUrl, c.IsActive,
                c.Products.Count(p => p.IsActive)
            ))
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        return await _db.Categories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoryDto(
                c.Id, c.Name, c.Description, c.ImageUrl, c.IsActive,
                c.Products.Count(p => p.IsActive)
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Category created: {Name} (ID: {Id})", category.Name, category.Id);

        return new CategoryDto(category.Id, category.Name, category.Description, category.ImageUrl, category.IsActive, 0);
    }

    public async Task UpdateAsync(int id, CategoryCreateDto dto)
    {
        var category = await _db.Categories.FindAsync(id)
            ?? throw new KeyNotFoundException($"Category {id} not found");

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.ImageUrl = dto.ImageUrl;

        await _db.SaveChangesAsync();
        _logger.LogInformation("Category updated: {Name} (ID: {Id})", category.Name, id);
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _db.Categories.FindAsync(id)
            ?? throw new KeyNotFoundException($"Category {id} not found");

        category.IsActive = false;
        await _db.SaveChangesAsync();
        _logger.LogInformation("Category soft-deleted: ID {Id}", id);
    }

    public async Task<CategoryDto> RestoreAsync(int id)
    {
        var category = await _db.Categories.FindAsync(id)
            ?? throw new KeyNotFoundException($"Category {id} not found");

        category.IsActive = true;
        await _db.SaveChangesAsync();

        _logger.LogInformation("Category restored: {Name} (ID: {Id})", category.Name, id);
        return new CategoryDto(category.Id, category.Name, category.Description, category.ImageUrl, category.IsActive, 
            await _db.Products.CountAsync(p => p.CategoryId == id && p.IsActive));
    }
}

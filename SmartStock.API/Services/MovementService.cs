using Microsoft.EntityFrameworkCore;
using SmartStock.API.Data;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Models.Entities;

namespace SmartStock.API.Services;

public interface IMovementService
{
    Task<PaginatedResponse<MovementDto>> GetAllAsync(int page, int pageSize, int? productId, int? warehouseId, string? movementType);
    Task<MovementDto> CreateAsync(MovementCreateDto dto, string userId);
    Task<byte[]> ExportToCsvAsync(int? warehouseId, string? movementType);
}

public class MovementService : IMovementService
{
    private readonly AppDbContext _db;
    private readonly ILogger<MovementService> _logger;

    public MovementService(AppDbContext db, ILogger<MovementService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<PaginatedResponse<MovementDto>> GetAllAsync(int page, int pageSize, int? productId, int? warehouseId, string? movementType)
    {
        var query = _db.StockMovements
            .AsNoTracking()
            .Include(m => m.Product)
            .Include(m => m.Warehouse)
            .Include(m => m.User)
            .AsQueryable();

        if (productId.HasValue)
            query = query.Where(m => m.ProductId == productId.Value);

        if (warehouseId.HasValue)
            query = query.Where(m => m.WarehouseId == warehouseId.Value);

        // Server-side movement type filter
        if (!string.IsNullOrWhiteSpace(movementType) && Enum.TryParse<MovementType>(movementType, true, out var mt))
            query = query.Where(m => m.MovementType == mt);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MovementDto(
                m.Id, m.ProductId, m.Product.Name, m.Product.SKU,
                m.WarehouseId, m.Warehouse.Name, m.UserId, m.User.FullName,
                m.MovementType.ToString(), m.Quantity, m.ScanMethod.ToString(),
                m.Notes, m.CreatedAt
            ))
            .ToListAsync();

        return new PaginatedResponse<MovementDto>(
            items, totalCount, page, pageSize,
            (int)Math.Ceiling(totalCount / (double)pageSize)
        );
    }

    public async Task<MovementDto> CreateAsync(MovementCreateDto dto, string userId)
    {
        var product = await _db.Products.FindAsync(dto.ProductId)
            ?? throw new KeyNotFoundException($"Product {dto.ProductId} not found");

        var warehouse = await _db.Warehouses.FindAsync(dto.WarehouseId)
            ?? throw new KeyNotFoundException($"Warehouse {dto.WarehouseId} not found");

        if (!warehouse.IsActive)
            throw new InvalidOperationException($"No se pueden realizar movimientos de stock en el almacén archivado: {warehouse.Name}");

        if (!Enum.TryParse<MovementType>(dto.MovementType, true, out var movementType))
            throw new ArgumentException($"Invalid movement type: {dto.MovementType}");

        var scanMethod = ScanMethod.Manual;
        if (!string.IsNullOrWhiteSpace(dto.ScanMethod))
            Enum.TryParse(dto.ScanMethod, true, out scanMethod);

        // Update stock based on movement type
        switch (movementType)
        {
            case MovementType.Entry:
                product.CurrentStock += dto.Quantity;
                break;
            case MovementType.Exit:
                if (product.CurrentStock < dto.Quantity)
                    throw new InvalidOperationException($"Insufficient stock. Available: {product.CurrentStock}, Requested: {dto.Quantity}");
                product.CurrentStock -= dto.Quantity;
                break;
            case MovementType.Adjustment:
                product.CurrentStock = dto.Quantity;
                break;
        }

        product.UpdatedAt = DateTime.UtcNow;

        var movement = new StockMovement
        {
            ProductId = dto.ProductId,
            WarehouseId = dto.WarehouseId,
            UserId = userId,
            MovementType = movementType,
            Quantity = dto.Quantity,
            ScanMethod = scanMethod,
            Notes = dto.Notes
        };

        _db.StockMovements.Add(movement);
        await _db.SaveChangesAsync();

        _logger.LogInformation(
            "Movement {Type} created: {Quantity}x {ProductName} at {Warehouse} by user {UserId}",
            movementType, dto.Quantity, product.Name, warehouse.Name, userId);

        return new MovementDto(
            movement.Id, movement.ProductId, product.Name, product.SKU,
            movement.WarehouseId, warehouse.Name, movement.UserId, "",
            movement.MovementType.ToString(), movement.Quantity,
            movement.ScanMethod.ToString(), movement.Notes, movement.CreatedAt
        );
    }

    public async Task<byte[]> ExportToCsvAsync(int? warehouseId, string? movementType)
    {
        var query = _db.StockMovements
            .AsNoTracking()
            .Include(m => m.Product)
            .Include(m => m.Warehouse)
            .Include(m => m.User)
            .AsQueryable();

        if (warehouseId.HasValue)
            query = query.Where(m => m.WarehouseId == warehouseId.Value);

        if (!string.IsNullOrWhiteSpace(movementType) && Enum.TryParse<MovementType>(movementType, true, out var mt))
            query = query.Where(m => m.MovementType == mt);

        var movements = await query.OrderByDescending(m => m.CreatedAt).ToListAsync();

        var csv = new System.Text.StringBuilder();
        csv.AppendLine("Fecha;Tipo;Producto;SKU;Almacén;Cantidad;Método;Usuario;Notas");

        foreach (var m in movements)
        {
            var tipo = m.MovementType switch
            {
                MovementType.Entry => "Entrada",
                MovementType.Exit => "Salida",
                MovementType.Adjustment => "Ajuste",
                _ => "Transferencia"
            };
            var fecha = m.CreatedAt.ToString("dd/MM/yyyy HH:mm");
            var producto = $"\"{m.Product?.Name?.Replace("\"", "\"\"")}\"";
            var almacen = $"\"{m.Warehouse?.Name?.Replace("\"", "\"\"")}\"";
            var usuario = $"\"{m.User?.FullName?.Replace("\"", "\"\"")}\"";
            var notas = $"\"{(m.Notes ?? "").Replace("\"", "\"\"")}\"";

            csv.AppendLine($"{fecha};{tipo};{producto};{m.Product?.SKU};{almacen};{m.Quantity};{m.ScanMethod};{usuario};{notas}");
        }

        var bom = System.Text.Encoding.UTF8.GetPreamble();
        var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        return bom.Concat(bytes).ToArray();
    }
}

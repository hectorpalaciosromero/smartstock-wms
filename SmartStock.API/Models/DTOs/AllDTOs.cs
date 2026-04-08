using System.ComponentModel.DataAnnotations;

namespace SmartStock.API.Models.DTOs;

// ============ AUTH DTOs ============

public record RegisterDto(
    [Required] string FullName,
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password,
    string? Phone = null,
    string? Department = null,
    string? Position = null,
    DateTime? BirthDate = null,
    string? Role = "Viewer"
);

public record LoginDto(
    [Required, EmailAddress] string Email,
    [Required] string Password
);

public record UpdateRoleDto(
    [Required] string Role
);

public record AuthResponseDto(
    string Token,
    string RefreshToken,
    string UserId,
    string FullName,
    string Email,
    string Role,
    DateTime Expiration,
    string? AvatarUrl = null,
    string? Phone = null,
    string? Department = null,
    string? Position = null,
    DateTime? BirthDate = null
);

public record UpdateProfileDto(
    [Required, MaxLength(150)] string FullName,
    string? Phone = null,
    string? Department = null,
    string? Position = null,
    DateTime? BirthDate = null,
    string? AvatarUrl = null
);

public record ChangePasswordDto(
    [Required] string CurrentPassword,
    [Required, MinLength(6)] string NewPassword
);

public record UserDto(
    string Id,
    string FullName,
    string Email,
    string? AvatarUrl,
    string? Phone,
    string? Department,
    string? Position,
    DateTime? BirthDate,
    string Role,
    bool IsActive,
    DateTime CreatedAt
);

// ============ PRODUCT DTOs ============

public record ProductCreateDto(
    [Required, MaxLength(200)] string Name,
    string? SKU,
    string? Barcode,
    string? Description,
    decimal Price,
    int CurrentStock,
    int MinStock,
    string? ImageUrl,
    [Required] int CategoryId,
    int? SupplierId
);

public record ProductUpdateDto(
    [Required, MaxLength(200)] string Name,
    string? SKU,
    string? Barcode,
    string? Description,
    decimal Price,
    int MinStock,
    string? ImageUrl,
    [Required] int CategoryId,
    int? SupplierId
);

public record BatchActionDto(
    [Required] List<int> ProductIds,
    [Required] string Action // "Archive", "Restore"
);

public record ProductDto(
    int Id,
    string Name,
    string? SKU,
    string? Barcode,
    string? QRCode,
    string? Description,
    decimal Price,
    int CurrentStock,
    int MinStock,
    string? ImageUrl,
    string? AICategory,
    int CategoryId,
    string CategoryName,
    int? SupplierId,
    string? SupplierName,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ProductListDto(
    int Id,
    string Name,
    string? SKU,
    string? Barcode,
    string? Description,
    decimal Price,
    int CurrentStock,
    int MinStock,
    string? ImageUrl,
    int CategoryId,
    string CategoryName,
    int? SupplierId,
    bool IsLowStock,
    bool IsActive
);

// ============ CATEGORY DTOs ============

public record CategoryCreateDto(
    [Required, MaxLength(100)] string Name,
    string? Description,
    string? ImageUrl
);

public record CategoryDto(
    int Id,
    string Name,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    int ProductCount
);

// ============ SUPPLIER DTOs ============

public record SupplierCreateDto(
    [Required, MaxLength(200)] string CompanyName,
    string? ContactName,
    string? Phone,
    string? Email,
    string? Address
);

public record SupplierDto(
    int Id,
    string CompanyName,
    string? ContactName,
    string? Phone,
    string? Email,
    string? Address,
    bool IsActive,
    int ProductCount
);

// ============ WAREHOUSE DTOs ============

public record WarehouseCreateDto(
    [Required, MaxLength(150)] string Name,
    string? Location,
    [Required, MaxLength(20)] string Code
);

public record WarehouseDto(
    int Id,
    string Name,
    string? Location,
    string Code,
    bool IsActive,
    int TotalMovements
);

// ============ MOVEMENT DTOs ============

public record MovementCreateDto(
    [Required] int ProductId,
    [Required] int WarehouseId,
    [Required] string MovementType, // Entry, Exit, Transfer, Adjustment
    [Required] int Quantity,
    string? ScanMethod, // Manual, Barcode, QRCode, RFID, ImageRecognition
    string? Notes
);

public record MovementDto(
    int Id,
    int ProductId,
    string ProductName,
    string? ProductSKU,
    int WarehouseId,
    string WarehouseName,
    string UserId,
    string UserName,
    string MovementType,
    int Quantity,
    string ScanMethod,
    string? Notes,
    DateTime CreatedAt
);

// ============ SCAN DTOs ============

public record ScanCreateDto(
    [Required] string ScanType,
    [Required] string RawData,
    string? DeviceInfo
);

public record ScanResultDto(
    int ScanLogId,
    bool ProductFound,
    ProductDto? Product,
    string ScanType,
    string RawData,
    DateTime ScannedAt
);

// ============ DASHBOARD DTOs ============

public record DashboardDto(
    int TotalProducts,
    int TotalCategories,
    int TotalWarehouses,
    int LowStockCount,
    int OutOfStockCount,
    int TodayMovements,
    decimal TotalInventoryValue,
    List<CategoryStockDto> StockByCategory,
    List<MovementDto> RecentMovements,
    List<ProductListDto> LowStockProducts,
    List<DailyMovementDto> MovementTrend
);

public record CategoryStockDto(
    string CategoryName,
    int TotalProducts,
    int TotalStock
);

public record DailyMovementDto(
    string Date,
    int Entries,
    int Exits
);

// ============ PAGINATION ============

public record PaginatedResponse<T>(
    List<T> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

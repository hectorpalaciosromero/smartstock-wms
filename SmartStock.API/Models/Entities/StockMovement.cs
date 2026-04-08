using System.ComponentModel.DataAnnotations;

namespace SmartStock.API.Models.Entities;

public enum MovementType
{
    Entry,
    Exit,
    Transfer,
    Adjustment
}

public enum ScanMethod
{
    Manual,
    Barcode,
    QRCode,
    RFID,
    ImageRecognition
}

public class StockMovement
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public int WarehouseId { get; set; }

    [Required, MaxLength(450)]
    public string UserId { get; set; } = string.Empty;

    public MovementType MovementType { get; set; }

    public int Quantity { get; set; }

    public ScanMethod ScanMethod { get; set; } = ScanMethod.Manual;

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Product Product { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}

using System.ComponentModel.DataAnnotations;

namespace SmartStock.API.Models.Entities;

public class ScanLog
{
    public int Id { get; set; }

    [Required, MaxLength(450)]
    public string UserId { get; set; } = string.Empty;

    public int? ProductId { get; set; }

    [Required, MaxLength(50)]
    public string ScanType { get; set; } = string.Empty; // QR, Barcode, EAN13, Code128, etc.

    [Required, MaxLength(1000)]
    public string RawData { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? DeviceInfo { get; set; }

    public bool WasResolved { get; set; } = false;

    public DateTime ScannedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ApplicationUser User { get; set; } = null!;
    public Product? Product { get; set; }
}

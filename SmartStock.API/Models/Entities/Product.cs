using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartStock.API.Models.Entities;

public class Product
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? SKU { get; set; }

    [MaxLength(100)]
    public string? Barcode { get; set; }

    [MaxLength(500)]
    public string? QRCode { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int CurrentStock { get; set; } = 0;

    public int MinStock { get; set; } = 5;

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [MaxLength(100)]
    public string? AICategory { get; set; }

    public bool IsActive { get; set; } = true;

    // Foreign Keys
    public int CategoryId { get; set; }
    public int? SupplierId { get; set; }

    // Navigation
    public Category Category { get; set; } = null!;
    public Supplier? Supplier { get; set; }
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    public ICollection<ScanLog> ScanLogs { get; set; } = new List<ScanLog>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

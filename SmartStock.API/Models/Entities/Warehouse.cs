using System.ComponentModel.DataAnnotations;

namespace SmartStock.API.Models.Entities;

public class Warehouse
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Location { get; set; }

    [Required, MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}

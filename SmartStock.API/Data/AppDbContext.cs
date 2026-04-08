using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartStock.API.Models.Entities;

namespace SmartStock.API.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<ScanLog> ScanLogs => Set<ScanLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Product configuration
        builder.Entity<Product>(entity =>
        {
            entity.HasIndex(p => p.SKU).IsUnique().HasFilter("[SKU] IS NOT NULL");
            entity.HasIndex(p => p.Barcode);
            entity.HasIndex(p => p.QRCode);

            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Supplier)
                  .WithMany(s => s.Products)
                  .HasForeignKey(p => p.SupplierId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // StockMovement configuration
        builder.Entity<StockMovement>(entity =>
        {
            entity.HasIndex(m => m.CreatedAt);

            entity.HasOne(m => m.Product)
                  .WithMany(p => p.StockMovements)
                  .HasForeignKey(m => m.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Warehouse)
                  .WithMany(w => w.StockMovements)
                  .HasForeignKey(m => m.WarehouseId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.User)
                  .WithMany(u => u.StockMovements)
                  .HasForeignKey(m => m.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(m => m.MovementType)
                  .HasConversion<string>();

            entity.Property(m => m.ScanMethod)
                  .HasConversion<string>();
        });

        // ScanLog configuration
        builder.Entity<ScanLog>(entity =>
        {
            entity.HasIndex(s => s.ScannedAt);

            entity.HasOne(s => s.User)
                  .WithMany(u => u.ScanLogs)
                  .HasForeignKey(s => s.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Product)
                  .WithMany(p => p.ScanLogs)
                  .HasForeignKey(s => s.ProductId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Seed Categories
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electrónica", Description = "Dispositivos y componentes electrónicos" },
            new Category { Id = 2, Name = "RFID Tags", Description = "Etiquetas y tags de identificación por radiofrecuencia" },
            new Category { Id = 3, Name = "Lectores", Description = "Lectores de código de barras y RFID" },
            new Category { Id = 4, Name = "Impresoras", Description = "Impresoras de etiquetas y fotochecks" },
            new Category { Id = 5, Name = "Suministros", Description = "Insumos y materiales consumibles" },
            new Category { Id = 6, Name = "Terminales", Description = "Terminales portátiles y móviles" },
            new Category { Id = 7, Name = "Biometría", Description = "Equipos de reconocimiento biométrico" },
            new Category { Id = 8, Name = "Agroindustria", Description = "Productos para el sector agroindustrial" },
            new Category { Id = 9, Name = "Retail", Description = "Soluciones para comercio mayorista y minorista" },
            new Category { Id = 10, Name = "Accesorios", Description = "Accesorios y complementos" }
        );

        // Seed Warehouses
        builder.Entity<Warehouse>().HasData(
            new Warehouse { Id = 1, Name = "Almacén Principal", Location = "San Isidro, Lima", Code = "ALM-001" },
            new Warehouse { Id = 2, Name = "Almacén Secundario", Location = "Miraflores, Lima", Code = "ALM-002" },
            new Warehouse { Id = 3, Name = "Almacén Frío", Location = "Barranco, Lima", Code = "ALM-FRIO" }
        );

        // Seed Suppliers
        builder.Entity<Supplier>().HasData(
            new Supplier { Id = 1, CompanyName = "Zebra Technologies", ContactName = "Hector Palacios", Phone = "+51 999 888 777", Email = "ventas@zebra.com", Address = "Vía Principal 133, San Isidro, Lima" },
            new Supplier { Id = 2, CompanyName = "Honeywell Perú", ContactName = "Elizabeth Myers", Phone = "+51 999 777 666", Email = "contacto@honeywell.pe", Address = "Av. Del Derby 055, San Isidro, Lima" },
            new Supplier { Id = 3, CompanyName = "Datalogic SAC", ContactName = "Astrid Taylor", Phone = "+51 999 666 555", Email = "info@datalogic.pe", Address = "Calle Las Begonias 415, San Isidro, Lima" },
            new Supplier { Id = 4, CompanyName = "SATO Global", ContactName = "Maxon Darcy", Phone = "+51 999 555 444", Email = "ventas@sato.pe", Address = "Av. Rivera Navarrete 762, San Isidro, Lima" }
        );

        // Seed Products
        builder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Terminal Móvil TC52x", SKU = "ZBR-TC52X", Barcode = "7501234567890", QRCode = "SMARTSTOCK-PRD001", Price = 2500.00m, CurrentStock = 45, MinStock = 10, CategoryId = 6, SupplierId = 1, Description = "Terminal portátil Android con lector de código de barras 2D integrado" },
            new Product { Id = 2, Name = "Lector RFID FX9600", SKU = "ZBR-FX9600", Barcode = "7501234567891", QRCode = "SMARTSTOCK-PRD002", Price = 3200.00m, CurrentStock = 12, MinStock = 5, CategoryId = 3, SupplierId = 1, Description = "Lector RFID fijo de 4 puertos para almacén" },
            new Product { Id = 3, Name = "Impresora ZT411", SKU = "ZBR-ZT411", Barcode = "7501234567892", QRCode = "SMARTSTOCK-PRD003", Price = 1800.00m, CurrentStock = 30, MinStock = 8, CategoryId = 4, SupplierId = 1, Description = "Impresora industrial de etiquetas de código de barras" },
            new Product { Id = 4, Name = "Tag RFID UHF Wet Inlay", SKU = "TAG-UHFWET", Barcode = "7501234567893", QRCode = "SMARTSTOCK-PRD004", Price = 0.35m, CurrentStock = 50000, MinStock = 10000, CategoryId = 2, SupplierId = 4, Description = "Etiqueta RFID UHF adhesiva para trazabilidad" },
            new Product { Id = 5, Name = "Lector 2D Voyager 1472g", SKU = "HNW-1472G", Barcode = "7501234567894", QRCode = "SMARTSTOCK-PRD005", Price = 450.00m, CurrentStock = 65, MinStock = 15, CategoryId = 3, SupplierId = 2, Description = "Lector inalámbrico de código de barras 2D" },
            new Product { Id = 6, Name = "Tablet Rugged ET40", SKU = "ZBR-ET40", Barcode = "7501234567895", QRCode = "SMARTSTOCK-PRD006", Price = 1600.00m, CurrentStock = 8, MinStock = 5, CategoryId = 6, SupplierId = 1, Description = "Tablet industrial Android resistente a golpes y agua" },
            new Product { Id = 7, Name = "Marcador Biométrico SF300", SKU = "BIO-SF300", Barcode = "7501234567896", QRCode = "SMARTSTOCK-PRD007", Price = 680.00m, CurrentStock = 3, MinStock = 5, CategoryId = 7, SupplierId = 3, Description = "Terminal biométrico facial para control de asistencia" },
            new Product { Id = 8, Name = "Ribbon Cera 110mm", SKU = "SUM-RIB110", Barcode = "7501234567897", QRCode = "SMARTSTOCK-PRD008", Price = 25.00m, CurrentStock = 200, MinStock = 50, CategoryId = 5, SupplierId = 4, Description = "Ribbon de cera para impresoras de etiquetas" },
            new Product { Id = 9, Name = "Antena RFID AN440", SKU = "ZBR-AN440", Barcode = "7501234567898", QRCode = "SMARTSTOCK-PRD009", Price = 320.00m, CurrentStock = 18, MinStock = 5, CategoryId = 2, SupplierId = 1, Description = "Antena RFID circular de alta ganancia" },
            new Product { Id = 10, Name = "PowerScan PD9531", SKU = "DLG-PD9531", Barcode = "7501234567899", QRCode = "SMARTSTOCK-PRD010", Price = 890.00m, CurrentStock = 22, MinStock = 10, CategoryId = 3, SupplierId = 3, Description = "Lector industrial de código de barras 2D con alta resistencia" },
            new Product { Id = 11, Name = "Impresora Fotocheck Evolis Primacy 2", SKU = "EVL-PRIM2", Barcode = "7501234567900", QRCode = "SMARTSTOCK-PRD011", Price = 2100.00m, CurrentStock = 6, MinStock = 3, CategoryId = 4, SupplierId = 2, Description = "Impresora de tarjetas PVC a doble cara" },
            new Product { Id = 12, Name = "Sensor de Proximidad IR", SKU = "SEN-PRXIR", Barcode = "7501234567901", QRCode = "SMARTSTOCK-PRD012", Price = 120.00m, CurrentStock = 150, MinStock = 30, CategoryId = 1, SupplierId = 3, Description = "Sensor infrarrojo de proximidad para automatización" },
            new Product { Id = 13, Name = "Kiosco Interactivo K200", SKU = "KSK-K200", Barcode = "7501234567902", QRCode = "SMARTSTOCK-PRD013", Price = 4500.00m, CurrentStock = 2, MinStock = 2, CategoryId = 6, SupplierId = 2, Description = "Kiosco de autoservicio con pantalla táctil 21 pulgadas" },
            new Product { Id = 14, Name = "Etiquetas Térmicas 100x50mm", SKU = "SUM-ETQ100", Barcode = "7501234567903", QRCode = "SMARTSTOCK-PRD014", Price = 15.00m, CurrentStock = 500, MinStock = 100, CategoryId = 5, SupplierId = 4, Description = "Rollo de 1000 etiquetas térmicas directas" },
            new Product { Id = 15, Name = "Access Point WiFi 6 AP650", SKU = "NET-AP650", Barcode = "7501234567904", QRCode = "SMARTSTOCK-PRD015", Price = 750.00m, CurrentStock = 25, MinStock = 8, CategoryId = 1, SupplierId = 2, Description = "Access point empresarial WiFi 6 para almacén" }
        );
    }
}

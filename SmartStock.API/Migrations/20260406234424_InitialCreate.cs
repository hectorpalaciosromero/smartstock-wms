using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartStock.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QRCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentStock = table.Column<int>(type: "int", nullable: false),
                    MinStock = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AICategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ScanLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ScanType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RawData = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DeviceInfo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WasResolved = table.Column<bool>(type: "bit", nullable: false),
                    ScannedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScanLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScanLogs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "StockMovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    MovementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ScanMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMovements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovements_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovements_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(767), "Dispositivos y componentes electrónicos", null, true, "Electrónica" },
                    { 2, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(771), "Etiquetas y tags de identificación por radiofrecuencia", null, true, "RFID Tags" },
                    { 3, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(772), "Lectores de código de barras y RFID", null, true, "Lectores" },
                    { 4, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(773), "Impresoras de etiquetas y fotochecks", null, true, "Impresoras" },
                    { 5, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(774), "Insumos y materiales consumibles", null, true, "Suministros" },
                    { 6, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(775), "Terminales portátiles y móviles", null, true, "Terminales" },
                    { 7, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(776), "Equipos de reconocimiento biométrico", null, true, "Biometría" },
                    { 8, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(777), "Productos para el sector agroindustrial", null, true, "Agroindustria" },
                    { 9, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(777), "Soluciones para comercio mayorista y minorista", null, true, "Retail" },
                    { 10, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(778), "Accesorios y complementos", null, true, "Accesorios" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Address", "CompanyName", "ContactName", "CreatedAt", "Email", "IsActive", "Phone" },
                values: new object[,]
                {
                    { 1, null, "Zebra Technologies", "Carlos Mendoza", new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1003), "ventas@zebra.com", true, "+51 999 888 777" },
                    { 2, null, "Honeywell Perú", "María García", new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1006), "contacto@honeywell.pe", true, "+51 999 777 666" },
                    { 3, null, "Datalogic SAC", "Juan Pérez", new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1008), "info@datalogic.pe", true, "+51 999 666 555" },
                    { 4, null, "SATO Global", "Ana Torres", new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1009), "ventas@sato.pe", true, "+51 999 555 444" }
                });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "Code", "CreatedAt", "IsActive", "Location", "Name" },
                values: new object[,]
                {
                    { 1, "ALM-001", new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(978), true, "San Isidro, Lima", "Almacén Principal" },
                    { 2, "ALM-002", new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(984), true, "Surco, Lima", "Almacén Secundario" },
                    { 3, "ALM-FRIO", new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(985), true, "Callao", "Almacén Frío" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AICategory", "Barcode", "CategoryId", "CreatedAt", "CurrentStock", "Description", "ImageUrl", "IsActive", "MinStock", "Name", "Price", "QRCode", "SKU", "SupplierId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, "7501234567890", 6, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1027), 45, "Terminal portátil Android con lector de código de barras 2D integrado", null, true, 10, "Terminal Móvil TC52x", 2500.00m, null, "ZBR-TC52X", 1, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1027) },
                    { 2, null, "7501234567891", 3, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1035), 12, "Lector RFID fijo de 4 puertos para almacén", null, true, 5, "Lector RFID FX9600", 3200.00m, null, "ZBR-FX9600", 1, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1035) },
                    { 3, null, "7501234567892", 4, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1038), 30, "Impresora industrial de etiquetas de código de barras", null, true, 8, "Impresora ZT411", 1800.00m, null, "ZBR-ZT411", 1, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1038) },
                    { 4, null, "7501234567893", 2, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1041), 50000, "Etiqueta RFID UHF adhesiva para trazabilidad", null, true, 10000, "Tag RFID UHF Wet Inlay", 0.35m, null, "TAG-UHFWET", 4, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1041) },
                    { 5, null, "7501234567894", 3, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1043), 65, "Lector inalámbrico de código de barras 2D", null, true, 15, "Lector 2D Voyager 1472g", 450.00m, null, "HNW-1472G", 2, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1043) },
                    { 6, null, "7501234567895", 6, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1045), 8, "Tablet industrial Android resistente a golpes y agua", null, true, 5, "Tablet Rugged ET40", 1600.00m, null, "ZBR-ET40", 1, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1046) },
                    { 7, null, "7501234567896", 7, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1048), 3, "Terminal biométrico facial para control de asistencia", null, true, 5, "Marcador Biométrico SF300", 680.00m, null, "BIO-SF300", 3, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1048) },
                    { 8, null, "7501234567897", 5, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1050), 200, "Ribbon de cera para impresoras de etiquetas", null, true, 50, "Ribbon Cera 110mm", 25.00m, null, "SUM-RIB110", 4, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1050) },
                    { 9, null, "7501234567898", 2, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1052), 18, "Antena RFID circular de alta ganancia", null, true, 5, "Antena RFID AN440", 320.00m, null, "ZBR-AN440", 1, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1052) },
                    { 10, null, "7501234567899", 3, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1054), 22, "Lector industrial de código de barras 2D con alta resistencia", null, true, 10, "PowerScan PD9531", 890.00m, null, "DLG-PD9531", 3, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1054) },
                    { 11, null, "7501234567900", 4, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1056), 6, "Impresora de tarjetas PVC a doble cara", null, true, 3, "Impresora Fotocheck Evolis Primacy 2", 2100.00m, null, "EVL-PRIM2", 2, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1057) },
                    { 12, null, "7501234567901", 1, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1059), 150, "Sensor infrarrojo de proximidad para automatización", null, true, 30, "Sensor de Proximidad IR", 120.00m, null, "SEN-PRXIR", 3, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1059) },
                    { 13, null, "7501234567902", 6, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1061), 2, "Kiosco de autoservicio con pantalla táctil 21 pulgadas", null, true, 2, "Kiosco Interactivo K200", 4500.00m, null, "KSK-K200", 2, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1061) },
                    { 14, null, "7501234567903", 5, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1063), 500, "Rollo de 1000 etiquetas térmicas directas", null, true, 100, "Etiquetas Térmicas 100x50mm", 15.00m, null, "SUM-ETQ100", 4, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1063) },
                    { 15, null, "7501234567904", 1, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1114), 25, "Access point empresarial WiFi 6 para almacén", null, true, 8, "Access Point WiFi 6 AP650", 750.00m, null, "NET-AP650", 2, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1114) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Barcode",
                table: "Products",
                column: "Barcode");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_QRCode",
                table: "Products",
                column: "QRCode");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU",
                unique: true,
                filter: "[SKU] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanLogs_ProductId",
                table: "ScanLogs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanLogs_ScannedAt",
                table: "ScanLogs",
                column: "ScannedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ScanLogs_UserId",
                table: "ScanLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_CreatedAt",
                table: "StockMovements",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ProductId",
                table: "StockMovements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_UserId",
                table: "StockMovements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_WarehouseId",
                table: "StockMovements",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ScanLogs");

            migrationBuilder.DropTable(
                name: "StockMovements");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}

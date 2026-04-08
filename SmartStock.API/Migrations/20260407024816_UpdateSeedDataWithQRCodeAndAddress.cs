using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStock.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedDataWithQRCodeAndAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5525));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5528));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5529));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5530));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5558));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5558));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5560));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5561));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5562));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5563));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5722), "SMARTSTOCK-PRD001", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5722) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5728), "SMARTSTOCK-PRD002", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5729) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5731), "SMARTSTOCK-PRD003", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5731) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5733), "SMARTSTOCK-PRD004", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5733) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5735), "SMARTSTOCK-PRD005", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5735) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5737), "SMARTSTOCK-PRD006", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5737) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5739), "SMARTSTOCK-PRD007", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5739) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5741), "SMARTSTOCK-PRD008", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5742) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5743), "SMARTSTOCK-PRD009", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5744) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5745), "SMARTSTOCK-PRD010", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5746) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5748), "SMARTSTOCK-PRD011", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5748) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5750), "SMARTSTOCK-PRD012", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5750) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5752), "SMARTSTOCK-PRD013", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5752) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5755), "SMARTSTOCK-PRD014", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5755) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5759), "SMARTSTOCK-PRD015", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5760) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "CreatedAt" },
                values: new object[] { "Av. Javier Prado Este 4600, Santiago de Surco, Lima", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5695) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "CreatedAt" },
                values: new object[] { "Av. Del Derby 055, Santiago de Surco, Lima", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5699) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Address", "CreatedAt" },
                values: new object[] { "Calle Las Begonias 415, San Isidro, Lima", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5701) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Address", "CreatedAt" },
                values: new object[] { "Av. Rivera Navarrete 762, San Isidro, Lima", new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5703) });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5674));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5678));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5680));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(767));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(771));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(772));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(773));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(774));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(775));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(776));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(777));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(777));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(778));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1027), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1027) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1035), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1035) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1038), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1038) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1041), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1041) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1043), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1043) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1045), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1046) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1048), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1048) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1050), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1050) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1052), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1052) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1054), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1054) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1056), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1057) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1059), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1059) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1061), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1061) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1063), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1063) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "QRCode", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1114), null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1114) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1003) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1006) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Address", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1008) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Address", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(1009) });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(978));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(984));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 23, 44, 23, 740, DateTimeKind.Utc).AddTicks(985));
        }
    }
}

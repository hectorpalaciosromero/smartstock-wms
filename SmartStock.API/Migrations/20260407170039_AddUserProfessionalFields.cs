using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStock.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfessionalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 984, DateTimeKind.Utc).AddTicks(9756));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 984, DateTimeKind.Utc).AddTicks(9760));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 984, DateTimeKind.Utc).AddTicks(9761));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 984, DateTimeKind.Utc).AddTicks(9762));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 984, DateTimeKind.Utc).AddTicks(9763));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 984, DateTimeKind.Utc).AddTicks(9764));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 984, DateTimeKind.Utc).AddTicks(9765));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 984, DateTimeKind.Utc).AddTicks(9767));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 984, DateTimeKind.Utc).AddTicks(9768));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 984, DateTimeKind.Utc).AddTicks(9799));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(528), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(528) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(537), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(537) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(540), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(540) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(542), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(542) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(544), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(545) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(546), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(547) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(549), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(549) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(551), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(551) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(553), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(553) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(555), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(555) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(557), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(557) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(559), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(559) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(561), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(561) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(564), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(564) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(567), new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(567) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(497));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(500));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(501));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(504));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(470));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(476));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(478));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "AspNetUsers");

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
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5722), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5722) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5728), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5729) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5731), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5731) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5733), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5733) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5735), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5735) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5737), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5737) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5739), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5739) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5741), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5742) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5743), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5744) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5745), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5746) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5748), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5748) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5750), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5750) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5752), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5752) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5755), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5755) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5759), new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5760) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5695));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5699));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5701));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 2, 48, 16, 322, DateTimeKind.Utc).AddTicks(5703));

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
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStock.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupplierNamesAndAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3492));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3497));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3498));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3499));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3500));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3536));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3538));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3540));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3541));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3542));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3797), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3798) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3804), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3804) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3807), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3807) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3809), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3809) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3811), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3812) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3813), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3814) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3816), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3816) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3818), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3818) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3821), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3821) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3823), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3823) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3825), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3825) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3827), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3827) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3829), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3829) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3834), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3834) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3839), new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3839) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "ContactName", "CreatedAt" },
                values: new object[] { "Av. Javier Prado Este 4600, San Isidro, Lima", "Maxon Darcy", new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3769) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "ContactName", "CreatedAt" },
                values: new object[] { "Av. Del Derby 055, San Isidro, Lima", "Elizabeth Myers", new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3772) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ContactName", "CreatedAt" },
                values: new object[] { "Astrid Taylor", new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3774) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ContactName", "CreatedAt" },
                values: new object[] { "Hector Palacios", new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3777) });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3741));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3749));

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 1, 13, 8, 713, DateTimeKind.Utc).AddTicks(3750));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "Address", "ContactName", "CreatedAt" },
                values: new object[] { "Av. Javier Prado Este 4600, Santiago de Surco, Lima", "Carlos Mendoza", new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(497) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "ContactName", "CreatedAt" },
                values: new object[] { "Av. Del Derby 055, Santiago de Surco, Lima", "María García", new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(500) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ContactName", "CreatedAt" },
                values: new object[] { "Juan Pérez", new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(501) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ContactName", "CreatedAt" },
                values: new object[] { "Ana Torres", new DateTime(2026, 4, 7, 17, 0, 38, 985, DateTimeKind.Utc).AddTicks(504) });

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
    }
}

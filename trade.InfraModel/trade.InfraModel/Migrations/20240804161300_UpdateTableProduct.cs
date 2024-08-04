using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace trade.InfraModel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("58f8d9e2-a1e4-4555-a5a4-68b4707388ae"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aa49ea13-022d-4437-8534-1753207eacc2"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f7182ab4-eec8-4e96-966e-9ab7c66a0f8a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0c53faa2-ee7d-4cca-be17-ac9f751deb77"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cc942bb7-690a-4dc1-afb0-d78182e6a825"));

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0c53faa2-ee7d-4cca-be17-ac9f751deb77"), "Gaming", new DateTime(2024, 8, 2, 20, 48, 14, 428, DateTimeKind.Utc).AddTicks(1796), null, null, false, null },
                    { new Guid("cc942bb7-690a-4dc1-afb0-d78182e6a825"), "Social Media", new DateTime(2024, 8, 2, 20, 48, 14, 428, DateTimeKind.Utc).AddTicks(1792), null, null, false, null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "CreatedBy", "DeletedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("58f8d9e2-a1e4-4555-a5a4-68b4707388ae"), new Guid("cc942bb7-690a-4dc1-afb0-d78182e6a825"), new DateTime(2024, 8, 2, 20, 48, 14, 428, DateTimeKind.Utc).AddTicks(2220), null, null, false, "Facebook Account", null },
                    { new Guid("aa49ea13-022d-4437-8534-1753207eacc2"), new Guid("cc942bb7-690a-4dc1-afb0-d78182e6a825"), new DateTime(2024, 8, 2, 20, 48, 14, 428, DateTimeKind.Utc).AddTicks(2223), null, null, false, "TikTok Account", null },
                    { new Guid("f7182ab4-eec8-4e96-966e-9ab7c66a0f8a"), new Guid("0c53faa2-ee7d-4cca-be17-ac9f751deb77"), new DateTime(2024, 8, 2, 20, 48, 14, 428, DateTimeKind.Utc).AddTicks(2226), null, null, false, "Garena Account", null }
                });
        }
    }
}

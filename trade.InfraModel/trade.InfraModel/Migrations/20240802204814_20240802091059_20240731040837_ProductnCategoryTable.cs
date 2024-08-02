using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace trade.InfraModel.Migrations
{
    /// <inheritdoc />
    public partial class _20240802091059_20240731040837_ProductnCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryName = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}

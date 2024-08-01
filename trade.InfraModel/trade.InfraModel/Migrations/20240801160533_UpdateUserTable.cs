using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trade.InfraModel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("300430fe-acdc-4f61-ab6c-8a746a78aa6c"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "Name", "PassWordHash", "Role", "UpdatedAt" },
                values: new object[] { new Guid("300430fe-acdc-4f61-ab6c-8a746a78aa6c"), new DateTime(2024, 8, 1, 16, 2, 35, 189, DateTimeKind.Utc).AddTicks(9793), new Guid("4bc50d5c-a97f-4f8a-a282-d63d3d878e03"), null, "su@trade.vn", null, "123 123", 1, null });
        }
    }
}

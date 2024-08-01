using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trade.InfraModel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("772c55dd-461f-451d-b093-06596b6d85af"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "Name", "PassWordHash", "Role", "UpdatedAt" },
                values: new object[] { new Guid("300430fe-acdc-4f61-ab6c-8a746a78aa6c"), new DateTime(2024, 8, 1, 16, 2, 35, 189, DateTimeKind.Utc).AddTicks(9793), new Guid("4bc50d5c-a97f-4f8a-a282-d63d3d878e03"), null, "su@trade.vn", null, "123 123", 1, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("300430fe-acdc-4f61-ab6c-8a746a78aa6c"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Email", "PassWordHash", "Role", "UpdatedAt" },
                values: new object[] { new Guid("772c55dd-461f-451d-b093-06596b6d85af"), new DateTime(2024, 7, 31, 4, 8, 32, 789, DateTimeKind.Utc).AddTicks(2790), new Guid("8b844221-ef00-4f3a-b466-4be476b52f52"), null, "su@trade.vn", "123 123", 1, null });
        }
    }
}

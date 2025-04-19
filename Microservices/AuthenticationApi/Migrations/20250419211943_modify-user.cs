using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthenticationApi.Migrations
{
    /// <inheritdoc />
    public partial class modifyuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("7bd971e7-0e36-4e03-9e54-be3b6a133d9d"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("fb544946-a4a7-47af-80ea-2009f4cf1a95"));

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2ac28042-2400-4af9-b605-73595764a89e"), "User" },
                    { new Guid("50783243-c0b2-4a1e-80a5-1181bedccbf5"), "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2ac28042-2400-4af9-b605-73595764a89e"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("50783243-c0b2-4a1e-80a5-1181bedccbf5"));

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("7bd971e7-0e36-4e03-9e54-be3b6a133d9d"), "Admin" },
                    { new Guid("fb544946-a4a7-47af-80ea-2009f4cf1a95"), "User" }
                });
        }
    }
}

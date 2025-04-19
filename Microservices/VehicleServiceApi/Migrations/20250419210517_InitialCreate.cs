using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VehicleServiceApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DailyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "DailyRate", "IsAvailable", "LicensePlate", "Location", "Make", "Model", "Type", "Year" },
                values: new object[,]
                {
                    { new Guid("57bd4a67-2136-49b4-9e9c-56d1ce150c33"), 80.00m, true, "LMN9876", "Chicago", "Ford", "F-150", "Truck", 2022 },
                    { new Guid("70bfc2b0-cdf6-48ca-a7bd-ab74c4352d90"), 55.00m, true, "XYZ5678", "Los Angeles", "Honda", "Civic", "Sedan", 2021 },
                    { new Guid("75f04c79-6d03-4ecd-8da2-f38653441dfb"), 50.00m, true, "ABC1234", "New York", "Toyota", "Corolla", "Sedan", 2020 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}

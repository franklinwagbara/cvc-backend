using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class newMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OfficerID",
                table: "PlantFieldOfficers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "vPlantFieldOfficers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    OfficerID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepotName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vPlantFieldOfficers", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlantFieldOfficers_OfficerID",
                table: "PlantFieldOfficers",
                column: "OfficerID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantFieldOfficers_AspNetUsers_OfficerID",
                table: "PlantFieldOfficers",
                column: "OfficerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantFieldOfficers_AspNetUsers_OfficerID",
                table: "PlantFieldOfficers");

            migrationBuilder.DropTable(
                name: "vPlantFieldOfficers");

            migrationBuilder.DropIndex(
                name: "IX_PlantFieldOfficers_OfficerID",
                table: "PlantFieldOfficers");

            migrationBuilder.AlterColumn<Guid>(
                name: "OfficerID",
                table: "PlantFieldOfficers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Migrations
{
    /// <inheritdoc />
    public partial class facsource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppFees_FacilityTypes_FacilityTypeId",
                table: "AppFees");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_FacilityTypes_FacilityTypeId",
                table: "Facilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_LGAs_LgaId",
                table: "Facilities");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityTypeDocuments_FacilityTypes_FacilityTypeId",
                table: "FacilityTypeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_FacilityTypeDocuments_FacilityTypeId",
                table: "FacilityTypeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_Facilities_FacilityTypeId",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "FacilityTypeId",
                table: "Facilities");

            migrationBuilder.RenameColumn(
                name: "FacilityTypeId",
                table: "WorkFlows",
                newName: "VesselTypeId");

            migrationBuilder.RenameColumn(
                name: "FacilityTypeId",
                table: "FacilityTypeDocuments",
                newName: "VessleTypeId");

            migrationBuilder.RenameColumn(
                name: "LgaId",
                table: "Facilities",
                newName: "VesselTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Facilities_LgaId",
                table: "Facilities",
                newName: "IX_Facilities_VesselTypeId");

            migrationBuilder.RenameColumn(
                name: "FacilityTypeId",
                table: "AppFees",
                newName: "VesseltypeId");

            migrationBuilder.RenameIndex(
                name: "IX_AppFees_FacilityTypeId",
                table: "AppFees",
                newName: "IX_AppFees_VesseltypeId");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationTypeId",
                table: "WorkFlows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "WorkFlows",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VesselTypeId",
                table: "FacilityTypeDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Capacity",
                table: "Facilities",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DeadWeight",
                table: "Facilities",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FacilitySources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    LgaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitySources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VesselTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VesselTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilityTypeDocuments_VesselTypeId",
                table: "FacilityTypeDocuments",
                column: "VesselTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppFees_VesselTypes_VesseltypeId",
                table: "AppFees",
                column: "VesseltypeId",
                principalTable: "VesselTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_VesselTypes_VesselTypeId",
                table: "Facilities",
                column: "VesselTypeId",
                principalTable: "VesselTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityTypeDocuments_VesselTypes_VesselTypeId",
                table: "FacilityTypeDocuments",
                column: "VesselTypeId",
                principalTable: "VesselTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppFees_VesselTypes_VesseltypeId",
                table: "AppFees");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_VesselTypes_VesselTypeId",
                table: "Facilities");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityTypeDocuments_VesselTypes_VesselTypeId",
                table: "FacilityTypeDocuments");

            migrationBuilder.DropTable(
                name: "FacilitySources");

            migrationBuilder.DropTable(
                name: "VesselTypes");

            migrationBuilder.DropIndex(
                name: "IX_FacilityTypeDocuments_VesselTypeId",
                table: "FacilityTypeDocuments");

            migrationBuilder.DropColumn(
                name: "ApplicationTypeId",
                table: "WorkFlows");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "WorkFlows");

            migrationBuilder.DropColumn(
                name: "VesselTypeId",
                table: "FacilityTypeDocuments");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "DeadWeight",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "VesselTypeId",
                table: "WorkFlows",
                newName: "FacilityTypeId");

            migrationBuilder.RenameColumn(
                name: "VessleTypeId",
                table: "FacilityTypeDocuments",
                newName: "FacilityTypeId");

            migrationBuilder.RenameColumn(
                name: "VesselTypeId",
                table: "Facilities",
                newName: "LgaId");

            migrationBuilder.RenameIndex(
                name: "IX_Facilities_VesselTypeId",
                table: "Facilities",
                newName: "IX_Facilities_LgaId");

            migrationBuilder.RenameColumn(
                name: "VesseltypeId",
                table: "AppFees",
                newName: "FacilityTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_AppFees_VesseltypeId",
                table: "AppFees",
                newName: "IX_AppFees_FacilityTypeId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Facilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FacilityTypeId",
                table: "Facilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FacilityTypeDocuments_FacilityTypeId",
                table: "FacilityTypeDocuments",
                column: "FacilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_FacilityTypeId",
                table: "Facilities",
                column: "FacilityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppFees_FacilityTypes_FacilityTypeId",
                table: "AppFees",
                column: "FacilityTypeId",
                principalTable: "FacilityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_FacilityTypes_FacilityTypeId",
                table: "Facilities",
                column: "FacilityTypeId",
                principalTable: "FacilityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_LGAs_LgaId",
                table: "Facilities",
                column: "LgaId",
                principalTable: "LGAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityTypeDocuments_FacilityTypes_FacilityTypeId",
                table: "FacilityTypeDocuments",
                column: "FacilityTypeId",
                principalTable: "FacilityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

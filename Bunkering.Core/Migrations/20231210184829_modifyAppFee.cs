using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class modifyAppFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppFees_ApplicationTypes_ApplicationTypeId",
                table: "AppFees");

            migrationBuilder.DropForeignKey(
                name: "FK_AppFees_VesselTypes_VesseltypeId",
                table: "AppFees");

            migrationBuilder.DropIndex(
                name: "IX_AppFees_ApplicationTypeId",
                table: "AppFees");

            migrationBuilder.DropIndex(
                name: "IX_AppFees_VesseltypeId",
                table: "AppFees");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "AccreditationFee",
                table: "AppFees");

            migrationBuilder.DropColumn(
                name: "VesseltypeId",
                table: "AppFees");

            migrationBuilder.RenameColumn(
                name: "VesselLicenseFee",
                table: "AppFees",
                newName: "ProcessingFee");

            migrationBuilder.RenameColumn(
                name: "InspectionFee",
                table: "AppFees",
                newName: "NOAFee");

            migrationBuilder.RenameColumn(
                name: "AdministrativeFee",
                table: "AppFees",
                newName: "COQFee");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppFees",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppFees");

            migrationBuilder.RenameColumn(
                name: "ProcessingFee",
                table: "AppFees",
                newName: "VesselLicenseFee");

            migrationBuilder.RenameColumn(
                name: "NOAFee",
                table: "AppFees",
                newName: "InspectionFee");

            migrationBuilder.RenameColumn(
                name: "COQFee",
                table: "AppFees",
                newName: "AdministrativeFee");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AccreditationFee",
                table: "AppFees",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "VesseltypeId",
                table: "AppFees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppFees_ApplicationTypeId",
                table: "AppFees",
                column: "ApplicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppFees_VesseltypeId",
                table: "AppFees",
                column: "VesseltypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppFees_ApplicationTypes_ApplicationTypeId",
                table: "AppFees",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppFees_VesselTypes_VesseltypeId",
                table: "AppFees",
                column: "VesseltypeId",
                principalTable: "VesselTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

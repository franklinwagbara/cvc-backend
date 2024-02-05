using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class modifyplantCoq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SumDiffLongTonsAir",
                table: "ProcessingPlantCOQTanks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SumDiffMCubeAt15Degree",
                table: "ProcessingPlantCOQTanks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SumDiffMTAir",
                table: "ProcessingPlantCOQTanks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SumDiffMTVac",
                table: "ProcessingPlantCOQTanks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SumDiffUsBarrelsAt15Degree",
                table: "ProcessingPlantCOQTanks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProcessingPlantCOQS",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProcessingPlantCOQS",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DeliveredLongTonsAir",
                table: "ProcessingPlantCOQS",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeliveredMCubeAt15Degree",
                table: "ProcessingPlantCOQS",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeliveredMTAir",
                table: "ProcessingPlantCOQS",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeliveredMTVac",
                table: "ProcessingPlantCOQS",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeliveredUsBarrelsAt15Degree",
                table: "ProcessingPlantCOQS",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "ProcessingPlantCOQS",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ProcessingPlantCOQS",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumDiffLongTonsAir",
                table: "ProcessingPlantCOQTanks");

            migrationBuilder.DropColumn(
                name: "SumDiffMCubeAt15Degree",
                table: "ProcessingPlantCOQTanks");

            migrationBuilder.DropColumn(
                name: "SumDiffMTAir",
                table: "ProcessingPlantCOQTanks");

            migrationBuilder.DropColumn(
                name: "SumDiffMTVac",
                table: "ProcessingPlantCOQTanks");

            migrationBuilder.DropColumn(
                name: "SumDiffUsBarrelsAt15Degree",
                table: "ProcessingPlantCOQTanks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProcessingPlantCOQS");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProcessingPlantCOQS");

            migrationBuilder.DropColumn(
                name: "DeliveredLongTonsAir",
                table: "ProcessingPlantCOQS");

            migrationBuilder.DropColumn(
                name: "DeliveredMCubeAt15Degree",
                table: "ProcessingPlantCOQS");

            migrationBuilder.DropColumn(
                name: "DeliveredMTAir",
                table: "ProcessingPlantCOQS");

            migrationBuilder.DropColumn(
                name: "DeliveredMTVac",
                table: "ProcessingPlantCOQS");

            migrationBuilder.DropColumn(
                name: "DeliveredUsBarrelsAt15Degree",
                table: "ProcessingPlantCOQS");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "ProcessingPlantCOQS");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProcessingPlantCOQS");
        }
    }
}

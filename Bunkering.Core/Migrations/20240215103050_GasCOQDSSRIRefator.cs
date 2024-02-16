using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class GasCOQDSSRIRefator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "ExtraPayments");

            migrationBuilder.AddColumn<double>(
                name: "CorrectedLiquidLevel",
                table: "TankMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CorrectedLiquidVolumeM3",
                table: "TankMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CorrectedVapourVolume",
                table: "TankMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "GrossStandardVolumeGas",
                table: "TankMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LiquidDensityAir",
                table: "TankMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LiquidWeightAir",
                table: "TankMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LiquidWeightVAC",
                table: "TankMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalGasWeightAir",
                table: "TankMeasurements",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalGasWeightVAC",
                table: "TankMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VapourVolume",
                table: "TankMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VapourWeightAir",
                table: "TankMeasurements",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VapourWeightVAC",
                table: "TankMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsCleared",
            //    table: "Plants",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsDefaulter",
            //    table: "Plants",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<double>(
            //    name: "GrandTotalWeightKg",
            //    table: "CoQs",
            //    type: "float",
            //    nullable: false,
            //    defaultValue: 0.0);

            //migrationBuilder.AddColumn<double>(
            //    name: "ShoreFigureMTAirGas",
            //    table: "CoQs",
            //    type: "float",
            //    nullable: false,
            //    defaultValue: 0.0);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsCleared",
            //    table: "AspNetUsers",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.CreateTable(
            //    name: "DemandNotices",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        DebitNoteId = table.Column<int>(type: "int", nullable: false),
            //        Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Amount = table.Column<double>(type: "float", nullable: false),
            //        AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Paid = table.Column<bool>(type: "bit", nullable: false),
            //        PaymentId = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_DemandNotices", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_DemandNotices_Payments_PaymentId",
            //            column: x => x.PaymentId,
            //            principalTable: "Payments",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_DemandNotices_PaymentId",
            //    table: "DemandNotices",
            //    column: "PaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemandNotices");

            migrationBuilder.DropColumn(
                name: "CorrectedLiquidLevel",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "CorrectedLiquidVolumeM3",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "CorrectedVapourVolume",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "GrossStandardVolumeGas",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "LiquidDensityAir",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "LiquidWeightAir",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "LiquidWeightVAC",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "TotalGasWeightAir",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "TotalGasWeightVAC",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "VapourVolume",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "VapourWeightAir",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "VapourWeightVAC",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "IsCleared",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "IsDefaulter",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "GrandTotalWeightKg",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "ShoreFigureMTAirGas",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "IsCleared",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ExtraPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraPayments", x => x.Id);
                });
        }
    }
}

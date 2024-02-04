using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class ModifyProcessinplantCOQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterReadings");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQLiquidDynamicReadings");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQLiquidDynamics");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "Meters",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQLiquidDynamicBatches",
                columns: table => new
                {
                    ProcessingPlantCOQLiquidDynamicBatchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQId = table.Column<int>(type: "int", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    SumDiffMCubeAt15Degree = table.Column<double>(type: "float", nullable: true),
                    SumDiffUsBarrelsAt15Degree = table.Column<double>(type: "float", nullable: true),
                    SumDiffMTVac = table.Column<double>(type: "float", nullable: true),
                    SumDiffMTAir = table.Column<double>(type: "float", nullable: true),
                    SumDiffLongTonsAir = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQLiquidDynamicBatches", x => x.ProcessingPlantCOQLiquidDynamicBatchId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQLiquidDynamicMeters",
                columns: table => new
                {
                    ProcessingPlantCOQLiquidDynamicMeterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQLiquidDynamicBatchId = table.Column<int>(type: "int", nullable: false),
                    MeterId = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Density = table.Column<double>(type: "float", nullable: false),
                    MeterFactor = table.Column<double>(type: "float", nullable: false),
                    Ctl = table.Column<double>(type: "float", nullable: false),
                    Cpl = table.Column<double>(type: "float", nullable: false),
                    WTAir = table.Column<double>(type: "float", nullable: false),
                    MCubeAt15Degree = table.Column<double>(type: "float", nullable: false),
                    UsBarrelsAt15Degree = table.Column<double>(type: "float", nullable: false),
                    MTVac = table.Column<double>(type: "float", nullable: false),
                    MTAir = table.Column<double>(type: "float", nullable: false),
                    LongTonsAir = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQLiquidDynamicMeters", x => x.ProcessingPlantCOQLiquidDynamicMeterId);
                    table.ForeignKey(
                        name: "FK_ProcessingPlantCOQLiquidDynamicMeters_ProcessingPlantCOQLiquidDynamicBatches_ProcessingPlantCOQLiquidDynamicBatchId",
                        column: x => x.ProcessingPlantCOQLiquidDynamicBatchId,
                        principalTable: "ProcessingPlantCOQLiquidDynamicBatches",
                        principalColumn: "ProcessingPlantCOQLiquidDynamicBatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiquidDynamicMeterReadings",
                columns: table => new
                {
                    LiquidDynamicMeterReadingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQLiquidDynamicMeterId = table.Column<int>(type: "int", nullable: false),
                    MeasurementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MCube = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidDynamicMeterReadings", x => x.LiquidDynamicMeterReadingId);
                    table.ForeignKey(
                        name: "FK_LiquidDynamicMeterReadings_ProcessingPlantCOQLiquidDynamicMeters_ProcessingPlantCOQLiquidDynamicMeterId",
                        column: x => x.ProcessingPlantCOQLiquidDynamicMeterId,
                        principalTable: "ProcessingPlantCOQLiquidDynamicMeters",
                        principalColumn: "ProcessingPlantCOQLiquidDynamicMeterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiquidDynamicMeterReadings_ProcessingPlantCOQLiquidDynamicMeterId",
                table: "LiquidDynamicMeterReadings",
                column: "ProcessingPlantCOQLiquidDynamicMeterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingPlantCOQLiquidDynamicMeters_ProcessingPlantCOQLiquidDynamicBatchId",
                table: "ProcessingPlantCOQLiquidDynamicMeters",
                column: "ProcessingPlantCOQLiquidDynamicBatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiquidDynamicMeterReadings");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQLiquidDynamicMeters");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQLiquidDynamicBatches");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "Meters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MeterReadings",
                columns: table => new
                {
                    MeterReadingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MCube = table.Column<double>(type: "float", nullable: false),
                    ProcessingPlantCOQLiquidDynamicReadingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadings", x => x.MeterReadingId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQLiquidDynamics",
                columns: table => new
                {
                    ProcessingPlantCOQLiquidDynamicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeterId = table.Column<int>(type: "int", nullable: false),
                    ProcessingPlantCOQId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQLiquidDynamics", x => x.ProcessingPlantCOQLiquidDynamicId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQLiquidDynamicReadings",
                columns: table => new
                {
                    ProcessingPlantCOQLiquidDynamicReadingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Batch = table.Column<int>(type: "int", nullable: false),
                    Cpl = table.Column<double>(type: "float", nullable: false),
                    Ctl = table.Column<double>(type: "float", nullable: false),
                    Density = table.Column<double>(type: "float", nullable: false),
                    LongTonsAir = table.Column<double>(type: "float", nullable: false),
                    MCubeAt15Degree = table.Column<double>(type: "float", nullable: false),
                    MTAir = table.Column<double>(type: "float", nullable: false),
                    MTVac = table.Column<double>(type: "float", nullable: false),
                    MeterFactor = table.Column<double>(type: "float", nullable: false),
                    ProcessingPlantCOQLiquidDynamicId = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    UsBarrelsAt15Degree = table.Column<double>(type: "float", nullable: false),
                    WTAir = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQLiquidDynamicReadings", x => x.ProcessingPlantCOQLiquidDynamicReadingId);
                    table.ForeignKey(
                        name: "FK_ProcessingPlantCOQLiquidDynamicReadings_ProcessingPlantCOQLiquidDynamics_ProcessingPlantCOQLiquidDynamicId",
                        column: x => x.ProcessingPlantCOQLiquidDynamicId,
                        principalTable: "ProcessingPlantCOQLiquidDynamics",
                        principalColumn: "ProcessingPlantCOQLiquidDynamicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingPlantCOQLiquidDynamicReadings_ProcessingPlantCOQLiquidDynamicId",
                table: "ProcessingPlantCOQLiquidDynamicReadings",
                column: "ProcessingPlantCOQLiquidDynamicId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class latestmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COQTanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoQId = table.Column<int>(type: "int", nullable: false),
                    TankName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COQTanks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TankMeasurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COQTankId = table.Column<int>(type: "int", nullable: false),
                    MeasurementTypeId = table.Column<int>(type: "int", nullable: false),
                    DIP = table.Column<double>(type: "float", nullable: false),
                    WaterDIP = table.Column<double>(type: "float", nullable: false),
                    TOV = table.Column<double>(type: "float", nullable: false),
                    WaterVolume = table.Column<double>(type: "float", nullable: false),
                    FloatRoofCorr = table.Column<double>(type: "float", nullable: false),
                    GOV = table.Column<double>(type: "float", nullable: false),
                    Tempearture = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Density = table.Column<double>(type: "float", nullable: false),
                    VCF = table.Column<double>(type: "float", nullable: false),
                    GSV = table.Column<double>(type: "float", nullable: false),
                    MTVAC = table.Column<double>(type: "float", nullable: false),
                    LiquidDensityAir = table.Column<double>(type: "float", nullable: false),
                    ObservedSounding = table.Column<double>(type: "float", nullable: false),
                    TapeCorrection = table.Column<double>(type: "float", nullable: false),
                    CorrectedLiquidVolume = table.Column<double>(type: "float", nullable: false),
                    ObservedLiquidVolume = table.Column<double>(type: "float", nullable: false),
                    ShrinkageFactorLiquid = table.Column<double>(type: "float", nullable: false),
                    CorrectedLiquidVolumeM3 = table.Column<double>(type: "float", nullable: false),
                    LiquidWeightVAC = table.Column<double>(type: "float", nullable: false),
                    LiquidWeightAir = table.Column<double>(type: "float", nullable: false),
                    TankVolume = table.Column<double>(type: "float", nullable: false),
                    VapourVolume = table.Column<double>(type: "float", nullable: false),
                    ShrinkageFactorVapour = table.Column<double>(type: "float", nullable: false),
                    CorrectedVapourVolume = table.Column<double>(type: "float", nullable: false),
                    VapourTemperature = table.Column<double>(type: "float", nullable: false),
                    VapourPressure = table.Column<double>(type: "float", nullable: false),
                    MolecularWeight = table.Column<double>(type: "float", nullable: false),
                    VapourFactor = table.Column<double>(type: "float", nullable: false),
                    VapourWeightVAC = table.Column<double>(type: "float", nullable: false),
                    VapourWeightAir = table.Column<double>(type: "float", nullable: false),
                    TotalGasWeightVAC = table.Column<double>(type: "float", nullable: false),
                    TotalGasWeightAir = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TankMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TankMeasurements_COQTanks_COQTankId",
                        column: x => x.COQTankId,
                        principalTable: "COQTanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TankMeasurements_COQTankId",
                table: "TankMeasurements",
                column: "COQTankId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeasurementTypes");

            migrationBuilder.DropTable(
                name: "TankMeasurements");

            migrationBuilder.DropTable(
                name: "COQTanks");
        }
    }
}

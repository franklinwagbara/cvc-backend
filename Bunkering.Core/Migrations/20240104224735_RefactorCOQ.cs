using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCOQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectedLiquidVolume",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "CorrectedLiquidVolumeM3",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "CorrectedVapourVolume",
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
                name: "MeasurementTypeId",
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
                name: "TankName",
                table: "COQTanks");

            migrationBuilder.DropColumn(
                name: "IMONumber",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "VapourWeightVAC",
                table: "TankMeasurements",
                newName: "LiquidDensityVac");

            migrationBuilder.AddColumn<string>(
                name: "MeasurementType",
                table: "TankMeasurements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TankId",
                table: "COQTanks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "MT_VAC",
                table: "CoQs",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "MT_AIR",
                table: "CoQs",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "GSV",
                table: "CoQs",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "GOV",
                table: "CoQs",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "DepotPrice",
                table: "CoQs",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<double>(
                name: "ArrivalShipFigure",
                table: "CoQs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DifferenceBtwShipAndShoreFigure",
                table: "CoQs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DischargeShipFigure",
                table: "CoQs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageDifference",
                table: "CoQs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PlantId",
                table: "CoQs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "QuauntityReflectedOnBill",
                table: "CoQs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElpsPlantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyElpsId = table.Column<long>(type: "bigint", nullable: false),
                    PlantType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlantTanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    TankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantTanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantTanks_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlantTanks_PlantId",
                table: "PlantTanks",
                column: "PlantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlantTanks");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropColumn(
                name: "MeasurementType",
                table: "TankMeasurements");

            migrationBuilder.DropColumn(
                name: "TankId",
                table: "COQTanks");

            migrationBuilder.DropColumn(
                name: "ArrivalShipFigure",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "DifferenceBtwShipAndShoreFigure",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "DischargeShipFigure",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "PercentageDifference",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "PlantId",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "QuauntityReflectedOnBill",
                table: "CoQs");

            migrationBuilder.RenameColumn(
                name: "LiquidDensityVac",
                table: "TankMeasurements",
                newName: "VapourWeightVAC");

            migrationBuilder.AddColumn<double>(
                name: "CorrectedLiquidVolume",
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

            migrationBuilder.AddColumn<int>(
                name: "MeasurementTypeId",
                table: "TankMeasurements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalGasWeightAir",
                table: "TankMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "TankName",
                table: "COQTanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "MT_VAC",
                table: "CoQs",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "MT_AIR",
                table: "CoQs",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "GSV",
                table: "CoQs",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "GOV",
                table: "CoQs",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "DepotPrice",
                table: "CoQs",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "IMONumber",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

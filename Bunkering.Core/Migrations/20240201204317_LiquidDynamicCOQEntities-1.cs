using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class LiquidDynamicCOQEntities1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeterReadings",
                columns: table => new
                {
                    MeterReadingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQLiquidDynamicReadingId = table.Column<int>(type: "int", nullable: false),
                    MCube = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadings", x => x.MeterReadingId);
                });

            migrationBuilder.CreateTable(
                name: "Meters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQLiquidDynamics",
                columns: table => new
                {
                    ProcessingPlantCOQLiquidDynamicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQId = table.Column<int>(type: "int", nullable: false),
                    MeterId = table.Column<int>(type: "int", nullable: false)
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
                    ProcessingPlantCOQLiquidDynamicId = table.Column<int>(type: "int", nullable: false),
                    Batch = table.Column<int>(type: "int", nullable: false),
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterReadings");

            migrationBuilder.DropTable(
                name: "Meters");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQLiquidDynamicReadings");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQLiquidDynamics");
        }
    }
}

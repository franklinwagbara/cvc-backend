using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class ProccessingPlantCOQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "DippingMethod",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_DippingMethod", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "MeterType",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MeterType", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQS",
                columns: table => new
                {
                    ProcessingPlantCOQId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    MeasurementSystem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeterTypeId = table.Column<int>(type: "int", nullable: true),
                    DipMethodId = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsignorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Consignee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Terminal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipmentNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShoreFigure = table.Column<double>(type: "float", nullable: true),
                    ShipFigure = table.Column<double>(type: "float", nullable: true),
                    PrevMCubeAt15Degree = table.Column<double>(type: "float", nullable: true),
                    PrevUsBarrelsAt15Degree = table.Column<double>(type: "float", nullable: true),
                    PrevMTVac = table.Column<double>(type: "float", nullable: true),
                    PrevMTAir = table.Column<double>(type: "float", nullable: true),
                    PrevWTAir = table.Column<double>(type: "float", nullable: true),
                    PrevLongTonsAir = table.Column<double>(type: "float", nullable: true),
                    LeftMCubeAt15Degree = table.Column<double>(type: "float", nullable: true),
                    LeftUsBarrelsAt15Degree = table.Column<double>(type: "float", nullable: true),
                    LeftMTVac = table.Column<double>(type: "float", nullable: true),
                    LeftMTAir = table.Column<double>(type: "float", nullable: true),
                    LeftLongTonsAir = table.Column<double>(type: "float", nullable: true),
                    TotalMCubeAt15Degree = table.Column<double>(type: "float", nullable: true),
                    TotalUsBarrelsAt15Degree = table.Column<double>(type: "float", nullable: true),
                    TotalMTVac = table.Column<double>(type: "float", nullable: true),
                    TotalMTAir = table.Column<double>(type: "float", nullable: true),
                    TotalLongTonsAir = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQS", x => x.ProcessingPlantCOQId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQTanks",
                columns: table => new
                {
                    ProcessingPlantCOQTankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQId = table.Column<int>(type: "int", nullable: false),
                    TankId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQTanks", x => x.ProcessingPlantCOQTankId);
                });

            //migrationBuilder.CreateTable(
            //    name: "TransferRecord",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        IMONumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        VesselName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        VesselTypeId = table.Column<int>(type: "int", nullable: false),
            //        MotherVessel = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        LoadingPort = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TransferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        TotalVolume = table.Column<double>(type: "float", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TransferRecord", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQTankReadings",
                columns: table => new
                {
                    ProcessingPlantCOQTankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReadingM = table.Column<double>(type: "float", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Density = table.Column<double>(type: "float", nullable: false),
                    SpecificGravityObs = table.Column<double>(type: "float", nullable: false),
                    BarrelsAtTankTables = table.Column<double>(type: "float", nullable: false),
                    VolumeCorrectionFactor = table.Column<double>(type: "float", nullable: false),
                    WTAir = table.Column<double>(type: "float", nullable: false),
                    BarrelsToMCube = table.Column<double>(type: "float", nullable: false),
                    MCubeAt15Degree = table.Column<double>(type: "float", nullable: false),
                    UsBarrelsAt15Degree = table.Column<double>(type: "float", nullable: false),
                    MTVac = table.Column<double>(type: "float", nullable: false),
                    MTAir = table.Column<double>(type: "float", nullable: false),
                    LongTonsAir = table.Column<double>(type: "float", nullable: false),
                    ProcessingPlantCOQTankId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQTankReadings", x => x.ProcessingPlantCOQTankId);
                    table.ForeignKey(
                        name: "FK_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTanks_ProcessingPlantCOQTankId1",
                        column: x => x.ProcessingPlantCOQTankId1,
                        principalTable: "ProcessingPlantCOQTanks",
                        principalColumn: "ProcessingPlantCOQTankId",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "TransferDetail",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        IMONumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        VesselName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ProductId = table.Column<int>(type: "int", nullable: false),
            //        OfftakeVolume = table.Column<double>(type: "float", nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        TransferRecordId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TransferDetail", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_TransferDetail_TransferRecord_TransferRecordId",
            //            column: x => x.TransferRecordId,
            //            principalTable: "TransferRecord",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTankId1",
                table: "ProcessingPlantCOQTankReadings",
                column: "ProcessingPlantCOQTankId1");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TransferDetail_TransferRecordId",
            //    table: "TransferDetail",
            //    column: "TransferRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "DippingMethod");

            //migrationBuilder.DropTable(
            //    name: "MeterType");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQS");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQTankReadings");

            //migrationBuilder.DropTable(
            //    name: "TransferDetail");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQTanks");

            //migrationBuilder.DropTable(
            //    name: "TransferRecord");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class CondensateCoq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_OperatingFacility",
            //    table: "OperatingFacility");

            //migrationBuilder.DropColumn(
            //    name: "CompanyId",
            //    table: "OperatingFacility");

            //migrationBuilder.RenameTable(
            //    name: "OperatingFacility",
            //    newName: "OperatingFacilities");

            //migrationBuilder.AddColumn<string>(
            //    name: "UserId",
            //    table: "TransferRecord",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "ApiGravity",
                table: "ProcessingPlantCOQs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AverageBsw",
                table: "ProcessingPlantCOQs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AverageSgAt60",
                table: "ProcessingPlantCOQs",
                type: "float",
                nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "CurrentDeskId",
            //    table: "ProcessingPlantCOQs",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "DateModified",
            //    table: "ProcessingPlantCOQs",
            //    type: "datetime2",
            //    nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ProcessingPlantCOQs",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "SubmittedDate",
            //    table: "ProcessingPlantCOQs",
            //    type: "datetime2",
            //    nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalBswBarrelsAt60",
                table: "ProcessingPlantCOQs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalBswLongTons",
                table: "ProcessingPlantCOQs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalGrossBarrelsAt60",
                table: "ProcessingPlantCOQs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalGrossLongTons",
                table: "ProcessingPlantCOQs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalGrossUsBarrelsAtTankTemp",
                table: "ProcessingPlantCOQs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalNettLongTons",
                table: "ProcessingPlantCOQs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalNettMetricTons",
                table: "ProcessingPlantCOQs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalNettUsBarrelsAt60",
                table: "ProcessingPlantCOQs",
                type: "float",
                nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsPPCOQ",
            //    table: "Messages",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProcessingCOQId",
            //    table: "Messages",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsDefaulter",
            //    table: "AspNetUsers",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "CompanyEmail",
            //    table: "OperatingFacilities",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_OperatingFacilities",
            //    table: "OperatingFacilities",
            //    column: "Id");

            //migrationBuilder.CreateTable(
            //    name: "CoQReferences",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        DepotCoQId = table.Column<int>(type: "int", nullable: true),
            //        PlantCoQId = table.Column<int>(type: "int", nullable: true),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CoQReferences", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_CoQReferences_CoQs_DepotCoQId",
            //            column: x => x.DepotCoQId,
            //            principalTable: "CoQs",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_CoQReferences_ProcessingPlantCOQs_PlantCoQId",
            //            column: x => x.PlantCoQId,
            //            principalTable: "ProcessingPlantCOQs",
            //            principalColumn: "ProcessingPlantCOQId");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "COQSubmittedDocuments",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CoQId = table.Column<int>(type: "int", nullable: false),
            //        FileId = table.Column<int>(type: "int", nullable: false),
            //        DocId = table.Column<int>(type: "int", nullable: false),
            //        DocSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        DocType = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        DocName = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_COQSubmittedDocuments", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PPCOQCertificates",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        COQId = table.Column<int>(type: "int", nullable: true),
            //        ProductId = table.Column<int>(type: "int", nullable: true),
            //        ElpsId = table.Column<int>(type: "int", nullable: false),
            //        ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CertifcateNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Signature = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        QRCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PPCOQCertificates", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_PPCOQCertificates_CoQs_COQId",
            //            column: x => x.COQId,
            //            principalTable: "CoQs",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_PPCOQCertificates_Products_ProductId",
            //            column: x => x.ProductId,
            //            principalTable: "Products",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PPCOQHistories",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        COQId = table.Column<int>(type: "int", nullable: false),
            //        TriggeredBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        TriggeredByRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        TargetedTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        TargetRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Date = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PPCOQHistories", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PPCOQSubmittedDocuments",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ProcessingPlantCOQId = table.Column<int>(type: "int", nullable: false),
            //        FileId = table.Column<int>(type: "int", nullable: false),
            //        DocId = table.Column<int>(type: "int", nullable: false),
            //        DocSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        DocType = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        DocName = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PPCOQSubmittedDocuments", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQCondensateDBatches",
                columns: table => new
                {
                    ProcessingPlantCOQCondensateDBatchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQId = table.Column<int>(type: "int", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    SumDiffGrossUsBarrelsAtTankTemp = table.Column<double>(type: "float", nullable: true),
                    SumDiffGrossBarrelsAt60 = table.Column<double>(type: "float", nullable: true),
                    SumDiffGrossLongTons = table.Column<double>(type: "float", nullable: true),
                    SumDiffBswBarrelsAt60 = table.Column<double>(type: "float", nullable: true),
                    SumDiffBswLongTons = table.Column<double>(type: "float", nullable: true),
                    SumDiffNettUsBarrelsAt60 = table.Column<double>(type: "float", nullable: true),
                    SumDiffNettLongTons = table.Column<double>(type: "float", nullable: true),
                    SumDiffNettMetricTons = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQCondensateDBatches", x => x.ProcessingPlantCOQCondensateDBatchId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQCondensateSBatches",
                columns: table => new
                {
                    ProcessingPlantCOQCondensateSBatchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQId = table.Column<int>(type: "int", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    SumDiffGrossUsBarrelsAtTankTemp = table.Column<double>(type: "float", nullable: true),
                    SumDiffGrossBarrelsAt60 = table.Column<double>(type: "float", nullable: true),
                    SumDiffGrossLongTons = table.Column<double>(type: "float", nullable: true),
                    SumDiffBswBarrelsAt60 = table.Column<double>(type: "float", nullable: true),
                    SumDiffBswLongTons = table.Column<double>(type: "float", nullable: true),
                    SumDiffNettUsBarrelsAt60 = table.Column<double>(type: "float", nullable: true),
                    SumDiffNettLongTons = table.Column<double>(type: "float", nullable: true),
                    SumDiffNettMetricTons = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQCondensateSBatches", x => x.ProcessingPlantCOQCondensateSBatchId);
                });

            //migrationBuilder.CreateTable(
            //    name: "SourceRecipientVessel",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SourceVesselId = table.Column<int>(type: "int", nullable: false),
            //        DestinationVesselId = table.Column<int>(type: "int", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SourceRecipientVessel", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_SourceRecipientVessel_Facilities_DestinationVesselId",
            //            column: x => x.DestinationVesselId,
            //            principalTable: "Facilities",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_SourceRecipientVessel_Facilities_SourceVesselId",
            //            column: x => x.SourceVesselId,
            //            principalTable: "Facilities",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQCondensateDBatchMeters",
                columns: table => new
                {
                    ProcessingPlantCOQCondensateDBatchMeterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQCondensateDBatchId = table.Column<int>(type: "int", nullable: false),
                    MeterId = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Pressure = table.Column<double>(type: "float", nullable: false),
                    MeterFactor = table.Column<double>(type: "float", nullable: false),
                    Ctl = table.Column<double>(type: "float", nullable: false),
                    Cpl = table.Column<double>(type: "float", nullable: false),
                    ApiAt60 = table.Column<double>(type: "float", nullable: false),
                    Vcf = table.Column<double>(type: "float", nullable: false),
                    Bsw = table.Column<double>(type: "float", nullable: false),
                    GrossLtBblFactor = table.Column<double>(type: "float", nullable: false),
                    GrossUsBarrelsAt60 = table.Column<double>(type: "float", nullable: false),
                    GrossLongTons = table.Column<double>(type: "float", nullable: false),
                    BswBarrelsAt60 = table.Column<double>(type: "float", nullable: false),
                    BswLongTons = table.Column<double>(type: "float", nullable: false),
                    NettUsBarrelsAt60 = table.Column<double>(type: "float", nullable: false),
                    NettLongTons = table.Column<double>(type: "float", nullable: false),
                    NettMetricTons = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQCondensateDBatchMeters", x => x.ProcessingPlantCOQCondensateDBatchMeterId);
                    table.ForeignKey(
                        name: "FK_ProcessingPlantCOQCondensateDBatchMeters_ProcessingPlantCOQCondensateDBatches_ProcessingPlantCOQCondensateDBatchId",
                        column: x => x.ProcessingPlantCOQCondensateDBatchId,
                        principalTable: "ProcessingPlantCOQCondensateDBatches",
                        principalColumn: "ProcessingPlantCOQCondensateDBatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQCondensateSBatchTanks",
                columns: table => new
                {
                    ProcessingPlantCOQCondensateSBatchTankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQCondensateSBatchId = table.Column<int>(type: "int", nullable: false),
                    TankId = table.Column<int>(type: "int", nullable: false),
                    DiffGrossUsBarrelsAtTankTemp = table.Column<double>(type: "float", nullable: true),
                    DiffGrossBarrelsAt60 = table.Column<double>(type: "float", nullable: true),
                    DiffGrossLongTons = table.Column<double>(type: "float", nullable: true),
                    DiffBswBarrelsAt60 = table.Column<double>(type: "float", nullable: true),
                    DiffBswLongTons = table.Column<double>(type: "float", nullable: true),
                    DiffNettUsBarrelsAt60 = table.Column<double>(type: "float", nullable: true),
                    DiffNettLongTons = table.Column<double>(type: "float", nullable: true),
                    DiffNettMetricTons = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQCondensateSBatchTanks", x => x.ProcessingPlantCOQCondensateSBatchTankId);
                    table.ForeignKey(
                        name: "FK_ProcessingPlantCOQCondensateSBatchTanks_ProcessingPlantCOQCondensateSBatches_ProcessingPlantCOQCondensateSBatchId",
                        column: x => x.ProcessingPlantCOQCondensateSBatchId,
                        principalTable: "ProcessingPlantCOQCondensateSBatches",
                        principalColumn: "ProcessingPlantCOQCondensateSBatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CondensateDynamicMeterReadings",
                columns: table => new
                {
                    CondensateDynamicMeterReadingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQCondensateDBatchMeterId = table.Column<int>(type: "int", nullable: false),
                    MeasurementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MReadingBbl = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CondensateDynamicMeterReadings", x => x.CondensateDynamicMeterReadingId);
                    table.ForeignKey(
                        name: "FK_CondensateDynamicMeterReadings_ProcessingPlantCOQCondensateDBatchMeters_ProcessingPlantCOQCondensateDBatchMeterId",
                        column: x => x.ProcessingPlantCOQCondensateDBatchMeterId,
                        principalTable: "ProcessingPlantCOQCondensateDBatchMeters",
                        principalColumn: "ProcessingPlantCOQCondensateDBatchMeterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQCondensateTankReadings",
                columns: table => new
                {
                    ProcessingPlantCOQCondensateTankReadingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQCondensateSBatchTankId = table.Column<int>(type: "int", nullable: false),
                    MeasurementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ullage = table.Column<double>(type: "float", nullable: false),
                    TankTemp = table.Column<double>(type: "float", nullable: false),
                    Tov = table.Column<double>(type: "float", nullable: false),
                    Bsw = table.Column<double>(type: "float", nullable: false),
                    WaterGuage = table.Column<double>(type: "float", nullable: false),
                    ObsvWater = table.Column<double>(type: "float", nullable: false),
                    ApiAt60 = table.Column<double>(type: "float", nullable: false),
                    Vcf = table.Column<double>(type: "float", nullable: false),
                    LtBblFactor = table.Column<double>(type: "float", nullable: false),
                    GrossUsBarrelsAtTankTemp = table.Column<double>(type: "float", nullable: false),
                    GrossUsBarrelsAt60 = table.Column<double>(type: "float", nullable: false),
                    GrossLongTons = table.Column<double>(type: "float", nullable: false),
                    BswBarrelsAt60 = table.Column<double>(type: "float", nullable: false),
                    BswLongTons = table.Column<double>(type: "float", nullable: false),
                    NettUsBarrelsAt60 = table.Column<double>(type: "float", nullable: false),
                    NettLongTons = table.Column<double>(type: "float", nullable: false),
                    NettMetricTons = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQCondensateTankReadings", x => x.ProcessingPlantCOQCondensateTankReadingId);
                    table.ForeignKey(
                        name: "FK_ProcessingPlantCOQCondensateTankReadings_ProcessingPlantCOQCondensateSBatchTanks_ProcessingPlantCOQCondensateSBatchTankId",
                        column: x => x.ProcessingPlantCOQCondensateSBatchTankId,
                        principalTable: "ProcessingPlantCOQCondensateSBatchTanks",
                        principalColumn: "ProcessingPlantCOQCondensateSBatchTankId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingPlantCOQs_PlantId",
                table: "ProcessingPlantCOQs",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingPlantCOQs_ProductId",
                table: "ProcessingPlantCOQs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Meters_PlantId",
                table: "Meters",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_CondensateDynamicMeterReadings_ProcessingPlantCOQCondensateDBatchMeterId",
                table: "CondensateDynamicMeterReadings",
                column: "ProcessingPlantCOQCondensateDBatchMeterId");

            migrationBuilder.CreateIndex(
                name: "IX_CoQReferences_DepotCoQId",
                table: "CoQReferences",
                column: "DepotCoQId");

            migrationBuilder.CreateIndex(
                name: "IX_CoQReferences_PlantCoQId",
                table: "CoQReferences",
                column: "PlantCoQId");

            migrationBuilder.CreateIndex(
                name: "IX_PPCOQCertificates_COQId",
                table: "PPCOQCertificates",
                column: "COQId");

            migrationBuilder.CreateIndex(
                name: "IX_PPCOQCertificates_ProductId",
                table: "PPCOQCertificates",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingPlantCOQCondensateDBatchMeters_ProcessingPlantCOQCondensateDBatchId",
                table: "ProcessingPlantCOQCondensateDBatchMeters",
                column: "ProcessingPlantCOQCondensateDBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingPlantCOQCondensateSBatchTanks_ProcessingPlantCOQCondensateSBatchId",
                table: "ProcessingPlantCOQCondensateSBatchTanks",
                column: "ProcessingPlantCOQCondensateSBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingPlantCOQCondensateTankReadings_ProcessingPlantCOQCondensateSBatchTankId",
                table: "ProcessingPlantCOQCondensateTankReadings",
                column: "ProcessingPlantCOQCondensateSBatchTankId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceRecipientVessel_DestinationVesselId",
                table: "SourceRecipientVessel",
                column: "DestinationVesselId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceRecipientVessel_SourceVesselId",
                table: "SourceRecipientVessel",
                column: "SourceVesselId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meters_Plants_PlantId",
                table: "Meters",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessingPlantCOQs_Plants_PlantId",
                table: "ProcessingPlantCOQs",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessingPlantCOQs_Products_ProductId",
                table: "ProcessingPlantCOQs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meters_Plants_PlantId",
                table: "Meters");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessingPlantCOQs_Plants_PlantId",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessingPlantCOQs_Products_ProductId",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropTable(
                name: "CondensateDynamicMeterReadings");

            migrationBuilder.DropTable(
                name: "CoQReferences");

            migrationBuilder.DropTable(
                name: "COQSubmittedDocuments");

            migrationBuilder.DropTable(
                name: "PPCOQCertificates");

            migrationBuilder.DropTable(
                name: "PPCOQHistories");

            migrationBuilder.DropTable(
                name: "PPCOQSubmittedDocuments");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQCondensateTankReadings");

            migrationBuilder.DropTable(
                name: "SourceRecipientVessel");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQCondensateDBatchMeters");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQCondensateSBatchTanks");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQCondensateDBatches");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQCondensateSBatches");

            migrationBuilder.DropIndex(
                name: "IX_ProcessingPlantCOQs_PlantId",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropIndex(
                name: "IX_ProcessingPlantCOQs_ProductId",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropIndex(
                name: "IX_Meters_PlantId",
                table: "Meters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OperatingFacilities",
                table: "OperatingFacilities");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TransferRecord");

            migrationBuilder.DropColumn(
                name: "ApiGravity",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "AverageBsw",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "AverageSgAt60",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "CurrentDeskId",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "SubmittedDate",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "TotalBswBarrelsAt60",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "TotalBswLongTons",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "TotalGrossBarrelsAt60",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "TotalGrossLongTons",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "TotalGrossUsBarrelsAtTankTemp",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "TotalNettLongTons",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "TotalNettMetricTons",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "TotalNettUsBarrelsAt60",
                table: "ProcessingPlantCOQs");

            migrationBuilder.DropColumn(
                name: "IsPPCOQ",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ProcessingCOQId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsDefaulter",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyEmail",
                table: "OperatingFacilities");

            migrationBuilder.RenameTable(
                name: "OperatingFacilities",
                newName: "OperatingFacility");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "OperatingFacility",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperatingFacility",
                table: "OperatingFacility",
                column: "Id");
        }
    }
}

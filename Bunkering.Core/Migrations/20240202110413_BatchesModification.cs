using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class BatchesModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTanks_ProcessingPlantCOQTankId",
            //    table: "ProcessingPlantCOQTankReadings");

            //migrationBuilder.DropTable(
            //    name: "ProcessingPlantCOQTanks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessingPlantCOQS",
                table: "ProcessingPlantCOQS");

            migrationBuilder.RenameTable(
                name: "ProcessingPlantCOQS",
                newName: "ProcessingPlantCOQs");

            migrationBuilder.RenameColumn(
                name: "ProcessingPlantCOQTankId",
                table: "ProcessingPlantCOQTankReadings",
                newName: "ProcessingPlantCOQBatchTankId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTankId",
            //    table: "ProcessingPlantCOQTankReadings",
            //    newName: "IX_ProcessingPlantCOQTankReadings_ProcessingPlantCOQBatchTankId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessingPlantCOQs",
                table: "ProcessingPlantCOQs",
                column: "ProcessingPlantCOQId");

            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    BatchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.BatchId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQBatches",
                columns: table => new
                {
                    ProcessingPlantCOQBatchId = table.Column<int>(type: "int", nullable: false)
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
                    table.PrimaryKey("PK_ProcessingPlantCOQBatches", x => x.ProcessingPlantCOQBatchId);
                });

            //migrationBuilder.CreateTable(
            //    name: "ProcessingPlantCOQBatchTanks",
            //    columns: table => new
            //    {
            //        ProcessingPlantCOQBatchTankId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ProcessingPlantCOQBatchId = table.Column<int>(type: "int", nullable: false),
            //        TankId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ProcessingPlantCOQBatchTanks", x => x.ProcessingPlantCOQBatchTankId);
            //        table.ForeignKey(
            //            name: "FK_ProcessingPlantCOQBatchTanks_ProcessingPlantCOQBatches_ProcessingPlantCOQBatchId",
            //            column: x => x.ProcessingPlantCOQBatchId,
            //            principalTable: "ProcessingPlantCOQBatches",
            //            principalColumn: "ProcessingPlantCOQBatchId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingPlantCOQBatchTanks_ProcessingPlantCOQBatchId",
                table: "ProcessingPlantCOQBatchTanks",
                column: "ProcessingPlantCOQBatchId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProcessingPlantCOQTankReadings_ProcessingPlantCOQBatchTanks_ProcessingPlantCOQBatchTankId",
            //    table: "ProcessingPlantCOQTankReadings",
            //    column: "ProcessingPlantCOQBatchTankId",
            //    principalTable: "ProcessingPlantCOQBatchTanks",
            //    principalColumn: "ProcessingPlantCOQBatchTankId",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessingPlantCOQTankReadings_ProcessingPlantCOQBatchTanks_ProcessingPlantCOQBatchTankId",
                table: "ProcessingPlantCOQTankReadings");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQBatchTanks");

            migrationBuilder.DropTable(
                name: "ProcessingPlantCOQBatches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessingPlantCOQs",
                table: "ProcessingPlantCOQs");

            migrationBuilder.RenameTable(
                name: "ProcessingPlantCOQs",
                newName: "ProcessingPlantCOQS");

            migrationBuilder.RenameColumn(
                name: "ProcessingPlantCOQBatchTankId",
                table: "ProcessingPlantCOQTankReadings",
                newName: "ProcessingPlantCOQTankId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessingPlantCOQTankReadings_ProcessingPlantCOQBatchTankId",
                table: "ProcessingPlantCOQTankReadings",
                newName: "IX_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTankId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessingPlantCOQS",
                table: "ProcessingPlantCOQS",
                column: "ProcessingPlantCOQId");

            migrationBuilder.CreateTable(
                name: "ProcessingPlantCOQTanks",
                columns: table => new
                {
                    ProcessingPlantCOQTankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingPlantCOQId = table.Column<int>(type: "int", nullable: false),
                    SumDiffLongTonsAir = table.Column<double>(type: "float", nullable: true),
                    SumDiffMCubeAt15Degree = table.Column<double>(type: "float", nullable: true),
                    SumDiffMTAir = table.Column<double>(type: "float", nullable: true),
                    SumDiffMTVac = table.Column<double>(type: "float", nullable: true),
                    SumDiffUsBarrelsAt15Degree = table.Column<double>(type: "float", nullable: true),
                    TankId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingPlantCOQTanks", x => x.ProcessingPlantCOQTankId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTanks_ProcessingPlantCOQTankId",
                table: "ProcessingPlantCOQTankReadings",
                column: "ProcessingPlantCOQTankId",
                principalTable: "ProcessingPlantCOQTanks",
                principalColumn: "ProcessingPlantCOQTankId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

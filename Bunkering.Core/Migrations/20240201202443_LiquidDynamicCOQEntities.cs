using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class LiquidDynamicCOQEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTanks_ProcessingPlantCOQTankId1",
            //    table: "ProcessingPlantCOQTankReadings");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_ProcessingPlantCOQTankReadings",
            //    table: "ProcessingPlantCOQTankReadings");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTankId1",
            //    table: "ProcessingPlantCOQTankReadings");

            //migrationBuilder.RenameColumn(
            //    name: "ProcessingPlantCOQTankId1",
            //    table: "ProcessingPlantCOQTankReadings",
            //    newName: "ProcessingPlantCOQTankReadingId");

            //migrationBuilder.AlterColumn<int>(
            //    name: "ProcessingPlantCOQTankId",
            //    table: "ProcessingPlantCOQTankReadings",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AlterColumn<int>(
            //    name: "ProcessingPlantCOQTankReadingId",
            //    table: "ProcessingPlantCOQTankReadings",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .Annotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_ProcessingPlantCOQTankReadings",
            //    table: "ProcessingPlantCOQTankReadings",
            //    column: "ProcessingPlantCOQTankReadingId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTankId",
            //    table: "ProcessingPlantCOQTankReadings",
            //    column: "ProcessingPlantCOQTankId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTanks_ProcessingPlantCOQTankId",
            //    table: "ProcessingPlantCOQTankReadings",
            //    column: "ProcessingPlantCOQTankId",
            //    principalTable: "ProcessingPlantCOQTanks",
            //    principalColumn: "ProcessingPlantCOQTankId",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTanks_ProcessingPlantCOQTankId",
                table: "ProcessingPlantCOQTankReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessingPlantCOQTankReadings",
                table: "ProcessingPlantCOQTankReadings");

            migrationBuilder.DropIndex(
                name: "IX_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTankId",
                table: "ProcessingPlantCOQTankReadings");

            migrationBuilder.RenameColumn(
                name: "ProcessingPlantCOQTankReadingId",
                table: "ProcessingPlantCOQTankReadings",
                newName: "ProcessingPlantCOQTankId1");

            migrationBuilder.AlterColumn<int>(
                name: "ProcessingPlantCOQTankId",
                table: "ProcessingPlantCOQTankReadings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "ProcessingPlantCOQTankId1",
                table: "ProcessingPlantCOQTankReadings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessingPlantCOQTankReadings",
                table: "ProcessingPlantCOQTankReadings",
                column: "ProcessingPlantCOQTankId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTankId1",
                table: "ProcessingPlantCOQTankReadings",
                column: "ProcessingPlantCOQTankId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTanks_ProcessingPlantCOQTankId1",
                table: "ProcessingPlantCOQTankReadings",
                column: "ProcessingPlantCOQTankId1",
                principalTable: "ProcessingPlantCOQTanks",
                principalColumn: "ProcessingPlantCOQTankId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

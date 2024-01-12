using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCOQAndAmountColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoQs_Depots_DepotId",
                table: "CoQs");

            migrationBuilder.DropIndex(
                name: "IX_CoQs_DepotId",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "DepotId",
                table: "CoQs");

            migrationBuilder.AlterColumn<double>(
                name: "NonRenewalPenalty",
                table: "Payments",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "LateRenewalPenalty",
                table: "Payments",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_CoQs_PlantId",
                table: "CoQs",
                column: "PlantId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoQs_Plants_PlantId",
                table: "CoQs",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoQs_Plants_PlantId",
                table: "CoQs");

            migrationBuilder.DropIndex(
                name: "IX_CoQs_PlantId",
                table: "CoQs");

            migrationBuilder.AlterColumn<decimal>(
                name: "NonRenewalPenalty",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "LateRenewalPenalty",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "DepotId",
                table: "CoQs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CoQs_DepotId",
                table: "CoQs",
                column: "DepotId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoQs_Depots_DepotId",
                table: "CoQs",
                column: "DepotId",
                principalTable: "Depots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

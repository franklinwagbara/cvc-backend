using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCOQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDepots_Depots_DepotId",
                table: "ApplicationDepots");

            migrationBuilder.DropColumn(
                name: "DifferenceBtwShipAndShoreFigure",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "PercentageDifference",
                table: "CoQs");

            migrationBuilder.AlterColumn<long>(
                name: "ElpsPlantId",
                table: "Plants",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "NameConsignee",
                table: "CoQs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ApplicationDepots_Plants_DepotId",
            //    table: "ApplicationDepots",
            //    column: "DepotId",
            //    principalTable: "Plants",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDepots_Plants_DepotId",
                table: "ApplicationDepots");

            migrationBuilder.DropColumn(
                name: "NameConsignee",
                table: "CoQs");

            migrationBuilder.AlterColumn<long>(
                name: "ElpsPlantId",
                table: "Plants",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DifferenceBtwShipAndShoreFigure",
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

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationDepots_Depots_DepotId",
                table: "ApplicationDepots",
                column: "DepotId",
                principalTable: "Depots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

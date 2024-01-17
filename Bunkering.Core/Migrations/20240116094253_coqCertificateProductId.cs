using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class coqCertificateProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_COQCertificates_CoQs_COQId",
                table: "COQCertificates");

            migrationBuilder.AlterColumn<int>(
                name: "COQId",
                table: "COQCertificates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "COQCertificates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_COQCertificates_ProductId",
                table: "COQCertificates",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_COQCertificates_CoQs_COQId",
                table: "COQCertificates",
                column: "COQId",
                principalTable: "CoQs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_COQCertificates_Products_ProductId",
                table: "COQCertificates",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_COQCertificates_CoQs_COQId",
                table: "COQCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_COQCertificates_Products_ProductId",
                table: "COQCertificates");

            migrationBuilder.DropIndex(
                name: "IX_COQCertificates_ProductId",
                table: "COQCertificates");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "COQCertificates");

            migrationBuilder.AlterColumn<int>(
                name: "COQId",
                table: "COQCertificates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_COQCertificates_CoQs_COQId",
                table: "COQCertificates",
                column: "COQId",
                principalTable: "CoQs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

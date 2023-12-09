using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class removedNullableExtraPay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_ExtraPayments_ExtraPaymentId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "ExtraPaymentId",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_ExtraPayments_ExtraPaymentId",
                table: "Payments",
                column: "ExtraPaymentId",
                principalTable: "ExtraPayments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_ExtraPayments_ExtraPaymentId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "ExtraPaymentId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_ExtraPayments_ExtraPaymentId",
                table: "Payments",
                column: "ExtraPaymentId",
                principalTable: "ExtraPayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class check : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDepots_Applications_ProductId",
                table: "ApplicationDepots");

            migrationBuilder.DropColumn(
                name: "DischargePort",
                table: "Applications");

            migrationBuilder.AddColumn<int>(
                name: "COQId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCOQ",
                table: "Messages",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketerName",
                table: "Depots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentDeskId",
                table: "CoQs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FADApproved",
                table: "CoQs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CoQs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "CoQs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CoQs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedDate",
                table: "CoQs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Jetty",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherVessel",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "COQCertificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COQId = table.Column<int>(type: "int", nullable: false),
                    ElpsId = table.Column<int>(type: "int", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CertifcateNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QRCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COQCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COQCertificates_CoQs_COQId",
                        column: x => x.COQId,
                        principalTable: "CoQs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COQHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COQId = table.Column<int>(type: "int", nullable: false),
                    TriggeredBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggeredByRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetedTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COQHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepotFieldOfficers_DepotID",
                table: "DepotFieldOfficers",
                column: "DepotID");

            migrationBuilder.CreateIndex(
                name: "IX_COQCertificates_COQId",
                table: "COQCertificates",
                column: "COQId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationDepots_Products_ProductId",
                table: "ApplicationDepots",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotFieldOfficers_Depots_DepotID",
                table: "DepotFieldOfficers",
                column: "DepotID",
                principalTable: "Depots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDepots_Products_ProductId",
                table: "ApplicationDepots");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotFieldOfficers_Depots_DepotID",
                table: "DepotFieldOfficers");

            migrationBuilder.DropForeignKey(
                name: "FK_Depots_States_StateId",
                table: "Depots");

            migrationBuilder.DropTable(
                name: "COQCertificates");

            migrationBuilder.DropTable(
                name: "COQHistories");

            migrationBuilder.DropIndex(
                name: "IX_Depots_StateId",
                table: "Depots");

            migrationBuilder.DropIndex(
                name: "IX_DepotFieldOfficers_DepotID",
                table: "DepotFieldOfficers");

            migrationBuilder.DropColumn(
                name: "COQId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsCOQ",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MarketerName",
                table: "Depots");

            migrationBuilder.DropColumn(
                name: "CurrentDeskId",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "FADApproved",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "SubmittedDate",
                table: "CoQs");

            migrationBuilder.DropColumn(
                name: "Jetty",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "MotherVessel",
                table: "Applications");

            migrationBuilder.AddColumn<string>(
                name: "DischargePort",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationDepots_Applications_ProductId",
                table: "ApplicationDepots",
                column: "ProductId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

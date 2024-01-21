using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddreatedAtToAppSurveyor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ApplicationSurveyors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.CreateTable(
            //    name: "JettyFieldOfficers",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        JettyId = table.Column<int>(type: "int", nullable: false),
            //        OfficerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_JettyFieldOfficers", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_JettyFieldOfficers_AspNetUsers_OfficerID",
            //            column: x => x.OfficerID,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_JettyFieldOfficers_Jetties_JettyId",
            //            column: x => x.JettyId,
            //            principalTable: "Jetties",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_JettyFieldOfficers_JettyId",
            //    table: "JettyFieldOfficers",
            //    column: "JettyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_JettyFieldOfficers_OfficerID",
            //    table: "JettyFieldOfficers",
            //    column: "OfficerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JettyFieldOfficers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ApplicationSurveyors");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddCoq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<int>(type: "int", nullable: false),
                    DepotId = table.Column<int>(type: "int", nullable: false),
                    DateOfVesselArrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfVesselUllage = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfSTAfterDischarge = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MTVAC = table.Column<decimal>(name: "MT_VAC", type: "decimal(18,2)", nullable: false),
                    MTAIR = table.Column<decimal>(name: "MT_AIR", type: "decimal(18,2)", nullable: false),
                    GOV = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GSV = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepotPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoQs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoQs_Applications_AppId",
                        column: x => x.AppId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoQs_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoQs_AppId",
                table: "CoQs",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_CoQs_DepotId",
                table: "CoQs",
                column: "DepotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoQs");
        }
    }
}

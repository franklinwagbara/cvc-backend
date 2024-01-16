using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddSignature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeskMovementDate",
                table: "Applications",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Signature",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeskMovementDate",
                table: "Applications");
        }
    }
}

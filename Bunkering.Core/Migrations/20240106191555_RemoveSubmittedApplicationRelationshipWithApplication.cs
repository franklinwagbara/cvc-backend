using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bunkering.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSubmittedApplicationRelationshipWithApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedDocuments_Applications_ApplicationId",
                table: "SubmittedDocuments");

            migrationBuilder.DropIndex(
                name: "IX_SubmittedDocuments_ApplicationId",
                table: "SubmittedDocuments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SubmittedDocuments_ApplicationId",
                table: "SubmittedDocuments",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedDocuments_Applications_ApplicationId",
                table: "SubmittedDocuments",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

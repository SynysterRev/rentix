using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rentix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MorePropertiesToDocumentAndLinkWithLease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ChargesAmount",
                table: "Leases",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "LeaseDocumentId",
                table: "Leases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Documents",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSizeInBytes",
                table: "Documents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Leases_LeaseDocumentId",
                table: "Leases",
                column: "LeaseDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_Documents_LeaseDocumentId",
                table: "Leases",
                column: "LeaseDocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leases_Documents_LeaseDocumentId",
                table: "Leases");

            migrationBuilder.DropIndex(
                name: "IX_Leases_LeaseDocumentId",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "ChargesAmount",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "LeaseDocumentId",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "FileSizeInBytes",
                table: "Documents");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Missing_CustomerPropertyExtra_Relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPropertyExtraDetails_CustomerPropertyExtras_customerPropertyExtraId",
                table: "CustomerPropertyExtraDetails");

            migrationBuilder.RenameColumn(
                name: "customerPropertyExtraId",
                table: "CustomerPropertyExtraDetails",
                newName: "CustomerPropertyExtraId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerPropertyExtraDetails_customerPropertyExtraId",
                table: "CustomerPropertyExtraDetails",
                newName: "IX_CustomerPropertyExtraDetails_CustomerPropertyExtraId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPropertyExtraDetails_CustomerPropertyExtras_CustomerPropertyExtraId",
                table: "CustomerPropertyExtraDetails",
                column: "CustomerPropertyExtraId",
                principalTable: "CustomerPropertyExtras",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPropertyExtraDetails_CustomerPropertyExtras_CustomerPropertyExtraId",
                table: "CustomerPropertyExtraDetails");

            migrationBuilder.RenameColumn(
                name: "CustomerPropertyExtraId",
                table: "CustomerPropertyExtraDetails",
                newName: "customerPropertyExtraId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerPropertyExtraDetails_CustomerPropertyExtraId",
                table: "CustomerPropertyExtraDetails",
                newName: "IX_CustomerPropertyExtraDetails_customerPropertyExtraId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPropertyExtraDetails_CustomerPropertyExtras_customerPropertyExtraId",
                table: "CustomerPropertyExtraDetails",
                column: "customerPropertyExtraId",
                principalTable: "CustomerPropertyExtras",
                principalColumn: "Id");
        }
    }
}

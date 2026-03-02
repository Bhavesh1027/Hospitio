using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_TableFor_CustomerPropertyExtrasDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CustomerPropertyExtras");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "CustomerPropertyExtras");

            migrationBuilder.CreateTable(
                name: "CustomerPropertyExtraDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customerPropertyExtraId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Link = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPropertyExtraDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPropertyExtraDetails_CustomerPropertyExtras_customerPropertyExtraId",
                        column: x => x.customerPropertyExtraId,
                        principalTable: "CustomerPropertyExtras",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPropertyExtraDetails_customerPropertyExtraId",
                table: "CustomerPropertyExtraDetails",
                column: "customerPropertyExtraId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPropertyExtraDetails");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CustomerPropertyExtras",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "CustomerPropertyExtras",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}

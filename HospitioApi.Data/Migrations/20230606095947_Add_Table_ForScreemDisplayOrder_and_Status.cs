using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_Table_ForScreemDisplayOrder_and_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPropertyInformationsSections");

            migrationBuilder.CreateTable(
                name: "ScreenDisplayOrderAndStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScreenName = table.Column<int>(type: "int", nullable: false),
                    JsonData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefrenceId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenDisplayOrderAndStatuses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScreenDisplayOrderAndStatuses");

            migrationBuilder.CreateTable(
                name: "CustomerPropertyInformationsSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerPropertyInformationId = table.Column<int>(type: "int", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    SectionName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPropertyInformationsSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPropertyInformationsSections_CustomerPropertyInformations_CustomerPropertyInformationId",
                        column: x => x.CustomerPropertyInformationId,
                        principalTable: "CustomerPropertyInformations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPropertyInformationsSections_CustomerPropertyInformationId",
                table: "CustomerPropertyInformationsSections",
                column: "CustomerPropertyInformationId");
        }
    }
}

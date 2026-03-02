using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class DisplayOrderAddedOnTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerGuestAppEnhanceYourStayItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerGuestAppEnhanceYourStayCategories",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerGuestAppEnhanceYourStayItems");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerGuestAppEnhanceYourStayCategories");
        }
    }
}

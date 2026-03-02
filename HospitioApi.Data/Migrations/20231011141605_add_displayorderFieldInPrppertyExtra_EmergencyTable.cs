using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class add_displayorderFieldInPrppertyExtra_EmergencyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerPropertyExtras",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerPropertyEmergencyNumbers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerPropertyExtras");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerPropertyEmergencyNumbers");
        }
    }
}

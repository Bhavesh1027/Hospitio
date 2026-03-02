using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddVonageColumnsInCustomerGuestJournyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VonageTemplateId",
                table: "CustomerGuestJournies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VonageTemplateStatus",
                table: "CustomerGuestJournies",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VonageTemplateId",
                table: "CustomerGuestJournies");

            migrationBuilder.DropColumn(
                name: "VonageTemplateStatus",
                table: "CustomerGuestJournies");
        }
    }
}

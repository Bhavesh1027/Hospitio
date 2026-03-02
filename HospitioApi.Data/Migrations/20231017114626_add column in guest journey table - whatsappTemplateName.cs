using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class addcolumninguestjourneytablewhatsappTemplateName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WhatsappTemplateName",
                table: "GuestJourneyMessagesTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsappTemplateName",
                table: "CustomerGuestJournies",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhatsappTemplateName",
                table: "GuestJourneyMessagesTemplates");

            migrationBuilder.DropColumn(
                name: "WhatsappTemplateName",
                table: "CustomerGuestJournies");
        }
    }
}

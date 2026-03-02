using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddVonageFieldsInGuestJourneyMessagesTemplateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Buttons",
                table: "GuestJourneyMessagesTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VonageTemplateId",
                table: "GuestJourneyMessagesTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VonageTemplateStatus",
                table: "GuestJourneyMessagesTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VonageStatus",
                table: "ChannelMessages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Buttons",
                table: "GuestJourneyMessagesTemplates");

            migrationBuilder.DropColumn(
                name: "VonageTemplateId",
                table: "GuestJourneyMessagesTemplates");

            migrationBuilder.DropColumn(
                name: "VonageTemplateStatus",
                table: "GuestJourneyMessagesTemplates");

            migrationBuilder.DropColumn(
                name: "VonageStatus",
                table: "ChannelMessages");
        }
    }
}

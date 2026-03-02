using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddColumnGuestWelcomeMessageToCheckInBuilder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuestWelcomeMessage",
                table: "CustomerGuestsCheckInFormBuilders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestWelcomeMessage",
                table: "CustomerGuestsCheckInFormBuilders");
        }
    }
}

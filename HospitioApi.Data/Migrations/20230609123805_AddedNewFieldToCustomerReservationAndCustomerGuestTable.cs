using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddedNewFieldToCustomerReservationAndCustomerGuestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isCheckInCompleted",
                table: "CustomerReservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isSkipCheckIn",
                table: "CustomerReservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCoGuest",
                table: "CustomerGuests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCheckInCompleted",
                table: "CustomerReservations");

            migrationBuilder.DropColumn(
                name: "isSkipCheckIn",
                table: "CustomerReservations");

            migrationBuilder.DropColumn(
                name: "IsCoGuest",
                table: "CustomerGuests");
        }
    }
}

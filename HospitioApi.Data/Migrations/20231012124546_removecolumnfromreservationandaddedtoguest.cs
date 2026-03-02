using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class removecolumnfromreservationandaddedtoguest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCheckInCompleted",
                table: "CustomerReservations");

            migrationBuilder.DropColumn(
                name: "isSkipCheckIn",
                table: "CustomerReservations");

            migrationBuilder.AddColumn<bool>(
                name: "isCheckInCompleted",
                table: "CustomerGuests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isSkipCheckIn",
                table: "CustomerGuests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCheckInCompleted",
                table: "CustomerGuests");

            migrationBuilder.DropColumn(
                name: "isSkipCheckIn",
                table: "CustomerGuests");

            migrationBuilder.AddColumn<bool>(
                name: "isCheckInCompleted",
                table: "CustomerReservations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isSkipCheckIn",
                table: "CustomerReservations",
                type: "bit",
                nullable: true);
        }
    }
}

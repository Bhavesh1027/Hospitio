using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddedNewFieldsToCustomerGuestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookingChannel",
                table: "CustomerGuests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartingFlightDate",
                table: "CustomerGuests",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingChannel",
                table: "CustomerGuests");

            migrationBuilder.DropColumn(
                name: "DepartingFlightDate",
                table: "CustomerGuests");
        }
    }
}

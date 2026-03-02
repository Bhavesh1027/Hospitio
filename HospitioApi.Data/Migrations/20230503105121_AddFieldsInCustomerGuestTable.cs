using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddFieldsInCustomerGuestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "CustomerGuests",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vat",
                table: "CustomerGuests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "CustomerGuests");

            migrationBuilder.DropColumn(
                name: "Vat",
                table: "CustomerGuests");
        }
    }
}

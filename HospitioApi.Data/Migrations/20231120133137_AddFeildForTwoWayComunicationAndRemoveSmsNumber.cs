using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddFeildForTwoWayComunicationAndRemoveSmsNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SMSCountry",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SMSNumber",
                table: "Customers");

            migrationBuilder.AddColumn<bool>(
                name: "IsTwoWayComunication",
                table: "Customers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTwoWayComunication",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "SMSCountry",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SMSNumber",
                table: "Customers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class addedguidtypeandremovecenturionhotelcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CenturianHotelCode",
                table: "Customers");

            migrationBuilder.AddColumn<byte>(
                name: "GuidType",
                table: "Customers",
                type: "tinyint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuidType",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "CenturianHotelCode",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

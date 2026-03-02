using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class addedaccesscodeandkeyidtoguest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppAccessCode",
                table: "CustomerGuests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KeyId",
                table: "CustomerGuests",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppAccessCode",
                table: "CustomerGuests");

            migrationBuilder.DropColumn(
                name: "KeyId",
                table: "CustomerGuests");
        }
    }
}

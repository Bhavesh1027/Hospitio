using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_LongitudeAndLatitude_Cloumn_In_CustomerPropertyExtraDetailsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "CustomerPropertyExtraDetails");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "CustomerPropertyExtraDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "CustomerPropertyExtraDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "CustomerPropertyExtraDetails");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "CustomerPropertyExtraDetails");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "CustomerPropertyExtraDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}

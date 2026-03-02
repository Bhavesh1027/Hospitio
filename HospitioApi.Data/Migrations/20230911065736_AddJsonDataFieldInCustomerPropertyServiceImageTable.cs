using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddJsonDataFieldInCustomerPropertyServiceImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerPropertyServiceImages",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerPropertyServiceImages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerPropertyServiceImages");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerPropertyServiceImages");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddJsonDataColumnInPropertyInformationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerPropertyServices",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerPropertyServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerPropertyInformations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerPropertyInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerPropertyGalleries",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerPropertyGalleries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerPropertyExtras",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerPropertyExtras",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerPropertyExtraDetails",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerPropertyExtraDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerPropertyEmergencyNumbers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerPropertyEmergencyNumbers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerPropertyServices");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerPropertyServices");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerPropertyInformations");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerPropertyInformations");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerPropertyGalleries");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerPropertyGalleries");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerPropertyExtras");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerPropertyExtras");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerPropertyExtraDetails");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerPropertyExtraDetails");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerPropertyEmergencyNumbers");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerPropertyEmergencyNumbers");
        }
    }
}

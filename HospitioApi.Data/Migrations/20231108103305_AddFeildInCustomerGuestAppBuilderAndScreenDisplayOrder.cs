using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddFeildInCustomerGuestAppBuilderAndScreenDisplayOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "ScreenDisplayOrderAndStatuses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenJsonData",
                table: "ScreenDisplayOrderAndStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppBuilders",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppBuilders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "ScreenDisplayOrderAndStatuses");

            migrationBuilder.DropColumn(
                name: "ScreenJsonData",
                table: "ScreenDisplayOrderAndStatuses");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppBuilders");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppBuilders");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddJsonDatFieldInTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppRoomServiceItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppRoomServiceItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppRoomServiceCategories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppRoomServiceCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppReceptionItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppReceptionItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppReceptionCategories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppReceptionCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppHousekeepingItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppHousekeepingItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppHousekeepingCategories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppHousekeepingCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppConciergeItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppConciergeItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppConciergeCategories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppConciergeCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppRoomServiceItems");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppRoomServiceItems");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppRoomServiceCategories");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppRoomServiceCategories");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppReceptionItems");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppReceptionItems");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppReceptionCategories");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppReceptionCategories");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppHousekeepingItems");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppHousekeepingItems");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppHousekeepingCategories");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppHousekeepingCategories");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppConciergeItems");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppConciergeItems");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppConciergeCategories");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppConciergeCategories");
        }
    }
}

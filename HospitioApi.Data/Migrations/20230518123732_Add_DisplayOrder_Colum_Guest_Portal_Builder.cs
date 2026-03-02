using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_DisplayOrder_Colum_Guest_Portal_Builder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "CustomerGuestAppReceptionItems");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "CustomerGuestAppHousekeepingItems");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerGuestAppRoomServiceItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerGuestAppRoomServiceCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerGuestAppReceptionItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerGuestAppReceptionCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerGuestAppHousekeepingItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerGuestAppHousekeepingCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerGuestAppConciergeItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerGuestAppConciergeCategories",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerGuestAppRoomServiceItems");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerGuestAppRoomServiceCategories");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerGuestAppReceptionItems");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerGuestAppReceptionCategories");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerGuestAppHousekeepingItems");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerGuestAppHousekeepingCategories");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerGuestAppConciergeItems");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerGuestAppConciergeCategories");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "CustomerGuestAppReceptionItems",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "CustomerGuestAppHousekeepingItems",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_JsonDataIsPublish_Column_Add_EnhanceYourStayTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppEnhanceYourStayItemsImages",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppEnhanceYourStayItemsImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppEnhanceYourStayItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppEnhanceYourStayItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppEnhanceYourStayCategoryItemsExtras",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppEnhanceYourStayCategoryItemsExtras",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "CustomerGuestAppEnhanceYourStayCategories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "CustomerGuestAppEnhanceYourStayCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppEnhanceYourStayItemsImages");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppEnhanceYourStayItemsImages");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppEnhanceYourStayItems");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppEnhanceYourStayItems");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppEnhanceYourStayCategoryItemsExtras");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppEnhanceYourStayCategoryItemsExtras");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "CustomerGuestAppEnhanceYourStayCategories");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "CustomerGuestAppEnhanceYourStayCategories");
        }
    }
}

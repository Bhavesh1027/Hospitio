using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_Filed_in_Permission_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEdit",
                table: "Permissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReply",
                table: "Permissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSend",
                table: "Permissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpload",
                table: "Permissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsView",
                table: "Permissions",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEdit",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsReply",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsSend",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsUpload",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsView",
                table: "Permissions");
        }
    }
}

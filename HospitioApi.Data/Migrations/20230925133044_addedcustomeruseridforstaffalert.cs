using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class addedcustomeruseridforstaffalert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerUserId",
                table: "CustomerStaffAlerts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerStaffAlerts_CustomerUserId",
                table: "CustomerStaffAlerts",
                column: "CustomerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerStaffAlerts_CustomerUsers_CustomerUserId",
                table: "CustomerStaffAlerts",
                column: "CustomerUserId",
                principalTable: "CustomerUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerStaffAlerts_CustomerUsers_CustomerUserId",
                table: "CustomerStaffAlerts");

            migrationBuilder.DropIndex(
                name: "IX_CustomerStaffAlerts_CustomerUserId",
                table: "CustomerStaffAlerts");

            migrationBuilder.DropColumn(
                name: "CustomerUserId",
                table: "CustomerStaffAlerts");
        }
    }
}

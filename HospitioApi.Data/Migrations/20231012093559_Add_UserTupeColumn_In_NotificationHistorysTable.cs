using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_UserTupeColumn_In_NotificationHistorysTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationHistorys_Customers_CustomerId",
                table: "NotificationHistorys");

            migrationBuilder.DropIndex(
                name: "IX_NotificationHistorys_CustomerId",
                table: "NotificationHistorys");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "NotificationHistorys",
                newName: "UserId");

            migrationBuilder.AddColumn<byte>(
                name: "UserType",
                table: "NotificationHistorys",
                type: "tinyint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "NotificationHistorys");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NotificationHistorys",
                newName: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationHistorys_CustomerId",
                table: "NotificationHistorys",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationHistorys_Customers_CustomerId",
                table: "NotificationHistorys",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}

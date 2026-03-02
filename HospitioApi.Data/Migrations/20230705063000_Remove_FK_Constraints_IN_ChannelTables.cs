using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Remove_FK_Constraints_IN_ChannelTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelUsers_CustomerGuests_CustomerGuestsId",
                table: "ChannelUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelUsers_CustomerUsers_CustomerUsersId",
                table: "ChannelUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelUsers_Users_UserId",
                table: "ChannelUsers");

            migrationBuilder.DropIndex(
                name: "IX_ChannelUsers_CustomerGuestsId",
                table: "ChannelUsers");

            migrationBuilder.DropIndex(
                name: "IX_ChannelUsers_CustomerUsersId",
                table: "ChannelUsers");

            migrationBuilder.DropIndex(
                name: "IX_ChannelUsers_UserId",
                table: "ChannelUsers");

            migrationBuilder.DropColumn(
                name: "CustomerGuestsId",
                table: "ChannelUsers");

            migrationBuilder.DropColumn(
                name: "CustomerUsersId",
                table: "ChannelUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerGuestsId",
                table: "ChannelUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerUsersId",
                table: "ChannelUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelUsers_CustomerGuestsId",
                table: "ChannelUsers",
                column: "CustomerGuestsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelUsers_CustomerUsersId",
                table: "ChannelUsers",
                column: "CustomerUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelUsers_UserId",
                table: "ChannelUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelUsers_CustomerGuests_CustomerGuestsId",
                table: "ChannelUsers",
                column: "CustomerGuestsId",
                principalTable: "CustomerGuests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelUsers_CustomerUsers_CustomerUsersId",
                table: "ChannelUsers",
                column: "CustomerUsersId",
                principalTable: "CustomerUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelUsers_Users_UserId",
                table: "ChannelUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

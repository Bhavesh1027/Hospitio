using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class makechannelTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_CustomerGuests_channelUsersId",
                table: "Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_Channels_CustomerUsers_channelUsersId",
                table: "Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelUsers_CustomerGuests_UserId",
                table: "ChannelUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelUsers_CustomerUsers_UserId",
                table: "ChannelUsers");

            migrationBuilder.DropIndex(
                name: "IX_ChannelUsers_UserId1",
                table: "ChannelUsers");

            migrationBuilder.DropIndex(
                name: "IX_Channels_channelUsersId",
                table: "Channels");

            migrationBuilder.RenameColumn(
                name: "channelUsersId",
                table: "Channels",
                newName: "channelUserID");

            migrationBuilder.RenameColumn(
                name: "CreatedFrom",
                table: "Channels",
                newName: "CreateForm");

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

            migrationBuilder.AlterColumn<string>(
                name: "Uuid",
                table: "Channels",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelUsers_CustomerGuestsId",
                table: "ChannelUsers",
                column: "CustomerGuestsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelUsers_CustomerUsersId",
                table: "ChannelUsers",
                column: "CustomerUsersId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelUsers_CustomerGuests_CustomerGuestsId",
                table: "ChannelUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelUsers_CustomerUsers_CustomerUsersId",
                table: "ChannelUsers");

            migrationBuilder.DropIndex(
                name: "IX_ChannelUsers_CustomerGuestsId",
                table: "ChannelUsers");

            migrationBuilder.DropIndex(
                name: "IX_ChannelUsers_CustomerUsersId",
                table: "ChannelUsers");

            migrationBuilder.DropColumn(
                name: "CustomerGuestsId",
                table: "ChannelUsers");

            migrationBuilder.DropColumn(
                name: "CustomerUsersId",
                table: "ChannelUsers");

            migrationBuilder.RenameColumn(
                name: "channelUserID",
                table: "Channels",
                newName: "channelUsersId");

            migrationBuilder.RenameColumn(
                name: "CreateForm",
                table: "Channels",
                newName: "CreatedFrom");

            migrationBuilder.AlterColumn<string>(
                name: "Uuid",
                table: "Channels",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelUsers_UserId1",
                table: "ChannelUsers",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_channelUsersId",
                table: "Channels",
                column: "channelUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_CustomerGuests_channelUsersId",
                table: "Channels",
                column: "channelUsersId",
                principalTable: "CustomerGuests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_CustomerUsers_channelUsersId",
                table: "Channels",
                column: "channelUsersId",
                principalTable: "CustomerUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelUsers_CustomerGuests_UserId",
                table: "ChannelUsers",
                column: "UserId",
                principalTable: "CustomerGuests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelUsers_CustomerUsers_UserId",
                table: "ChannelUsers",
                column: "UserId",
                principalTable: "CustomerUsers",
                principalColumn: "Id");
        }
    }
}

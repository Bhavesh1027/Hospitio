using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Addchannels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    channelUsersId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channels_CustomerGuests_channelUsersId",
                        column: x => x.channelUsersId,
                        principalTable: "CustomerGuests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Channels_CustomerUsers_channelUsersId",
                        column: x => x.channelUsersId,
                        principalTable: "CustomerUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChannelMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageSender = table.Column<byte>(type: "tinyint", nullable: true),
                    Source = table.Column<byte>(type: "tinyint", nullable: true),
                    MsgReqType = table.Column<byte>(type: "tinyint", nullable: true),
                    Attachment = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TranslateMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelMessages_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelMessages_GuestRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "GuestRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChannelUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    LastMessageReadTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastMessageReadId = table.Column<int>(type: "int", nullable: true),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelUsers_ChannelMessages_LastMessageReadId",
                        column: x => x.LastMessageReadId,
                        principalTable: "ChannelMessages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChannelUsers_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelUsers_CustomerGuests_UserId",
                        column: x => x.UserId,
                        principalTable: "CustomerGuests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChannelUsers_CustomerUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "CustomerUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChannelUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMessages_ChannelId",
                table: "ChannelMessages",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMessages_RequestId",
                table: "ChannelMessages",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_channelUsersId",
                table: "Channels",
                column: "channelUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelUsers_ChannelId",
                table: "ChannelUsers",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelUsers_LastMessageReadId",
                table: "ChannelUsers",
                column: "LastMessageReadId",
                unique: true,
                filter: "[LastMessageReadId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelUsers_UserId",
                table: "ChannelUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelUsers_UserId1",
                table: "ChannelUsers",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelUsers");

            migrationBuilder.DropTable(
                name: "ChannelMessages");

            migrationBuilder.DropTable(
                name: "Channels");
        }
    }
}

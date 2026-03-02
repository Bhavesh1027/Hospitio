using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class addedenhancestayrequestidinchannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnhanceStayItemsGuestRequestId",
                table: "ChannelMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMessages_EnhanceStayItemsGuestRequestId",
                table: "ChannelMessages",
                column: "EnhanceStayItemsGuestRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMessages_EnhanceStayItemsGuestRequests_EnhanceStayItemsGuestRequestId",
                table: "ChannelMessages",
                column: "EnhanceStayItemsGuestRequestId",
                principalTable: "EnhanceStayItemsGuestRequests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMessages_EnhanceStayItemsGuestRequests_EnhanceStayItemsGuestRequestId",
                table: "ChannelMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChannelMessages_EnhanceStayItemsGuestRequestId",
                table: "ChannelMessages");

            migrationBuilder.DropColumn(
                name: "EnhanceStayItemsGuestRequestId",
                table: "ChannelMessages");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddedNewFieldToGuestRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuestRequests_CustomerGuestAppEnhanceYourStayItems_CustomerGuestAppEnhanceYourStayItemId",
                table: "GuestRequests");

            migrationBuilder.RenameColumn(
                name: "CustomerGuestAppEnhanceYourStayItemId",
                table: "GuestRequests",
                newName: "CustomerGuestAppEnhanceYourStayCategoryItemsExtraId");

            migrationBuilder.RenameIndex(
                name: "IX_GuestRequests_CustomerGuestAppEnhanceYourStayItemId",
                table: "GuestRequests",
                newName: "IX_GuestRequests_CustomerGuestAppEnhanceYourStayCategoryItemsExtraId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuestRequests_CustomerGuestAppEnhanceYourStayCategoryItemsExtras_CustomerGuestAppEnhanceYourStayCategoryItemsExtraId",
                table: "GuestRequests",
                column: "CustomerGuestAppEnhanceYourStayCategoryItemsExtraId",
                principalTable: "CustomerGuestAppEnhanceYourStayCategoryItemsExtras",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuestRequests_CustomerGuestAppEnhanceYourStayCategoryItemsExtras_CustomerGuestAppEnhanceYourStayCategoryItemsExtraId",
                table: "GuestRequests");

            migrationBuilder.RenameColumn(
                name: "CustomerGuestAppEnhanceYourStayCategoryItemsExtraId",
                table: "GuestRequests",
                newName: "CustomerGuestAppEnhanceYourStayItemId");

            migrationBuilder.RenameIndex(
                name: "IX_GuestRequests_CustomerGuestAppEnhanceYourStayCategoryItemsExtraId",
                table: "GuestRequests",
                newName: "IX_GuestRequests_CustomerGuestAppEnhanceYourStayItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuestRequests_CustomerGuestAppEnhanceYourStayItems_CustomerGuestAppEnhanceYourStayItemId",
                table: "GuestRequests",
                column: "CustomerGuestAppEnhanceYourStayItemId",
                principalTable: "CustomerGuestAppEnhanceYourStayItems",
                principalColumn: "Id");
        }
    }
}

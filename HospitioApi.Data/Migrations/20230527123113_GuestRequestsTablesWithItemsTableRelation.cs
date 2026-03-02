using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GuestRequestsTablesWithItemsTableRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuestRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    RequestType = table.Column<byte>(type: "tinyint", nullable: true),
                    CustomerGuestAppConciergeItemId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppEnhanceYourStayItemId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppHousekeepingItemId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppRoomServiceItemId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppReceptionItemId = table.Column<int>(type: "int", nullable: true),
                    GuestId = table.Column<int>(type: "int", nullable: true),
                    MonthValue = table.Column<int>(type: "int", nullable: true),
                    DayValue = table.Column<int>(type: "int", nullable: true),
                    YearValue = table.Column<int>(type: "int", nullable: true),
                    HourValue = table.Column<int>(type: "int", nullable: true),
                    MinuteValue = table.Column<int>(type: "int", nullable: true),
                    PickupLocation = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuestRequests_CustomerGuestAppConciergeItems_CustomerGuestAppConciergeItemId",
                        column: x => x.CustomerGuestAppConciergeItemId,
                        principalTable: "CustomerGuestAppConciergeItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GuestRequests_CustomerGuestAppEnhanceYourStayItems_CustomerGuestAppEnhanceYourStayItemId",
                        column: x => x.CustomerGuestAppEnhanceYourStayItemId,
                        principalTable: "CustomerGuestAppEnhanceYourStayItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GuestRequests_CustomerGuestAppHousekeepingItems_CustomerGuestAppHousekeepingItemId",
                        column: x => x.CustomerGuestAppHousekeepingItemId,
                        principalTable: "CustomerGuestAppHousekeepingItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GuestRequests_CustomerGuestAppReceptionItems_CustomerGuestAppReceptionItemId",
                        column: x => x.CustomerGuestAppReceptionItemId,
                        principalTable: "CustomerGuestAppReceptionItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GuestRequests_CustomerGuestAppRoomServiceItems_CustomerGuestAppRoomServiceItemId",
                        column: x => x.CustomerGuestAppRoomServiceItemId,
                        principalTable: "CustomerGuestAppRoomServiceItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GuestRequests_CustomerGuests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "CustomerGuests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GuestRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuestRequests_CustomerGuestAppConciergeItemId",
                table: "GuestRequests",
                column: "CustomerGuestAppConciergeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestRequests_CustomerGuestAppEnhanceYourStayItemId",
                table: "GuestRequests",
                column: "CustomerGuestAppEnhanceYourStayItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestRequests_CustomerGuestAppHousekeepingItemId",
                table: "GuestRequests",
                column: "CustomerGuestAppHousekeepingItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestRequests_CustomerGuestAppReceptionItemId",
                table: "GuestRequests",
                column: "CustomerGuestAppReceptionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestRequests_CustomerGuestAppRoomServiceItemId",
                table: "GuestRequests",
                column: "CustomerGuestAppRoomServiceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestRequests_CustomerId",
                table: "GuestRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestRequests_GuestId",
                table: "GuestRequests",
                column: "GuestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuestRequests");
        }
    }
}

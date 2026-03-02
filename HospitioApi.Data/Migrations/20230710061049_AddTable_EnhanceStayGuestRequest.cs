using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddTable_EnhanceStayGuestRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnhanceStayItemsGuestRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    GuestId = table.Column<int>(type: "int", nullable: false),
                    CustomerGuestAppEnhanceYourStayItemId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: true),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnhanceStayItemsGuestRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnhanceStayItemsGuestRequests_CustomerGuestAppEnhanceYourStayItems_CustomerGuestAppEnhanceYourStayItemId",
                        column: x => x.CustomerGuestAppEnhanceYourStayItemId,
                        principalTable: "CustomerGuestAppEnhanceYourStayItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnhanceStayItemsGuestRequests_CustomerGuests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "CustomerGuests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnhanceStayItemsGuestRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnhanceStayItemExtraGuestRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnhanceStayItemsGuestRequestId = table.Column<int>(type: "int", nullable: false),
                    CustomerGuestAppEnhanceYourStayCategoryItemsExtraId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: true),
                    Day = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Hour = table.Column<int>(type: "int", nullable: true),
                    Minute = table.Column<int>(type: "int", nullable: true),
                    PickupLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qunatity = table.Column<int>(type: "int", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnhanceStayItemExtraGuestRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnhanceStayItemExtraGuestRequests_CustomerGuestAppEnhanceYourStayCategoryItemsExtras_CustomerGuestAppEnhanceYourStayCategory~",
                        column: x => x.CustomerGuestAppEnhanceYourStayCategoryItemsExtraId,
                        principalTable: "CustomerGuestAppEnhanceYourStayCategoryItemsExtras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnhanceStayItemExtraGuestRequests_EnhanceStayItemsGuestRequests_EnhanceStayItemsGuestRequestId",
                        column: x => x.EnhanceStayItemsGuestRequestId,
                        principalTable: "EnhanceStayItemsGuestRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnhanceStayItemExtraGuestRequests_CustomerGuestAppEnhanceYourStayCategoryItemsExtraId",
                table: "EnhanceStayItemExtraGuestRequests",
                column: "CustomerGuestAppEnhanceYourStayCategoryItemsExtraId");

            migrationBuilder.CreateIndex(
                name: "IX_EnhanceStayItemExtraGuestRequests_EnhanceStayItemsGuestRequestId",
                table: "EnhanceStayItemExtraGuestRequests",
                column: "EnhanceStayItemsGuestRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EnhanceStayItemsGuestRequests_CustomerGuestAppEnhanceYourStayItemId",
                table: "EnhanceStayItemsGuestRequests",
                column: "CustomerGuestAppEnhanceYourStayItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EnhanceStayItemsGuestRequests_CustomerId",
                table: "EnhanceStayItemsGuestRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_EnhanceStayItemsGuestRequests_GuestId",
                table: "EnhanceStayItemsGuestRequests",
                column: "GuestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnhanceStayItemExtraGuestRequests");

            migrationBuilder.DropTable(
                name: "EnhanceStayItemsGuestRequests");
        }
    }
}

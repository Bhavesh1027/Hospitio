using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CustomersPortalTablesV7GuestAppRoomServiceAndItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerGuestAppRoomServiceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerGuestAppBuilderId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGuestAppRoomServiceCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppRoomServiceCategories_CustomerGuestAppBuilders_CustomerGuestAppBuilderId",
                        column: x => x.CustomerGuestAppBuilderId,
                        principalTable: "CustomerGuestAppBuilders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppRoomServiceCategories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerGuestAppRoomServiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppBuilderId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppRoomServiceCategoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ItemsMonth = table.Column<bool>(type: "bit", nullable: true),
                    ItemsDay = table.Column<bool>(type: "bit", nullable: true),
                    ItemsMinute = table.Column<bool>(type: "bit", nullable: true),
                    ItemsHour = table.Column<bool>(type: "bit", nullable: true),
                    QuantityBar = table.Column<bool>(type: "bit", nullable: true),
                    PickupLocation = table.Column<bool>(type: "bit", nullable: true),
                    DestinationLocation = table.Column<bool>(type: "bit", nullable: true),
                    Comment = table.Column<bool>(type: "bit", nullable: true),
                    IsPriceEnable = table.Column<bool>(type: "bit", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGuestAppRoomServiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppRoomServiceItems_CustomerGuestAppBuilders_CustomerGuestAppBuilderId",
                        column: x => x.CustomerGuestAppBuilderId,
                        principalTable: "CustomerGuestAppBuilders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppRoomServiceItems_CustomerGuestAppRoomServiceCategories_CustomerGuestAppRoomServiceCategoryId",
                        column: x => x.CustomerGuestAppRoomServiceCategoryId,
                        principalTable: "CustomerGuestAppRoomServiceCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppRoomServiceItems_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppRoomServiceCategories_CustomerGuestAppBuilderId",
                table: "CustomerGuestAppRoomServiceCategories",
                column: "CustomerGuestAppBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppRoomServiceCategories_CustomerId",
                table: "CustomerGuestAppRoomServiceCategories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppRoomServiceItems_CustomerGuestAppBuilderId",
                table: "CustomerGuestAppRoomServiceItems",
                column: "CustomerGuestAppBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppRoomServiceItems_CustomerGuestAppRoomServiceCategoryId",
                table: "CustomerGuestAppRoomServiceItems",
                column: "CustomerGuestAppRoomServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppRoomServiceItems_CustomerId",
                table: "CustomerGuestAppRoomServiceItems",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerGuestAppRoomServiceItems");

            migrationBuilder.DropTable(
                name: "CustomerGuestAppRoomServiceCategories");
        }
    }
}

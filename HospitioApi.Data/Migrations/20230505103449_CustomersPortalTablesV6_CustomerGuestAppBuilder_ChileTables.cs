using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CustomersPortalTablesV6_CustomerGuestAppBuilder_ChileTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerGuestAppHousekeepingCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerGuestAppBuilderId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGuestAppHousekeepingCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppHousekeepingCategories_CustomerGuestAppBuilders_CustomerGuestAppBuilderId",
                        column: x => x.CustomerGuestAppBuilderId,
                        principalTable: "CustomerGuestAppBuilders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppHousekeepingCategories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerGuestAppReceptionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerGuestAppBuilderId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGuestAppReceptionCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppReceptionCategories_CustomerGuestAppBuilders_CustomerGuestAppBuilderId",
                        column: x => x.CustomerGuestAppBuilderId,
                        principalTable: "CustomerGuestAppBuilders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppReceptionCategories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerGuestAppHousekeepingItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppBuilderId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppHousekeepingCategoryId = table.Column<int>(type: "int", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
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
                    table.PrimaryKey("PK_CustomerGuestAppHousekeepingItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppHousekeepingItems_CustomerGuestAppBuilders_CustomerGuestAppBuilderId",
                        column: x => x.CustomerGuestAppBuilderId,
                        principalTable: "CustomerGuestAppBuilders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppHousekeepingItems_CustomerGuestAppHousekeepingCategories_CustomerGuestAppHousekeepingCategoryId",
                        column: x => x.CustomerGuestAppHousekeepingCategoryId,
                        principalTable: "CustomerGuestAppHousekeepingCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppHousekeepingItems_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerGuestAppReceptionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppBuilderId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppReceptionCategoryId = table.Column<int>(type: "int", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
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
                    table.PrimaryKey("PK_CustomerGuestAppReceptionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppReceptionItems_CustomerGuestAppBuilders_CustomerGuestAppBuilderId",
                        column: x => x.CustomerGuestAppBuilderId,
                        principalTable: "CustomerGuestAppBuilders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppReceptionItems_CustomerGuestAppReceptionCategories_CustomerGuestAppReceptionCategoryId",
                        column: x => x.CustomerGuestAppReceptionCategoryId,
                        principalTable: "CustomerGuestAppReceptionCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppReceptionItems_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppHousekeepingCategories_CustomerGuestAppBuilderId",
                table: "CustomerGuestAppHousekeepingCategories",
                column: "CustomerGuestAppBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppHousekeepingCategories_CustomerId",
                table: "CustomerGuestAppHousekeepingCategories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppHousekeepingItems_CustomerGuestAppBuilderId",
                table: "CustomerGuestAppHousekeepingItems",
                column: "CustomerGuestAppBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppHousekeepingItems_CustomerGuestAppHousekeepingCategoryId",
                table: "CustomerGuestAppHousekeepingItems",
                column: "CustomerGuestAppHousekeepingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppHousekeepingItems_CustomerId",
                table: "CustomerGuestAppHousekeepingItems",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppReceptionCategories_CustomerGuestAppBuilderId",
                table: "CustomerGuestAppReceptionCategories",
                column: "CustomerGuestAppBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppReceptionCategories_CustomerId",
                table: "CustomerGuestAppReceptionCategories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppReceptionItems_CustomerGuestAppBuilderId",
                table: "CustomerGuestAppReceptionItems",
                column: "CustomerGuestAppBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppReceptionItems_CustomerGuestAppReceptionCategoryId",
                table: "CustomerGuestAppReceptionItems",
                column: "CustomerGuestAppReceptionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppReceptionItems_CustomerId",
                table: "CustomerGuestAppReceptionItems",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerGuestAppHousekeepingItems");

            migrationBuilder.DropTable(
                name: "CustomerGuestAppReceptionItems");

            migrationBuilder.DropTable(
                name: "CustomerGuestAppHousekeepingCategories");

            migrationBuilder.DropTable(
                name: "CustomerGuestAppReceptionCategories");
        }
    }
}

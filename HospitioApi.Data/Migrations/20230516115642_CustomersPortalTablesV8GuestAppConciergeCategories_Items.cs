using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CustomersPortalTablesV8GuestAppConciergeCategories_Items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerGuestAppConciergeCategories",
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
                    table.PrimaryKey("PK_CustomerGuestAppConciergeCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppConciergeCategories_CustomerGuestAppBuilders_CustomerGuestAppBuilderId",
                        column: x => x.CustomerGuestAppBuilderId,
                        principalTable: "CustomerGuestAppBuilders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppConciergeCategories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerGuestAppConciergeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppBuilderId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppConciergeCategoryId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_CustomerGuestAppConciergeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppConciergeItems_CustomerGuestAppBuilders_CustomerGuestAppBuilderId",
                        column: x => x.CustomerGuestAppBuilderId,
                        principalTable: "CustomerGuestAppBuilders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppConciergeItems_CustomerGuestAppConciergeCategories_CustomerGuestAppConciergeCategoryId",
                        column: x => x.CustomerGuestAppConciergeCategoryId,
                        principalTable: "CustomerGuestAppConciergeCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppConciergeItems_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppConciergeCategories_CustomerGuestAppBuilderId",
                table: "CustomerGuestAppConciergeCategories",
                column: "CustomerGuestAppBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppConciergeCategories_CustomerId",
                table: "CustomerGuestAppConciergeCategories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppConciergeItems_CustomerGuestAppBuilderId",
                table: "CustomerGuestAppConciergeItems",
                column: "CustomerGuestAppBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppConciergeItems_CustomerGuestAppConciergeCategoryId",
                table: "CustomerGuestAppConciergeItems",
                column: "CustomerGuestAppConciergeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppConciergeItems_CustomerId",
                table: "CustomerGuestAppConciergeItems",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerGuestAppConciergeItems");

            migrationBuilder.DropTable(
                name: "CustomerGuestAppConciergeCategories");
        }
    }
}

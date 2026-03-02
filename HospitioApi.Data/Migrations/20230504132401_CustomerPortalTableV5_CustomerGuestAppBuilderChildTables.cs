using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CustomerPortalTableV5_CustomerGuestAppBuilderChildTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SecondaryMessage",
                table: "CustomerGuestAppBuilders",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "CustomerGuestAppBuilders",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerGuestAppEnhanceYourStayCategories",
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
                    table.PrimaryKey("PK_CustomerGuestAppEnhanceYourStayCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppEnhanceYourStayCategories_CustomerGuestAppBuilders_CustomerGuestAppBuilderId",
                        column: x => x.CustomerGuestAppBuilderId,
                        principalTable: "CustomerGuestAppBuilders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppEnhanceYourStayCategories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerGuestAppEnhanceYourStayItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerGuestAppBuilderId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppBuilderCategoryId = table.Column<int>(type: "int", nullable: true),
                    Badge = table.Column<byte>(type: "tinyint", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonType = table.Column<byte>(type: "tinyint", nullable: true),
                    ButtonText = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    ChargeType = table.Column<byte>(type: "tinyint", nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_CustomerGuestAppEnhanceYourStayItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppEnhanceYourStayItems_CustomerGuestAppBuilders_CustomerGuestAppBuilderId",
                        column: x => x.CustomerGuestAppBuilderId,
                        principalTable: "CustomerGuestAppBuilders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppEnhanceYourStayItems_CustomerGuestAppEnhanceYourStayCategories_CustomerGuestAppBuilderCategoryId",
                        column: x => x.CustomerGuestAppBuilderCategoryId,
                        principalTable: "CustomerGuestAppEnhanceYourStayCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppEnhanceYourStayItems_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerGuestAppEnhanceYourStayCategoryItemsExtras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerGuestAppEnhanceYourStayItemId = table.Column<int>(type: "int", nullable: true),
                    QueType = table.Column<byte>(type: "tinyint", nullable: true),
                    Questions = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OptionValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGuestAppEnhanceYourStayCategoryItemsExtras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppEnhanceYourStayCategoryItemsExtras_CustomerGuestAppEnhanceYourStayItems_CustomerGuestAppEnhanceYourStayItemId",
                        column: x => x.CustomerGuestAppEnhanceYourStayItemId,
                        principalTable: "CustomerGuestAppEnhanceYourStayItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerGuestAppEnhanceYourStayItemsImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerGuestAppEnhanceYourStayItemId = table.Column<int>(type: "int", nullable: true),
                    ItemsImages = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisaplayOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGuestAppEnhanceYourStayItemsImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppEnhanceYourStayItemsImages_CustomerGuestAppEnhanceYourStayItems_CustomerGuestAppEnhanceYourStayItemId",
                        column: x => x.CustomerGuestAppEnhanceYourStayItemId,
                        principalTable: "CustomerGuestAppEnhanceYourStayItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppEnhanceYourStayCategories_CustomerGuestAppBuilderId",
                table: "CustomerGuestAppEnhanceYourStayCategories",
                column: "CustomerGuestAppBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppEnhanceYourStayCategories_CustomerId",
                table: "CustomerGuestAppEnhanceYourStayCategories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppEnhanceYourStayCategoryItemsExtras_CustomerGuestAppEnhanceYourStayItemId",
                table: "CustomerGuestAppEnhanceYourStayCategoryItemsExtras",
                column: "CustomerGuestAppEnhanceYourStayItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppEnhanceYourStayItems_CustomerGuestAppBuilderCategoryId",
                table: "CustomerGuestAppEnhanceYourStayItems",
                column: "CustomerGuestAppBuilderCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppEnhanceYourStayItems_CustomerGuestAppBuilderId",
                table: "CustomerGuestAppEnhanceYourStayItems",
                column: "CustomerGuestAppBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppEnhanceYourStayItems_CustomerId",
                table: "CustomerGuestAppEnhanceYourStayItems",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppEnhanceYourStayItemsImages_CustomerGuestAppEnhanceYourStayItemId",
                table: "CustomerGuestAppEnhanceYourStayItemsImages",
                column: "CustomerGuestAppEnhanceYourStayItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerGuestAppEnhanceYourStayCategoryItemsExtras");

            migrationBuilder.DropTable(
                name: "CustomerGuestAppEnhanceYourStayItemsImages");

            migrationBuilder.DropTable(
                name: "CustomerGuestAppEnhanceYourStayItems");

            migrationBuilder.DropTable(
                name: "CustomerGuestAppEnhanceYourStayCategories");

            migrationBuilder.AlterColumn<string>(
                name: "SecondaryMessage",
                table: "CustomerGuestAppBuilders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "CustomerGuestAppBuilders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);
        }
    }
}

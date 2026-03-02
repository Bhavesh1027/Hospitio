using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class MajorDBChanges_WithAddModule_Product_TablesToDBContaxt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuesionAnswers_Qacategories_QacategorieId",
                table: "QuesionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Customers_CustomerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CustomerId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Qacategories",
                table: "Qacategories");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Qacategories",
                newName: "QuesionAnswerCategories");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentType",
                table: "QuesionAnswerAttachements",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupLeaderId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentMangerId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuesionAnswerCategories",
                table: "QuesionAnswerCategories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ModuleType = table.Column<byte>(type: "tinyint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ExpiresUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModuleServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleServices_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductModules_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductModuleServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductModuleId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ModuleServiceId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductModuleServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductModuleServices_ModuleServices_ModuleServiceId",
                        column: x => x.ModuleServiceId,
                        principalTable: "ModuleServices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductModuleServices_ProductModules_ProductModuleId",
                        column: x => x.ProductModuleId,
                        principalTable: "ProductModules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductModuleServices_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupLeaderId",
                table: "Groups",
                column: "GroupLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentMangerId",
                table: "Departments",
                column: "DepartmentMangerId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleServices_ModuleId",
                table: "ModuleServices",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductModules_ModuleId",
                table: "ProductModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductModules_ProductId",
                table: "ProductModules",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductModuleServices_ModuleServiceId",
                table: "ProductModuleServices",
                column: "ModuleServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductModuleServices_ProductId",
                table: "ProductModuleServices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductModuleServices_ProductModuleId",
                table: "ProductModuleServices",
                column: "ProductModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_DepartmentMangerId",
                table: "Departments",
                column: "DepartmentMangerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_GroupLeaderId",
                table: "Groups",
                column: "GroupLeaderId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuesionAnswers_QuesionAnswerCategories_QacategorieId",
                table: "QuesionAnswers",
                column: "QacategorieId",
                principalTable: "QuesionAnswerCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_DepartmentMangerId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_GroupLeaderId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_QuesionAnswers_QuesionAnswerCategories_QacategorieId",
                table: "QuesionAnswers");

            migrationBuilder.DropTable(
                name: "ProductModuleServices");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "ModuleServices");

            migrationBuilder.DropTable(
                name: "ProductModules");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Groups_GroupLeaderId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Departments_DepartmentMangerId",
                table: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuesionAnswerCategories",
                table: "QuesionAnswerCategories");

            migrationBuilder.DropColumn(
                name: "AttachmentType",
                table: "QuesionAnswerAttachements");

            migrationBuilder.DropColumn(
                name: "GroupLeaderId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "DepartmentMangerId",
                table: "Departments");

            migrationBuilder.RenameTable(
                name: "QuesionAnswerCategories",
                newName: "Qacategories");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Qacategories",
                table: "Qacategories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CustomerId",
                table: "Users",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuesionAnswers_Qacategories_QacategorieId",
                table: "QuesionAnswers",
                column: "QacategorieId",
                principalTable: "Qacategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Customers_CustomerId",
                table: "Users",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}

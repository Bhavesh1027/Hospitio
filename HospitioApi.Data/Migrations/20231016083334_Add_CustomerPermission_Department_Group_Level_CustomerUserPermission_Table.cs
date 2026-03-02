using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_CustomerPermission_Department_Group_Level_CustomerUserPermission_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerDepartmentId",
                table: "CustomerUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerGroupId",
                table: "CustomerUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerLevelId",
                table: "CustomerUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupervisorId",
                table: "CustomerUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DepartmentMangerId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerDepartments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerDepartments_CustomerUsers_DepartmentMangerId",
                        column: x => x.DepartmentMangerId,
                        principalTable: "CustomerUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NormalizedLevelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsCustomerUserType = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsView = table.Column<bool>(type: "bit", nullable: true),
                    IsEdit = table.Column<bool>(type: "bit", nullable: true),
                    IsUpload = table.Column<bool>(type: "bit", nullable: true),
                    IsReply = table.Column<bool>(type: "bit", nullable: true),
                    IsDownload = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    GroupLeaderId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGroups_CustomerDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "CustomerDepartments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGroups_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGroups_CustomerUsers_GroupLeaderId",
                        column: x => x.GroupLeaderId,
                        principalTable: "CustomerUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerUsersPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerPermissionId = table.Column<int>(type: "int", nullable: true),
                    CustomerUserId = table.Column<int>(type: "int", nullable: true),
                    IsView = table.Column<bool>(type: "bit", nullable: true),
                    IsEdit = table.Column<bool>(type: "bit", nullable: true),
                    IsUpload = table.Column<bool>(type: "bit", nullable: true),
                    IsReply = table.Column<bool>(type: "bit", nullable: true),
                    IsDownload = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerUsersPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerUsersPermissions_CustomerPermissions_CustomerPermissionId",
                        column: x => x.CustomerPermissionId,
                        principalTable: "CustomerPermissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerUsersPermissions_CustomerUsers_CustomerUserId",
                        column: x => x.CustomerUserId,
                        principalTable: "CustomerUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerUsers_CustomerDepartmentId",
                table: "CustomerUsers",
                column: "CustomerDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerUsers_CustomerGroupId",
                table: "CustomerUsers",
                column: "CustomerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerUsers_CustomerLevelId",
                table: "CustomerUsers",
                column: "CustomerLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerUsers_SupervisorId",
                table: "CustomerUsers",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDepartments_CustomerId",
                table: "CustomerDepartments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDepartments_DepartmentMangerId",
                table: "CustomerDepartments",
                column: "DepartmentMangerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGroups_CustomerId",
                table: "CustomerGroups",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGroups_DepartmentId",
                table: "CustomerGroups",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGroups_GroupLeaderId",
                table: "CustomerGroups",
                column: "GroupLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLevels_LevelName",
                table: "CustomerLevels",
                column: "LevelName",
                unique: true,
                filter: "[LevelName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerUsersPermissions_CustomerPermissionId",
                table: "CustomerUsersPermissions",
                column: "CustomerPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerUsersPermissions_CustomerUserId",
                table: "CustomerUsersPermissions",
                column: "CustomerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerUsers_CustomerDepartments_CustomerDepartmentId",
                table: "CustomerUsers",
                column: "CustomerDepartmentId",
                principalTable: "CustomerDepartments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerUsers_CustomerGroups_CustomerGroupId",
                table: "CustomerUsers",
                column: "CustomerGroupId",
                principalTable: "CustomerGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerUsers_CustomerLevels_CustomerLevelId",
                table: "CustomerUsers",
                column: "CustomerLevelId",
                principalTable: "CustomerLevels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerUsers_CustomerUsers_SupervisorId",
                table: "CustomerUsers",
                column: "SupervisorId",
                principalTable: "CustomerUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerUsers_CustomerDepartments_CustomerDepartmentId",
                table: "CustomerUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerUsers_CustomerGroups_CustomerGroupId",
                table: "CustomerUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerUsers_CustomerLevels_CustomerLevelId",
                table: "CustomerUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerUsers_CustomerUsers_SupervisorId",
                table: "CustomerUsers");

            migrationBuilder.DropTable(
                name: "CustomerGroups");

            migrationBuilder.DropTable(
                name: "CustomerLevels");

            migrationBuilder.DropTable(
                name: "CustomerUsersPermissions");

            migrationBuilder.DropTable(
                name: "CustomerDepartments");

            migrationBuilder.DropTable(
                name: "CustomerPermissions");

            migrationBuilder.DropIndex(
                name: "IX_CustomerUsers_CustomerDepartmentId",
                table: "CustomerUsers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerUsers_CustomerGroupId",
                table: "CustomerUsers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerUsers_CustomerLevelId",
                table: "CustomerUsers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerUsers_SupervisorId",
                table: "CustomerUsers");

            migrationBuilder.DropColumn(
                name: "CustomerDepartmentId",
                table: "CustomerUsers");

            migrationBuilder.DropColumn(
                name: "CustomerGroupId",
                table: "CustomerUsers");

            migrationBuilder.DropColumn(
                name: "CustomerLevelId",
                table: "CustomerUsers");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "CustomerUsers");
        }
    }
}

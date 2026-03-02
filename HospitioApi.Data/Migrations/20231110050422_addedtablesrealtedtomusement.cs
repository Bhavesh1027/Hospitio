using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class addedtablesrealtedtomusement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MusementGuestInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerGuestId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusementGuestInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MusementGuestInfos_CustomerGuests_CustomerGuestId",
                        column: x => x.CustomerGuestId,
                        principalTable: "CustomerGuests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MusementOrderInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusementGuestInfoId = table.Column<int>(type: "int", nullable: false),
                    OrderUUID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusementOrderInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MusementOrderInfos_MusementGuestInfos_MusementGuestInfoId",
                        column: x => x.MusementGuestInfoId,
                        principalTable: "MusementGuestInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MusementItemInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusementOrderInfoId = table.Column<int>(type: "int", nullable: true),
                    ProductActivityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    ReatailPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusementItemInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MusementItemInfos_MusementOrderInfos_MusementOrderInfoId",
                        column: x => x.MusementOrderInfoId,
                        principalTable: "MusementOrderInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MusementPaymentInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusementOrderInfoId = table.Column<int>(type: "int", nullable: false),
                    PaymentMetod = table.Column<byte>(type: "tinyint", nullable: true),
                    PaymentStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    PaymentIntentID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MusementItemInfoId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusementPaymentInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MusementPaymentInfos_MusementItemInfos_MusementItemInfoId",
                        column: x => x.MusementItemInfoId,
                        principalTable: "MusementItemInfos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MusementPaymentInfos_MusementOrderInfos_MusementOrderInfoId",
                        column: x => x.MusementOrderInfoId,
                        principalTable: "MusementOrderInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MusementGuestInfos_CustomerGuestId",
                table: "MusementGuestInfos",
                column: "CustomerGuestId");

            migrationBuilder.CreateIndex(
                name: "IX_MusementItemInfos_MusementOrderInfoId",
                table: "MusementItemInfos",
                column: "MusementOrderInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_MusementOrderInfos_MusementGuestInfoId",
                table: "MusementOrderInfos",
                column: "MusementGuestInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_MusementPaymentInfos_MusementItemInfoId",
                table: "MusementPaymentInfos",
                column: "MusementItemInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_MusementPaymentInfos_MusementOrderInfoId",
                table: "MusementPaymentInfos",
                column: "MusementOrderInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusementPaymentInfos");

            migrationBuilder.DropTable(
                name: "MusementItemInfos");

            migrationBuilder.DropTable(
                name: "MusementOrderInfos");

            migrationBuilder.DropTable(
                name: "MusementGuestInfos");
        }
    }
}

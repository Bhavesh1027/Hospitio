using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_TaxiTransferGuestRequest_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxiTransferGuestRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    GuestId = table.Column<int>(type: "int", nullable: true),
                    TransferId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransferStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GRPaymentId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GRPaymentDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxiTransferGuestRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxiTransferGuestRequests_CustomerGuests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "CustomerGuests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaxiTransferGuestRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxiTransferGuestRequests_CustomerId",
                table: "TaxiTransferGuestRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxiTransferGuestRequests_GuestId",
                table: "TaxiTransferGuestRequests",
                column: "GuestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxiTransferGuestRequests");
        }
    }
}

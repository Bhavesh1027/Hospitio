using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_RefundId_AND_RefundStatus_Column_In_TaxiTransferGuestRequest_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "TaxiTransferGuestRequests",
                newName: "RefundAmount");

            migrationBuilder.AddColumn<string>(
                name: "RefundId",
                table: "TaxiTransferGuestRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefundStatus",
                table: "TaxiTransferGuestRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefundId",
                table: "TaxiTransferGuestRequests");

            migrationBuilder.DropColumn(
                name: "RefundStatus",
                table: "TaxiTransferGuestRequests");

            migrationBuilder.RenameColumn(
                name: "RefundAmount",
                table: "TaxiTransferGuestRequests",
                newName: "Amount");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_FareAmount_And_HospitioFareAmount_Column_In_TaxiTransferRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FareAmount",
                table: "TaxiTransferGuestRequests",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HospitioFareAmount",
                table: "TaxiTransferGuestRequests",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FareAmount",
                table: "TaxiTransferGuestRequests");

            migrationBuilder.DropColumn(
                name: "HospitioFareAmount",
                table: "TaxiTransferGuestRequests");
        }
    }
}

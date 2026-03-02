using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_PickUpDate_And_IsMonthlyReport_Column_In_TaxiTransferGuestRequest_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMonthlyReport",
                table: "TaxiTransferGuestRequests",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PickUpDate",
                table: "TaxiTransferGuestRequests",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMonthlyReport",
                table: "TaxiTransferGuestRequests");

            migrationBuilder.DropColumn(
                name: "PickUpDate",
                table: "TaxiTransferGuestRequests");
        }
    }
}

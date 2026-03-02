using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class addedtaxitanscommissiontohospitioonboarding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaxiTransCommission",
                table: "HospitioOnboardings",
                type: "int",
                nullable: false,
                defaultValue: 12);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxiTransCommission",
                table: "HospitioOnboardings");
        }
    }
}

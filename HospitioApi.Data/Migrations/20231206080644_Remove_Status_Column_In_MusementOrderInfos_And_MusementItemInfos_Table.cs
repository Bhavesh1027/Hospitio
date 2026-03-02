using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Remove_Status_Column_In_MusementOrderInfos_And_MusementItemInfos_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "MusementOrderInfos");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MusementItemInfos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MusementOrderInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MusementItemInfos",
                type: "int",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_MusementCustomerInfo_And_MusementOrder_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "MusementOrderInfos",
                type: "int",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "MusementOrderInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "MusementOrderInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MusementCustomerId",
                table: "MusementGuestInfos",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "MusementOrderInfos");

            migrationBuilder.DropColumn(
                name: "currency",
                table: "MusementOrderInfos");

            migrationBuilder.DropColumn(
                name: "MusementCustomerId",
                table: "MusementGuestInfos");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "MusementOrderInfos",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

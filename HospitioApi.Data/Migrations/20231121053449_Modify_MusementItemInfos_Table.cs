using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_MusementItemInfos_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReatailPrice",
                table: "MusementItemInfos",
                newName: "TotalPrice");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "MusementItemInfos",
                type: "int",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CartItemUUID",
                table: "MusementItemInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "MusementItemInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemMusementProductId",
                table: "MusementItemInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemUUID",
                table: "MusementItemInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriceFeature",
                table: "MusementItemInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProductDiscountAmount",
                table: "MusementItemInfos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProductOriginalRetailPrice",
                table: "MusementItemInfos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProductServiceFee",
                table: "MusementItemInfos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketHolder",
                table: "MusementItemInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MusementItemInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionCode",
                table: "MusementItemInfos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartItemUUID",
                table: "MusementItemInfos");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "MusementItemInfos");

            migrationBuilder.DropColumn(
                name: "ItemMusementProductId",
                table: "MusementItemInfos");

            migrationBuilder.DropColumn(
                name: "ItemUUID",
                table: "MusementItemInfos");

            migrationBuilder.DropColumn(
                name: "PriceFeature",
                table: "MusementItemInfos");

            migrationBuilder.DropColumn(
                name: "ProductDiscountAmount",
                table: "MusementItemInfos");

            migrationBuilder.DropColumn(
                name: "ProductOriginalRetailPrice",
                table: "MusementItemInfos");

            migrationBuilder.DropColumn(
                name: "ProductServiceFee",
                table: "MusementItemInfos");

            migrationBuilder.DropColumn(
                name: "TicketHolder",
                table: "MusementItemInfos");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "MusementItemInfos");

            migrationBuilder.DropColumn(
                name: "TransactionCode",
                table: "MusementItemInfos");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "MusementItemInfos",
                newName: "ReatailPrice");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "MusementItemInfos",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

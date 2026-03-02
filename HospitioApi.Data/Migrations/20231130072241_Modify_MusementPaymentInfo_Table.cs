using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_MusementPaymentInfo_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusementPaymentInfos_MusementItemInfos_MusementItemInfoId",
                table: "MusementPaymentInfos");

            migrationBuilder.DropIndex(
                name: "IX_MusementPaymentInfos_MusementItemInfoId",
                table: "MusementPaymentInfos");

            migrationBuilder.RenameColumn(
                name: "PaymentIntentID",
                table: "MusementPaymentInfos",
                newName: "PaymentTransactionId");

            migrationBuilder.RenameColumn(
                name: "MusementItemInfoId",
                table: "MusementPaymentInfos",
                newName: "CustomerId");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "MusementPaymentInfos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "MusementPaymentInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerGuestId",
                table: "MusementPaymentInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MusementPaymentInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderUUID",
                table: "MusementPaymentInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentPlatForm",
                table: "MusementPaymentInfos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "MusementPaymentInfos");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "MusementPaymentInfos");

            migrationBuilder.DropColumn(
                name: "CustomerGuestId",
                table: "MusementPaymentInfos");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MusementPaymentInfos");

            migrationBuilder.DropColumn(
                name: "OrderUUID",
                table: "MusementPaymentInfos");

            migrationBuilder.DropColumn(
                name: "PaymentPlatForm",
                table: "MusementPaymentInfos");

            migrationBuilder.RenameColumn(
                name: "PaymentTransactionId",
                table: "MusementPaymentInfos",
                newName: "PaymentIntentID");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "MusementPaymentInfos",
                newName: "MusementItemInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_MusementPaymentInfos_MusementItemInfoId",
                table: "MusementPaymentInfos",
                column: "MusementItemInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_MusementPaymentInfos_MusementItemInfos_MusementItemInfoId",
                table: "MusementPaymentInfos",
                column: "MusementItemInfoId",
                principalTable: "MusementItemInfos",
                principalColumn: "Id");
        }
    }
}

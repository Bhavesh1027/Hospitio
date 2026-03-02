using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_MusementPayment_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusementPaymentInfos_MusementOrderInfos_MusementOrderInfoId",
                table: "MusementPaymentInfos");

            migrationBuilder.DropIndex(
                name: "IX_MusementPaymentInfos_MusementOrderInfoId",
                table: "MusementPaymentInfos");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "MusementOrderInfos");

            migrationBuilder.RenameColumn(
                name: "PaymentPlatForm",
                table: "MusementPaymentInfos",
                newName: "PlatForm");

            migrationBuilder.RenameColumn(
                name: "MusementOrderInfoId",
                table: "MusementPaymentInfos",
                newName: "OrderInfoId");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "MusementOrderInfos",
                newName: "Currency");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlatForm",
                table: "MusementPaymentInfos",
                newName: "PaymentPlatForm");

            migrationBuilder.RenameColumn(
                name: "OrderInfoId",
                table: "MusementPaymentInfos",
                newName: "MusementOrderInfoId");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "MusementOrderInfos",
                newName: "currency");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "MusementOrderInfos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MusementPaymentInfos_MusementOrderInfoId",
                table: "MusementPaymentInfos",
                column: "MusementOrderInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_MusementPaymentInfos_MusementOrderInfos_MusementOrderInfoId",
                table: "MusementPaymentInfos",
                column: "MusementOrderInfoId",
                principalTable: "MusementOrderInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

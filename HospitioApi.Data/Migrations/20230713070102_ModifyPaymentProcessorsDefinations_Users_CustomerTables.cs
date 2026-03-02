using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModifyPaymentProcessorsDefinations_Users_CustomerTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentProcessorsDefinations_PaymentProcessors_PaymentProcessorId",
                table: "PaymentProcessorsDefinations");

            migrationBuilder.DropColumn(
                name: "PaymentProcessorsId",
                table: "PaymentProcessorsDefinations");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeActivated",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentProcessorId",
                table: "PaymentProcessorsDefinations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeActivated",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentProcessorsDefinations_PaymentProcessors_PaymentProcessorId",
                table: "PaymentProcessorsDefinations",
                column: "PaymentProcessorId",
                principalTable: "PaymentProcessors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentProcessorsDefinations_PaymentProcessors_PaymentProcessorId",
                table: "PaymentProcessorsDefinations");

            migrationBuilder.DropColumn(
                name: "DeActivated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeActivated",
                table: "Customers");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentProcessorId",
                table: "PaymentProcessorsDefinations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PaymentProcessorsId",
                table: "PaymentProcessorsDefinations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentProcessorsDefinations_PaymentProcessors_PaymentProcessorId",
                table: "PaymentProcessorsDefinations",
                column: "PaymentProcessorId",
                principalTable: "PaymentProcessors",
                principalColumn: "Id");
        }
    }
}

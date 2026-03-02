using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddMissingPaymentProcessorTableonApplicationDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPaymentProcessor_Customers_CustomerId",
                table: "CustomerPaymentProcessor");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPaymentProcessor_PaymentProcessor_PaymentProcessorId",
                table: "CustomerPaymentProcessor");

            migrationBuilder.DropForeignKey(
                name: "FK_HospitioPaymentProcessor_PaymentProcessor_PaymentProcessorId",
                table: "HospitioPaymentProcessor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HospitioPaymentProcessor",
                table: "HospitioPaymentProcessor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentProcessor",
                table: "PaymentProcessor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerPaymentProcessor",
                table: "CustomerPaymentProcessor");

            migrationBuilder.RenameTable(
                name: "HospitioPaymentProcessor",
                newName: "HospitioPaymentProcessors");

            migrationBuilder.RenameTable(
                name: "PaymentProcessor",
                newName: "PaymentProcessors");

            migrationBuilder.RenameTable(
                name: "CustomerPaymentProcessor",
                newName: "CustomerPaymentProcessors");

            migrationBuilder.RenameIndex(
                name: "IX_HospitioPaymentProcessor_PaymentProcessorId",
                table: "HospitioPaymentProcessors",
                newName: "IX_HospitioPaymentProcessors_PaymentProcessorId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerPaymentProcessor_PaymentProcessorId",
                table: "CustomerPaymentProcessors",
                newName: "IX_CustomerPaymentProcessors_PaymentProcessorId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerPaymentProcessor_CustomerId",
                table: "CustomerPaymentProcessors",
                newName: "IX_CustomerPaymentProcessors_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HospitioPaymentProcessors",
                table: "HospitioPaymentProcessors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentProcessors",
                table: "PaymentProcessors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerPaymentProcessors",
                table: "CustomerPaymentProcessors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPaymentProcessors_Customers_CustomerId",
                table: "CustomerPaymentProcessors",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPaymentProcessors_PaymentProcessors_PaymentProcessorId",
                table: "CustomerPaymentProcessors",
                column: "PaymentProcessorId",
                principalTable: "PaymentProcessors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HospitioPaymentProcessors_PaymentProcessors_PaymentProcessorId",
                table: "HospitioPaymentProcessors",
                column: "PaymentProcessorId",
                principalTable: "PaymentProcessors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPaymentProcessors_Customers_CustomerId",
                table: "CustomerPaymentProcessors");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPaymentProcessors_PaymentProcessors_PaymentProcessorId",
                table: "CustomerPaymentProcessors");

            migrationBuilder.DropForeignKey(
                name: "FK_HospitioPaymentProcessors_PaymentProcessors_PaymentProcessorId",
                table: "HospitioPaymentProcessors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HospitioPaymentProcessors",
                table: "HospitioPaymentProcessors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentProcessors",
                table: "PaymentProcessors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerPaymentProcessors",
                table: "CustomerPaymentProcessors");

            migrationBuilder.RenameTable(
                name: "HospitioPaymentProcessors",
                newName: "HospitioPaymentProcessor");

            migrationBuilder.RenameTable(
                name: "PaymentProcessors",
                newName: "PaymentProcessor");

            migrationBuilder.RenameTable(
                name: "CustomerPaymentProcessors",
                newName: "CustomerPaymentProcessor");

            migrationBuilder.RenameIndex(
                name: "IX_HospitioPaymentProcessors_PaymentProcessorId",
                table: "HospitioPaymentProcessor",
                newName: "IX_HospitioPaymentProcessor_PaymentProcessorId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerPaymentProcessors_PaymentProcessorId",
                table: "CustomerPaymentProcessor",
                newName: "IX_CustomerPaymentProcessor_PaymentProcessorId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerPaymentProcessors_CustomerId",
                table: "CustomerPaymentProcessor",
                newName: "IX_CustomerPaymentProcessor_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HospitioPaymentProcessor",
                table: "HospitioPaymentProcessor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentProcessor",
                table: "PaymentProcessor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerPaymentProcessor",
                table: "CustomerPaymentProcessor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPaymentProcessor_Customers_CustomerId",
                table: "CustomerPaymentProcessor",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPaymentProcessor_PaymentProcessor_PaymentProcessorId",
                table: "CustomerPaymentProcessor",
                column: "PaymentProcessorId",
                principalTable: "PaymentProcessor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HospitioPaymentProcessor_PaymentProcessor_PaymentProcessorId",
                table: "HospitioPaymentProcessor",
                column: "PaymentProcessorId",
                principalTable: "PaymentProcessor",
                principalColumn: "Id");
        }
    }
}

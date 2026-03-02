using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ReArangeSomeTablesForPaymentFlow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "HospitioPaymentProcessors");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PaymentProcessors");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "CustomerPaymentProcessors");

            migrationBuilder.RenameColumn(
                name: "ClientSecret",
                table: "HospitioPaymentProcessors",
                newName: "GRWebhookURL");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "HospitioPaymentProcessors",
                newName: "GRPaymentServiceId");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "GuestRequests",
                newName: "GRPaymentId");

            migrationBuilder.RenameColumn(
                name: "PaymentDetails",
                table: "GuestRequests",
                newName: "GRPaymentDetails");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "EnhanceStayItemsGuestRequests",
                newName: "GRPaymentId");

            migrationBuilder.RenameColumn(
                name: "PaymentDetails",
                table: "EnhanceStayItemsGuestRequests",
                newName: "GRPaymentDetails");

            migrationBuilder.RenameColumn(
                name: "ClientSecret",
                table: "CustomerPaymentProcessors",
                newName: "GRWebhookURL");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "CustomerPaymentProcessors",
                newName: "GRPaymentServiceId");

            migrationBuilder.AddColumn<bool>(
                name: "GR3DSecureEnabled",
                table: "HospitioPaymentProcessors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GRAcceptedCountries",
                table: "HospitioPaymentProcessors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GRAcceptedCurrencies",
                table: "HospitioPaymentProcessors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GRFields",
                table: "HospitioPaymentProcessors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GRIsDeleted",
                table: "HospitioPaymentProcessors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GRMerchantProfile",
                table: "HospitioPaymentProcessors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GRCategory",
                table: "PaymentProcessors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GRGroup",
                table: "PaymentProcessors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GRID",
                table: "PaymentProcessors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GRIcon",
                table: "PaymentProcessors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GRName",
                table: "PaymentProcessors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GR3DSecureEnabled",
                table: "CustomerPaymentProcessors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GRAcceptedCountries",
                table: "CustomerPaymentProcessors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GRAcceptedCurrencies",
                table: "CustomerPaymentProcessors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GRFields",
                table: "CustomerPaymentProcessors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GRIsDeleted",
                table: "CustomerPaymentProcessors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GRMerchantProfile",
                table: "CustomerPaymentProcessors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerPaymentProcessorCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    MerchantId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SecretKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPaymentProcessorCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPaymentProcessorCredentials_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentProcessorsDefinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentProcessorsId = table.Column<int>(type: "int", nullable: false),
                    GRFields = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GRSupportedCountries = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GRSupportedCurrencies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GRSupportedFeatures = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentProcessorId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentProcessorsDefinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentProcessorsDefinations_PaymentProcessors_PaymentProcessorId",
                        column: x => x.PaymentProcessorId,
                        principalTable: "PaymentProcessors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HospitioPaymentProcessorCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SecretKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitioPaymentProcessorCredentials", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPaymentProcessorCredentials_CustomerId",
                table: "CustomerPaymentProcessorCredentials",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentProcessorsDefinations_PaymentProcessorId",
                table: "PaymentProcessorsDefinations",
                column: "PaymentProcessorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPaymentProcessorCredentials");

            migrationBuilder.DropTable(
                name: "PaymentProcessorsDefinations");

            migrationBuilder.DropTable(
                name: "HospitioPaymentProcessorCredentials");

            migrationBuilder.DropColumn(
                name: "GR3DSecureEnabled",
                table: "HospitioPaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRAcceptedCountries",
                table: "HospitioPaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRAcceptedCurrencies",
                table: "HospitioPaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRFields",
                table: "HospitioPaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRIsDeleted",
                table: "HospitioPaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRMerchantProfile",
                table: "HospitioPaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRCategory",
                table: "PaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRGroup",
                table: "PaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRID",
                table: "PaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRIcon",
                table: "PaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRName",
                table: "PaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GR3DSecureEnabled",
                table: "CustomerPaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRAcceptedCountries",
                table: "CustomerPaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRAcceptedCurrencies",
                table: "CustomerPaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRFields",
                table: "CustomerPaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRIsDeleted",
                table: "CustomerPaymentProcessors");

            migrationBuilder.DropColumn(
                name: "GRMerchantProfile",
                table: "CustomerPaymentProcessors");

            migrationBuilder.RenameColumn(
                name: "GRWebhookURL",
                table: "HospitioPaymentProcessors",
                newName: "ClientSecret");

            migrationBuilder.RenameColumn(
                name: "GRPaymentServiceId",
                table: "HospitioPaymentProcessors",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "GRPaymentId",
                table: "GuestRequests",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "GRPaymentDetails",
                table: "GuestRequests",
                newName: "PaymentDetails");

            migrationBuilder.RenameColumn(
                name: "GRPaymentId",
                table: "EnhanceStayItemsGuestRequests",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "GRPaymentDetails",
                table: "EnhanceStayItemsGuestRequests",
                newName: "PaymentDetails");

            migrationBuilder.RenameColumn(
                name: "GRWebhookURL",
                table: "CustomerPaymentProcessors",
                newName: "ClientSecret");

            migrationBuilder.RenameColumn(
                name: "GRPaymentServiceId",
                table: "CustomerPaymentProcessors",
                newName: "ClientId");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "HospitioPaymentProcessors",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PaymentProcessors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "CustomerPaymentProcessors",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddTable_PaymentProcessors_Hospitio_Customers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentProcessor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentProcessor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPaymentProcessor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    PaymentProcessorId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPaymentProcessor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPaymentProcessor_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerPaymentProcessor_PaymentProcessor_PaymentProcessorId",
                        column: x => x.PaymentProcessorId,
                        principalTable: "PaymentProcessor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HospitioPaymentProcessor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentProcessorId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitioPaymentProcessor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HospitioPaymentProcessor_PaymentProcessor_PaymentProcessorId",
                        column: x => x.PaymentProcessorId,
                        principalTable: "PaymentProcessor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPaymentProcessor_CustomerId",
                table: "CustomerPaymentProcessor",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPaymentProcessor_PaymentProcessorId",
                table: "CustomerPaymentProcessor",
                column: "PaymentProcessorId");

            migrationBuilder.CreateIndex(
                name: "IX_HospitioPaymentProcessor_PaymentProcessorId",
                table: "HospitioPaymentProcessor",
                column: "PaymentProcessorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPaymentProcessor");

            migrationBuilder.DropTable(
                name: "HospitioPaymentProcessor");

            migrationBuilder.DropTable(
                name: "PaymentProcessor");
        }
    }
}

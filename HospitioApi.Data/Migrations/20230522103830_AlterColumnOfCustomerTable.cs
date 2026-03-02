using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AlterColumnOfCustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServicePackageId",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
               name: "ProductId",
               table: "Customers",
               type: "int",
               nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ProductId",
                table: "Customers",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Products_ProductId",
                table: "Customers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Products_ProductId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_ProductId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Customers",
                newName: "ServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ServicePackageId",
                table: "Customers",
                column: "ServicePackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Products_ServicePackageId",
                table: "Customers",
                column: "ServicePackageId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}

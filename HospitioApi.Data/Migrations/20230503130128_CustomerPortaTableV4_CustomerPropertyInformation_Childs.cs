using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CustomerPortaTableV4_CustomerPropertyInformation_Childs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WifiUsername",
                table: "CustomerPropertyInformations",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WifiPassword",
                table: "CustomerPropertyInformations",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StreetNumber",
                table: "CustomerPropertyInformations",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "CustomerPropertyInformations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Postalcode",
                table: "CustomerPropertyInformations",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "CustomerPropertyInformations",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "CustomerPropertyInformations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerPropertyEmergencyNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerPropertyInformationId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneCountry = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPropertyEmergencyNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPropertyEmergencyNumbers_CustomerPropertyInformations_CustomerPropertyInformationId",
                        column: x => x.CustomerPropertyInformationId,
                        principalTable: "CustomerPropertyInformations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerPropertyExtras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerPropertyInformationId = table.Column<int>(type: "int", nullable: true),
                    ExtraType = table.Column<byte>(type: "tinyint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Link = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPropertyExtras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPropertyExtras_CustomerPropertyInformations_CustomerPropertyInformationId",
                        column: x => x.CustomerPropertyInformationId,
                        principalTable: "CustomerPropertyInformations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerPropertyServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerPropertyInformationId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPropertyServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPropertyServices_CustomerPropertyInformations_CustomerPropertyInformationId",
                        column: x => x.CustomerPropertyInformationId,
                        principalTable: "CustomerPropertyInformations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerPropertyServiceImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerPropertyServiceId = table.Column<int>(type: "int", nullable: true),
                    ServiceImages = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPropertyServiceImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPropertyServiceImages_CustomerPropertyServices_CustomerPropertyServiceId",
                        column: x => x.CustomerPropertyServiceId,
                        principalTable: "CustomerPropertyServices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPropertyEmergencyNumbers_CustomerPropertyInformationId",
                table: "CustomerPropertyEmergencyNumbers",
                column: "CustomerPropertyInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPropertyExtras_CustomerPropertyInformationId",
                table: "CustomerPropertyExtras",
                column: "CustomerPropertyInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPropertyServiceImages_CustomerPropertyServiceId",
                table: "CustomerPropertyServiceImages",
                column: "CustomerPropertyServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPropertyServices_CustomerPropertyInformationId",
                table: "CustomerPropertyServices",
                column: "CustomerPropertyInformationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPropertyEmergencyNumbers");

            migrationBuilder.DropTable(
                name: "CustomerPropertyExtras");

            migrationBuilder.DropTable(
                name: "CustomerPropertyServiceImages");

            migrationBuilder.DropTable(
                name: "CustomerPropertyServices");

            migrationBuilder.AlterColumn<string>(
                name: "WifiUsername",
                table: "CustomerPropertyInformations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WifiPassword",
                table: "CustomerPropertyInformations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StreetNumber",
                table: "CustomerPropertyInformations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "CustomerPropertyInformations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Postalcode",
                table: "CustomerPropertyInformations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "CustomerPropertyInformations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "CustomerPropertyInformations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}

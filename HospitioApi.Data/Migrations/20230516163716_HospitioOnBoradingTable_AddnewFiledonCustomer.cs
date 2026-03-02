using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class HospitioOnBoradingTable_AddnewFiledonCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SmsTitle",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HospitioOnboardings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhatsappCountry = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    WhatsappNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ViberCountry = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    ViberNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TelegramCounty = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    TelegramNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneCountry = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SmsTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Messenger = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClientDoamin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IncomingTranslationLangage = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NoTranslateWords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitioOnboardings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HospitioOnboardings");

            migrationBuilder.DropColumn(
                name: "SmsTitle",
                table: "Customers");
        }
    }
}

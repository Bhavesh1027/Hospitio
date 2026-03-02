using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CreateVonageCredetialsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VonageCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    SubAccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    APIKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    APISecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppPrivatKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppPublicKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VonageCredentials", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VonageCredentials");
        }
    }
}

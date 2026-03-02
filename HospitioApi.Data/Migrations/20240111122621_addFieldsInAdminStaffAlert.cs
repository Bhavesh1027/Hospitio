using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class addFieldsInAdminStaffAlert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VonageTemplateId",
                table: "AdminStaffAlerts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VonageTemplateStatus",
                table: "AdminStaffAlerts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsappTemplateName",
                table: "AdminStaffAlerts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VonageTemplateId",
                table: "AdminStaffAlerts");

            migrationBuilder.DropColumn(
                name: "VonageTemplateStatus",
                table: "AdminStaffAlerts");

            migrationBuilder.DropColumn(
                name: "WhatsappTemplateName",
                table: "AdminStaffAlerts");
        }
    }
}

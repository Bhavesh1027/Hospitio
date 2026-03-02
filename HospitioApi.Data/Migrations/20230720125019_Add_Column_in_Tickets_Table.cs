using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_Column_in_Tickets_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_GroupId",
                table: "Tickets",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Groups_GroupId",
                table: "Tickets",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Groups_GroupId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_GroupId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Tickets");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Update_Modules_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Modules SET Name = 'Communication' WHERE  Id = 2");
            migrationBuilder.Sql("UPDATE Modules SET Name = 'GuestJourney' WHERE  Id = 3");
            migrationBuilder.Sql("UPDATE Modules SET Name = 'OnlineCheckin' WHERE  Id = 4");
            migrationBuilder.Sql("UPDATE Modules SET Name = 'GuestPortal' WHERE  Id = 5");
            migrationBuilder.Sql("UPDATE Modules SET Name = 'eKeys' WHERE  Id = 6");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

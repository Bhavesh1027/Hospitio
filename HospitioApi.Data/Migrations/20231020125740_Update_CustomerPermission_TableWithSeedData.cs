using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Update_CustomerPermission_TableWithSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Upadet CustomerPermission Table

            migrationBuilder.Sql("UPDATE CustomerPermissions SET IsUpload = 0 WHERE  Id = 1");
            migrationBuilder.Sql("UPDATE CustomerPermissions SET IsView = 1 ,IsUpload = 1 , IsDownload = 0 WHERE  Id = 2");
            migrationBuilder.Sql("UPDATE CustomerPermissions SET IsUpload = 0 , IsDownload = 0 WHERE  Id = 3");
            migrationBuilder.Sql("UPDATE CustomerPermissions SET IsUpload = 1 , IsDownload = 1 WHERE  Id = 5");
            migrationBuilder.Sql("UPDATE CustomerPermissions SET IsEdit = 0 , IsUpload = 0 , IsDownload=0 WHERE  Id = 6");
            migrationBuilder.Sql("UPDATE CustomerPermissions SET IsUpload = 0 , IsDownload=0 WHERE  Id = 7");
            migrationBuilder.Sql("UPDATE CustomerPermissions SET IsDownload=0 WHERE  Id = 8");
            migrationBuilder.Sql("UPDATE CustomerPermissions SET IsDownload=0 WHERE  Id = 9");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

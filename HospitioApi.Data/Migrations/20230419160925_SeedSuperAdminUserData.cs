using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SeedSuperAdminUserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Seed Super Admin for Initial Login for Applicatin Setup
            migrationBuilder.Sql("INSERT INTO [dbo].[Users]([FirstName],[LastName],[Email],[Title],[UserName],[Password],[UserLevelId],[IsActive],[CreatedAt]) VALUES('Hospitio','Super Admin','hospitio@admin.com','Mr.','hospitio@admin.com','axzulFp0t63X4BgLN9OCEg==',1,1,GETUTCDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SeedApplicationUserTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Adding Default UserTypes
            migrationBuilder.Sql("INSERT INTO [dbo].[UserLevels]([LevelName],[NormalizedLevelName],[IsHospitioUserType],[IsActive],[CreatedAt]) VALUES('Super Admin','SuperAdmin',1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[UserLevels]([LevelName],[NormalizedLevelName],[IsHospitioUserType],[IsActive],[CreatedAt]) VALUES('CEO','CEO',1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[UserLevels]([LevelName],[NormalizedLevelName],[IsHospitioUserType],[IsActive],[CreatedAt]) VALUES('Manager','Manager',1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[UserLevels]([LevelName],[NormalizedLevelName],[IsHospitioUserType],[IsActive],[CreatedAt]) VALUES('Group Leader','GroupLeader',1,1,GETUTCDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[UserLevels]([LevelName],[NormalizedLevelName],[IsHospitioUserType],[IsActive],[CreatedAt]) VALUES('Staff','Staff',1,1,GETUTCDATE())");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

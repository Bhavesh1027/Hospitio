using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetUsers_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
           /****** Object:  StoredProcedure [dbo].[GetUsers]    Script Date: 10-05-2023 18:59:22 ******/
           SET ANSI_NULLS ON
           GO
           SET QUOTED_IDENTIFIER ON
           GO
           Create or ALTER Procedure [dbo].[GetUsers]
           as
            SELECT 
            ( 
            SELECT Id, FirstName,LastName,Email,Title,ProfilePicture,PhoneCountry,PhoneNumber,
                
                 (SELECT *, (select Name from Permissions where Id=up.PermissionId) as PermissionName
                    FROM UsersPermissions up 
                    WHERE  up.UserId = us.Id
                    FOR JSON PATH) AS UserModulePermissions
            FROM Users us where us.DeletedAt is null
            ORDER BY us.Id          
            FOR JSON PATH ) as UserOut
                      ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetUsers_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetUsers]
(
@DepartmentId INT = 0,
@GroupId INT = 0,
@UserLevelId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [Id],
                   [FirstName],
                   [LastName],
                   [Email],
                   [Title],
                   [ProfilePicture],
                   [PhoneCountry],
                   [PhoneNumber],
                   (
                       SELECT [Id],[PermissionId],[UserId],[IsView],[IsEdit],[IsUpload],[IsReply],[IsSend],
                              (
                                  SELECT [Name]
                                  FROM [dbo].[Permissions] (NOLOCK)
                                  WHERE [DeletedAt] IS NULL 
									AND [Id] = [up].[PermissionId]
                              ) AS [PermissionName]
                       FROM [dbo].[UsersPermissions] up (NOLOCK)
                       WHERE [up].[UserId] = [us].[Id]
                       FOR JSON PATH
                   ) AS [UserModulePermissions]
            FROM [dbo].[Users] us
            WHERE [us].[DeletedAt] IS NULL
			AND ([us].DepartmentId = ISNULL(@DepartmentId,0) OR (ISNULL(@DepartmentId,0) = 0))
			AND ([us].GroupId = ISNULL(@GroupId,0) OR (ISNULL(@GroupId,0) = 0))
			AND ([us].UserLevelId < ISNULL(@UserLevelId,0) OR (ISNULL(@UserLevelId,0) = 0))
            ORDER BY [us].[Id]
            FOR JSON PATH
        ) as UserOut
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

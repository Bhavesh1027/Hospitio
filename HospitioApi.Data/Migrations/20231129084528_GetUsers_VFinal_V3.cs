using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetUsers_VFinal_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetUsers]    Script Date: 29-11-2023 12:12:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetUsers]
(
@DepartmentId INT = 0,
@GroupId INT = 0,
@UserLevelId INT = 0,
@UserId INT = 0
)
AS

BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON
	Declare @query NVARCHAR(MAX) = ''

	If(@UserLevelId = 3 OR @UserLevelId=2)
	Begin
		Set @query += 'SELECT
				(SELECT
						[Id],
						[FirstName],
						[LastName],
						[Email],
						[Title],
						[ProfilePicture],
						[PhoneCountry],
						[PhoneNumber],(
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
				AND [us].[UserLevelId] = 2
				AND [us].[IsActive] = 1
				AND [us].[Id] <>'+ Cast(@UserId as NVARCHAR(50)) +'
				ORDER BY [us].[Id] OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY
			FOR JSON PATH) as UserOut'
	End
	Else If(@UserLevelId = 4)
    Begin
        Set @query += 'SELECT
				(SELECT
					   [Id],
					   [FirstName],
					   [LastName],
					   [Email],
					   [Title],
					   [ProfilePicture],
					   [PhoneCountry],
					   [PhoneNumber],(
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
				AND [us].[Id] <>'+ Cast(@UserId as NVARCHAR(50)) +'
				AND [us].[DepartmentId] = ' + Cast(ISNULL(@DepartmentId, 0) as NVARCHAR(50)) +
                   ' AND [us].[IsActive] = 1 AND [us].[UserLevelId] = 3
				ORDER BY [us].[Id] OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY
            FOR JSON PATH) as UserOut'
    End
	Else If(@UserLevelId = 5)
    Begin
        Set @query += 'SELECT
				(SELECT
					   [Id],
					   [FirstName],
					   [LastName],
					   [Email],
					   [Title],
					   [ProfilePicture],
					   [PhoneCountry],
					   [PhoneNumber],(
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
				AND [us].[Id] <>'+ Cast(@UserId as NVARCHAR(50)) +'
				AND (([us].[DepartmentId] = ' + Cast(ISNULL(@DepartmentId, 0) as NVARCHAR(50)) +
                   ' AND [us].[UserLevelId] = 3)
				OR ([us].[GroupId] = ' + Cast(ISNULL(@GroupId, 0) as NVARCHAR(50)) +
                   ' AND [us].[UserLevelId] = 4))
				AND [us].[IsActive] = 1
				ORDER BY [us].[Id] FOR JSON PATH) as UserOut'
    End
	Else
	BEGIN


		Set @query += 'SELECT
			(SELECT [Id],
						   [FirstName],
						   [LastName],
						   [Email],
						   [Title],
						   [ProfilePicture],
						   [PhoneCountry],
						   [PhoneNumber],
						   [IsActive],(
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
					AND ([us].DepartmentId = '+ Cast(ISNULL(@DepartmentId,0) as NVARCHAR(50)) +' OR ('+ Cast(ISNULL(@DepartmentId,0) as NVARCHAR(50)) +' = 0))
					AND ([us].GroupId = '+ Cast(ISNULL(@GroupId,0) as NVARCHAR(50)) +' OR ('+ Cast(ISNULL(@GroupId,0) as NVARCHAR(50))+' = 0))
					AND ([us].UserLevelId < '+ Cast(ISNULL(@UserLevelId,0) as NVARCHAR(50)) +' OR ('+ Cast(ISNULL(@UserLevelId,0) as NVARCHAR(50)) +' = 0))' 


			If(ISNULL(@UserLevelId,0) = 4 OR ISNULL(@UserLevelId,0) = 5)
				Set @query += 'AND [us].UserLevelId <> 2'

			If(IsNULL(@UserLevelId,0) <> 0)
				Set @query += 'AND [us].UserLevelId <> 1' 

		Set @query += 'ORDER BY [us].[Id]
				FOR JSON PATH
			) as UserOut'


	END

		EXEC sp_executesql @query
		Print @query
END


");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

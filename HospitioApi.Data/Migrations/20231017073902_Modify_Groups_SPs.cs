using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_Groups_SPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetGroups
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGroups]    Script Date: 17/10/2023 1:09:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetGroups]
(
   @UserType INT = 0,
   @UserId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

	IF @UserType = 1
	BEGIN
	    SELECT [Id],
               [Name],
               [DepartmentId],
               [IsActive],
               [GroupLeaderId]
        FROM [dbo].[Groups]
        WHERE [DeletedAt] IS NULL
	END

	ELSE IF @UserType = 2
	BEGIN
	    SELECT [Id],
               [Name],
               [DepartmentId],
               [IsActive],
               [GroupLeaderId]
        FROM [dbo].[CustomerGroups]
        WHERE [DeletedAt] IS NULL
		      AND [CustomerId] = @UserId
	END

END");
            #endregion

            #region GetGroupById
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGroupById]    Script Date: 17/10/2023 1:43:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetGroupById]
(
	@Id INT = 0,
	@UserType INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

	IF @UserType = 1
	BEGIN
	    SELECT [Id],
               [Name],
               [DepartmentId],
               [IsActive],
               [GroupLeaderId]
        FROM [dbo].[Groups] (NOLOCK)
        WHERE [DeletedAt] IS NULL
              AND [Id] = @Id
	END

	ELSE IF @UserType = 2
	BEGIN
	    SELECT [Id],
               [Name],
               [DepartmentId],
               [IsActive],
               [GroupLeaderId]
        FROM [dbo].[CustomerGroups] (NOLOCK)
        WHERE [DeletedAt] IS NULL
              AND [Id] = @Id
	END

END");
            #endregion

            #region GetGroupsByDepartmentId
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGroupsByDepartmentId]    Script Date: 17/10/2023 1:44:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetGroupsByDepartmentId]
(
	@DepartmentId INT = 0,
	@UserType INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

	IF @UserType = 1
	BEGIN
	    SELECT [dbo].[Groups].[Id],
               [dbo].[Groups].[Name]
        FROM [dbo].[Groups] (NOLOCK)
        WHERE [dbo].[Groups].[DeletedAt] IS NULL
              AND [dbo].[Groups].[DepartmentId] = @DepartmentId 
              AND [dbo].[Groups].[IsActive] = 1
	END

	ELSE IF @UserType = 2
	BEGIN
	    SELECT [dbo].[CustomerGroups].[Id],
               [dbo].[CustomerGroups].[Name]
        FROM [dbo].[CustomerGroups] (NOLOCK)
        WHERE [dbo].[CustomerGroups].[DeletedAt] IS NULL
              AND [dbo].[CustomerGroups].[DepartmentId] = @DepartmentId 
              AND [dbo].[CustomerGroups].[IsActive] = 1
	END

END 
");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_GetCustomerDepartmentById_And_Modify_GetDepartments_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerDepartmentById
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerDepartmentById]    Script Date: 16/10/2023 7:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetCustomerDepartmentById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [CD].[Id],
           [CD].[Name],
           [CD].[DepartmentMangerId]
    FROM [dbo].[CustomerDepartments] CD (NOLOCK)
    WHERE [CD].[DeletedAt] IS NULL
          AND [CD].[IsActive] = 1
          AND [CD].[Id] = @Id
END");
            #endregion

            #region GetDepartments
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetDepartments]    Script Date: 16/10/2023 7:00:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetDepartments] -- 2 ,20
(
   @UserType int =0,
   @UserId int = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

	IF @UserType = 1
	BEGIN
		SELECT [Id],
			   [Name],
			   [DepartmentMangerId]
		FROM [dbo].[Departments] (NOLOCK)
		WHERE [DeletedAt] IS NULL
			  AND [IsActive] = 1
    END

	ELSE IF @UserType = 2
	BEGIN

	    SELECT [Id],
			   [Name],
			   [DepartmentMangerId]
		FROM [dbo].[CustomerDepartments] (NOLOCK)
		WHERE [DeletedAt] IS NULL
			  AND [IsActive] = 1
			  AND [CustomerId] = @UserId
	END

END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

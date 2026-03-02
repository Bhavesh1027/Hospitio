using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_GetCustomerUsers_Sp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerUsers]    Script Date: 18/10/2023 1:55:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerUsers]
(
	@DepartmentId INT = 0,
	@GroupId INT = 0,
	@CustomerUserLevelId INT = 0,
	@CustomerUserId INT = 0,
	@CustomerId INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON	
	Declare @query NVARCHAR(MAX) = ''

	If(@CustomerUserLevelId = 3 OR @CustomerUserLevelId = 2)
	BEGIN
		SET @query += 'SELECT
				        (SELECT [Id],
								[FirstName],
								[LastName],
								[Email],
								[Title],
								[ProfilePicture],
								[PhoneCountry],
						        [PhoneNumber],
								(
						           SELECT [Id],
										  [CustomerPermissionId],
										  [CustomerUserId],
										  [IsView],
										  [IsEdit],
										  [IsUpload],
										  [IsReply],
										  [IsDownload]
						            FROM [dbo].[CustomerUsersPermissions] CUP (NOLOCK)
						                  WHERE [CUP].[CustomerUserId] = [CU].[Id]
						                  FOR JSON PATH
					             ) AS [CustomerUserModulePermissions]
				        FROM [dbo].[CustomerUsers] CU
				              WHERE [CU].[DeletedAt] IS NULL
				                    AND [CU].[CustomerLevelId] = 2
				                    AND [CU].[IsActive] = 1
									AND [CU].[CustomerId] ='+ Cast(@CustomerId as NVARCHAR(50))  +'
				                    AND [CU].[Id] <>'+ Cast(@CustomerUserId as NVARCHAR(50)) +'
				              ORDER BY [CU].[Id] OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY
			            FOR JSON PATH) AS [CustomerUserOut]'
	END
	ElSE IF(@CustomerUserLevelId = 4)
    BEGIN
        SET @query += 'SELECT
				        ( SELECT [Id],
					             [FirstName],
							     [LastName],
							     [Email],
							     [Title],
							     [ProfilePicture],
							     [PhoneCountry],
							     [PhoneNumber],
								 (
						           SELECT [Id],
										  [CustomerPermissionId],
										  [CustomerUserId],
										  [IsView],
										  [IsEdit],
										  [IsUpload],
										  [IsReply],
										  [IsDownload]
						            FROM [dbo].[CustomerUsersPermissions] CUP (NOLOCK)
						                  WHERE [CUP].[CustomerUserId] = [CU].[Id]
						                  FOR JSON PATH
					             ) AS [CustomerUserModulePermissions]
				       FROM [dbo].[CustomerUsers] CU
				             WHERE [CU].[DeletedAt] IS NULL
				                   AND [CU].[Id] <>'+ Cast(@CustomerUserId as NVARCHAR(50)) +'
				                   AND [CU].[CustomerDepartmentId] = ' + Cast(ISNULL(@DepartmentId, 0) as NVARCHAR(50)) + ' 
							       AND [CU].[IsActive] = 1 
							       AND [CU].[CustomerLevelId] = 3
								   AND [CU].[CustomerId] ='+ Cast(@CustomerId as NVARCHAR(50))  +'
				             ORDER BY [CU].[Id] OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY
                       FOR JSON PATH) AS [CustomerUserOut]'
    END
	ElSE IF(@CustomerUserLevelId = 5)
    BEGIN
        Set @query += 'SELECT
				       ( SELECT [Id],
					            [FirstName],
							    [LastName],
							    [Email],
							    [Title],
							    [ProfilePicture],
							    [PhoneCountry],
							    [PhoneNumber],
								 (
						           SELECT [Id],
										  [CustomerPermissionId],
										  [CustomerUserId],
										  [IsView],
										  [IsEdit],
										  [IsUpload],
										  [IsReply],
										  [IsDownload]
						            FROM [dbo].[CustomerUsersPermissions] CUP (NOLOCK)
						                  WHERE [CUP].[CustomerUserId] = [CU].[Id]
						                  FOR JSON PATH
				 	             ) AS [CustomerUserModulePermissions]
				      FROM [dbo].[CustomerUsers] CU
				           WHERE [CU].[DeletedAt] IS NULL
						         AND [CU].[CustomerId] ='+ Cast(@CustomerId as NVARCHAR(50))  +'
				                 AND [CU].[Id] <>'+ Cast(@CustomerUserId as NVARCHAR(50)) +'
				                 AND (    ( [CU].[CustomerDepartmentId] = ' + Cast(ISNULL(@DepartmentId, 0) as NVARCHAR(50)) +' 
								             AND [CU].[CustomerLevelId] = 3)
				                       OR ( [CU].[CustomerGroupId] = ' + Cast(ISNULL(@GroupId, 0) as NVARCHAR(50)) +' 
									        AND [CU].[CustomerLevelId] = 4)
									  )
				                AND [CU].[IsActive] = 1
				     ORDER BY [CU].[Id] FOR JSON PATH) as [CustomerUserOut]'
    END

	ELSE
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
								 (
						           SELECT [Id],
										  [CustomerPermissionId],
										  [CustomerUserId],
										  [IsView],
										  [IsEdit],
										  [IsUpload],
										  [IsReply],
										  [IsDownload]
						            FROM [dbo].[CustomerUsersPermissions] CUP (NOLOCK)
						                  WHERE [CUP].[CustomerUserId] = [CU].[Id]
						                  FOR JSON PATH
				 	             ) AS [CustomerUserModulePermissions]
					   FROM [dbo].[CustomerUsers] CU
					        WHERE [CU].[DeletedAt] IS NULL
							      AND [CU].[CustomerId] ='+ Cast(@CustomerId as NVARCHAR(50))  +'
					              AND ( [CU].CustomerDepartmentId = '+ Cast(ISNULL(@DepartmentId,0) as NVARCHAR(50)) +'
								         OR ('+ Cast(ISNULL(@DepartmentId,0) as NVARCHAR(50)) +' = 0))
					              AND ( [CU].CustomerGroupId = '+ Cast(ISNULL(@GroupId,0) as NVARCHAR(50)) +' 
								         OR ('+ Cast(ISNULL(@GroupId,0) as NVARCHAR(50))+' = 0))
					              AND ( [CU].CustomerLevelId < '+ Cast(ISNULL(@CustomerUserLevelId,0) as NVARCHAR(50)) +' 
								         OR ('+ Cast(ISNULL(@CustomerUserLevelId,0) as NVARCHAR(50)) +' = 0))' 

				If(ISNULL(@CustomerUserLevelId,0) = 4 OR ISNULL(@CustomerUserLevelId,0) = 5)
					Set @query += 'AND [CU].CustomerLevelId <> 2'

				If(IsNULL(@CustomerUserLevelId,0) <> 0)
					Set @query += 'AND [CU].CustomerLevelId <> 1' 

				Set @query += 'ORDER BY [CU].[Id]
						FOR JSON PATH
					) as [CustomerUserOut]'
	END

	    EXEC sp_executesql @query
		Print @query
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

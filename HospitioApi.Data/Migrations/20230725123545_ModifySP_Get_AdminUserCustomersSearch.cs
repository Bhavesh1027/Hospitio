using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModifySP_Get_AdminUserCustomersSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[SP_Get_AdminUserCustomersSearch] 
(
	@SearchString NVARCHAR(50) = NULL,
	@PageNo INT = 1,
    @PageSize INT = 10,
	@UserId INT = 0
)
AS
BEGIN
	
	SET NOCOUNT ON
    SET XACT_ABORT ON

	SET @SearchString = LTRIM(RTRIM(@SearchString));

	IF OBJECT_ID('tempdb..#TempTables') IS NOT NULL
        DROP TABLE #TempTables;

    CREATE TABLE #TempTables
    (
        [CustomerId] INT,
		[BusinessName] NVARCHAR(100),
        [FirstName] NVARCHAR(50),
        [LastName] NVARCHAR(50),
        [Email] NVARCHAR(100),
        [Title] NVARCHAR(5),
        [ProfilePicture] NVARCHAR(500),
        [PhoneCountry] NVARCHAR(3),
        [PhoneNumber] NVARCHAR(20),
        [UserName] NVARCHAR(100),
        [UserType] NVARCHAR(255)
   )

	INSERT INTO #TempTables ([CustomerId],[BusinessName],[FirstName],[LastName],[Email],[Title],[ProfilePicture],[PhoneCountry],[PhoneNumber],[UserName],[UserType])
	SELECT	[C].[Id],[C].[BusinessName],NULL AS [FirstName],NULL AS [LastName],[C].[Email],NULL AS [Title],NULL AS [ProfilePicture],[C].[PhoneCountry],[C].[PhoneNumber],NULL AS [UserName],'CustomerUser'
	FROM [dbo].[Customers] C (NOLOCK) 
	WHERE [C].[DeletedAt] IS NULL 
	UNION ALL
	SELECT	[U].[Id],NULL AS [BusinessName],[U].[FirstName],[U].[LastName],[U].[Email],[U].[Title],[U].[ProfilePicture],[U].[PhoneCountry],[U].[PhoneNumber],[U].[UserName],'HospitioUser'
	FROM [dbo].[Users] U (NOLOCK)
	WHERE [U].[DeletedAt] IS NULL AND [U].[Id] != @UserId

	SELECT (
            SELECT	[CustomerId] AS [Id],
					[BusinessName],
					[FirstName],
					[LastName],
					[Email],
					[Title],
					[ProfilePicture],
					[PhoneCountry],
					[PhoneNumber],
					[UserName],
					[UserType],
                   JSON_QUERY(
                   (
                       SELECT	[CU].[Id],NULL AS [BusinessName],[CU].[FirstName],[CU].[LastName],[CU].[Email],[CU].[ProfilePicture],[CU].[PhoneCountry],[CU].[PhoneNumber]
                       FROM [dbo].[CustomerUsers] CU (NOLOCK)
                       WHERE [CU].[DeletedAt] IS NULL 
						AND [CU].[CustomerId] = #TempTables.[CustomerId]
                       FOR JSON PATH
                   )
                 ) AS [UserOuts]
            FROM #TempTables
			WHERE 
			(
				[BusinessName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[FirstName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[LastName] LIKE '%' + ISNULL(@SearchString,'') + '%'
			)
			ORDER BY [CustomerId] ASC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
            FOR JSON PATH
        );
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetChatList_modification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetChatList]
(
    @UserId INT = 1,
	@UserType INT = 1,
    @PageNo INT = 1,
    @PageSize INT = 10,
	@IsDeleted BIT= 0
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

	IF OBJECT_ID('tempdb..#TempTables') IS NOT NULL
        DROP TABLE #TempTables;

    CREATE TABLE #TempTables
    (
        [ChatId] INT,
		[LastMessage] NVARCHAR(100),
        [LastMessageTime] DATETIME,
        [FirstName] NVARCHAR(50),
        [LastName] NVARCHAR(100),
        [ProfilePicture] NVARCHAR(500),
        [IsActive] BIT,
		[UnReadCount] INT,
		[UserType] NVARCHAR(20)
   )

	INSERT INTO #TempTables ([ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[ProfilePicture],[IsActive],[UnReadCount],[UserType])
	SELECT [CU].[ChannelId] AS [ChatId],
    [CM].[Message] AS [LastMessage],
    [CM].[CreatedAt] AS [LastMessageTime],
    CASE
        WHEN @UserType = 1 THEN [U].[FirstName]
        WHEN @UserType = 2 THEN [CS].[FirstName]
    END AS [FirstName],
    CASE
        WHEN @UserType = 1 THEN [U].[LastName]
        WHEN @UserType = 2 THEN [CS].[LastName]
    END AS [LastName],
    CASE
        WHEN @UserType = 1 THEN [U].[ProfilePicture]
        WHEN @UserType = 2 THEN [CS].[ProfilePicture]
    END AS [ProfilePicture],
    C.IsActive,
	 SUM(CASE WHEN CM.IsRead = 0 THEN 1 ELSE 0 END) AS UnReadCount,
	CASE 
		WHEN @UserType = 1 THEN 'HOSPITIOUSER'
		WHEN @UserType = 2 THEN 'CUSTOMER'
		END AS [UserType]
	FROM [dbo].[ChannelUsers] CU
		INNER JOIN [dbo].[Channels] C ON C.[Id] = CU.[ChannelId]
		LEFT JOIN [dbo].[ChannelMessages] CM ON CM.[ChannelId] = C.[Id]
		LEFT JOIN [dbo].[Users] U ON U.[Id] = CU.[UserId] AND @UserType = 1
		LEFT JOIN [dbo].[Customers] CT ON CT.[Id] = CU.[UserId] AND @UserType = 2
		LEFT JOIN [dbo].[CustomerUsers] CS ON CS.[CustomerId] = CT.[Id] AND @UserType = 2
	WHERE CU.[UserId] <> @UserId --AND CU.[DeletedAt] IS NULL
		AND ISNULL(CM.DeletedAt,0) = ISNULL(@IsDeleted,0)
	GROUP BY [CU].[ChannelId], [CM].[Message], [CM].[CreatedAt], [U].[FirstName], [U].[LastName], [U].[ProfilePicture], [CS].[FirstName], [CS].[LastName], [CS].[ProfilePicture], C.IsActive
	ORDER BY [CM].[CreatedAt] DESC

	SELECT (
            SELECT [ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[ProfilePicture],[IsActive],[UnReadCount],
			(SELECT SUM(CAST([IsActive] AS int)) FROM #TempTables) AS [TotalActiveCount],
			(SELECT SUM(CAST([UnReadCount] AS int)) FROM #TempTables) AS [TotalUnReadCount],[UserType]
			FROM #TempTables AS t
			ORDER BY t.[ChatId] ASC
			OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
            FOR JSON PATH
        );
	
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

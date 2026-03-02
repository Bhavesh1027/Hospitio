using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetChatList_modify_GetBusinessName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER    PROCEDURE [dbo].[SP_GetChatList]
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
		[BusinessName] NVARCHAr(100),
        [ProfilePicture] NVARCHAR(500),
        [IsActive] BIT,
		[UnReadCount] INT,
		[UserType] NVARCHAR(20)
   )

	INSERT INTO #TempTables ([ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[UnReadCount],[UserType])
	SELECT CT.ChannelId AS [ChatId],CM.[Message] AS LastMessage, CT.LastMessageReadTime AS LastMessageTime ,[FirstName],[LastName],'' AS [BusinessName],[ProfilePicture],
	CT.IsActive,CM.IsRead,'HOSPITIOUSER' AS [UserType]
	from [dbo].[Users] (NOLOCK) US
	INNER JOIN ChannelUsers CT ON CT.UserId = US.Id
	INNER JOIN ChannelMessages CM ON CM.Id = CT.LastMessageReadId
	WHERE ISNULL(CT.ChannelId,'') != ''
	AND US.Id <> @UserId
	UNION
	Select [CT].[ChannelId] AS [ChatId],[CM].[Message] AS LastMessage,CT.LastMessageReadTime AS LastMessageTime,'' AS [FirstName],'' AS [LastName],[C].[BusinessName],
	CU.[ProfilePicture],CT.IsActive,CM.IsRead,'CUSTOMER' AS UserType
	from [dbo].[Customers] C 
	LEFT JOIN CustomerUsers CU ON CU.CustomerId = C.Id
	LEFT JOIN ChannelUsers CT ON CT.UserId = C.Id
	LEFT JOIN ChannelMessages CM ON CM.Id = CT.LastMessageReadId
	WHERE ISNULL(CT.ChannelId,'') != ''
	AND CU.Id <> @UserId

	SELECT (
            SELECT [ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[UnReadCount],
			(SELECT SUM(CAST([IsActive] AS INT)) FROM #TempTables) AS [TotalActiveCount],
			(SELECT SUM(CAST([UnReadCount] AS INT)) FROM #TempTables) AS [TotalUnReadCount],[UserType]
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

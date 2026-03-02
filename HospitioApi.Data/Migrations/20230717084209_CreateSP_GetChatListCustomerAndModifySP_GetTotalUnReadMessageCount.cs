using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CreateSP_GetChatListCustomerAndModifySP_GetTotalUnReadMessageCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // create SP_GetChatListCustomer
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetChatListCustomer]
(
    @UserId INT = 1,
    @PageNo INT = 1,
    @PageSize INT = 10,
	@ChatType VARCHAR(20) = '',
    @Filter VARCHAR(20) = ''
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

	IF OBJECT_ID('tempdb..#TempTables') IS NOT NULL
        DROP TABLE #TempTables;

    CREATE TABLE #TempTables
    (
		[UserId] INT,
        [ChatId] INT,
		[LastMessage] NVARCHAR(100),
        [LastMessageTime] DATETIME,
        [FirstName] NVARCHAR(50),
        [LastName] NVARCHAR(100),
        [ProfilePicture] NVARCHAR(500),
        [IsActive] BIT,
		[TotalUnReadCount] INT,
		[UserType] NVARCHAR(20),
		[Status] NVARCHAR(20),
   )

	INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType],[Status])
	SELECT [US].[Id],[CT].[ChannelId] AS [ChatId],CM.[Message] AS [LastMessage],[CT].[LastMessageReadTime] AS [LastMessageTime] ,[FirstName],[LastName],[ProfilePicture],
			[CT].[IsActive],
			(SELECT COUNT([C].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
			INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
			INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
			) AS [TotalUnReadCount],'HospitioUser' AS [UserType],'' AS [Status]
		FROM [dbo].[Users] (NOLOCK) US
		LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [US].[Id]
		LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CT].[ChannelId] AND CT.UserId = CM.MessageSenderId
	    WHERE ISNULL([CT].[ChannelId],'') != ''
		AND [US].[Id] <> @UserId 
		AND [CT].[UserType] = 'HospitioUser'
		AND [CM].[DeletedAt] IS NULL
		AND CT.ChannelId IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
		AND [CM].[Id] = (SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) INNER JOIN [dbo].[ChannelUsers] CU (NOLOCK) ON [CU].[ChannelId] = [CMS].[ChannelId] AND [CU].[UserId] = [CMS].[MessageSenderId] AND CU.UserId <> @UserId ORDER BY CMS.Id DESC)
	UNION
	SELECT [CG].[Id],[CT].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CT].[LastMessageReadTime] AS [LastMessageTime],[CG].[Firstname] AS [FirstName],[CG].[Lastname] AS [LastName],
				[CG].[Picture],[CT].[IsActive],
				(SELECT COUNT([C].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
					INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
					INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
				) AS [TotalUnReadCount],'CustomerGuest' AS [UserType],
			CASE WHEN (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
            AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE())) THEN 'In-House'
			WHEN (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
            AND CONVERT(DATE, CR.CheckoutDate) < CONVERT(DATE, GETDATE())) THEN 'Checked-out'
			END AS [Status]
		FROM [dbo].[CustomerReservations] CR
			LEFT JOIN [dbo].[CustomerGuests] CG  ON CG.CustomerReservationId = CR.Id 
			LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [CG].[Id]
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CT].[ChannelId] AND CT.UserId = CM.MessageSenderId
		WHERE ISNULL([CT].[ChannelId],'') != '' AND CR.CustomerId = @UserId
			AND [CT].[Id] <> @UserId
			AND [CT].[UserType] = 'CustomerGuest'
			AND [CM].[DeletedAt] IS NULL
			AND CT.ChannelId IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
			AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) INNER JOIN [dbo].[ChannelUsers] CU (NOLOCK) ON [CU].[ChannelId] = [CMS].[ChannelId] AND [CU].[UserId] = [CMS].[MessageSenderId] AND CU.UserId <> @UserId ORDER BY CMS.Id DESC),'')
	AND (@ChatType = ''
		OR (
            @ChatType = 'inbox'
			AND (
				SELECT COUNT(ChannelId)
				FROM ChannelMessages
				WHERE ChannelId = CT.ChannelId
				GROUP BY ChannelId
			) > 1 -- Users who have received a message and replied to it
        )
        OR (
            @ChatType = 'in-bound'
			--AND CM.IsRead = 0
			AND (
				SELECT COUNT(ChannelId)
				FROM ChannelMessages
				WHERE ChannelId = CT.ChannelId
				GROUP BY ChannelId
			) = 1
			AND EXISTS (
				SELECT 1
				FROM ChannelMessages
				WHERE ChannelId = CT.ChannelId
				AND MessageSenderId <> CT.UserId
			)
			AND CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
        )
        OR (
            @ChatType = 'contacted'
			--AND CM.IsRead = 0
			AND (
				SELECT COUNT(ChannelId)
				FROM ChannelMessages
				WHERE ChannelId = CT.ChannelId
				GROUP BY ChannelId
			) = 1
			AND EXISTS (
				SELECT 1
				FROM ChannelMessages
				WHERE ChannelId = CT.ChannelId
				AND MessageSenderId <> CT.UserId
			)
			AND (
				CONVERT(DATE, CR.CheckinDate) > CONVERT(DATE, GETDATE())
			)
        )
    )
	AND (
        @Filter = ''
        OR (
            @Filter = 'in-house'
            AND CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
            AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE())
        )
        OR (
            @Filter = 'checkedout'
            AND CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
            AND CONVERT(DATE, CR.CheckoutDate) < CONVERT(DATE, GETDATE())
        )
    )

	SELECT
        (
            SELECT SUM(CAST([IsActive] AS INT)) AS ActiveUsers,
                   (
                       SELECT [UserId],[ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType],[Status]
						FROM #TempTables AS t
						ORDER BY t.[ChatId] ASC
						OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
                       FOR JSON PATH
                   ) AS [ChatList]
            FROM #TempTables AS ActiveUsers
            FOR JSON PATH 
        ) AS [ChatListResponse]
	
END
");

            // Modify SP_GetTotalUnReadMessageCount
            migrationBuilder.Sql(@"CREATE OR ALTER  PROCEDURE [dbo].[SP_GetTotalUnReadMessageCount]
(
	 @UserId INT = 0
)
AS
BEGIN
	
	SET NOCOUNT ON
    SET XACT_ABORT ON
	
	SELECT COUNT(DISTINCT([C].[Id])) AS [TotalUnreadCount]
	FROM [dbo].[Channels] C (NOLOCK)
		INNER JOIN [dbo].[ChannelUsers] CU 
			ON [C].[Id] = [CU].[ChannelId] 
				AND [CU].[UserId] = @UserId
		INNER JOIN [dbo].[ChannelMessages] CM  
			ON  [CM].[ChannelId] = [C].[Id] 
				AND [CM].[MessageSenderId] <> @UserId 
				AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

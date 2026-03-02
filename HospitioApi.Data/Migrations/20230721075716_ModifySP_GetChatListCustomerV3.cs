using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModifySP_GetChatListCustomerV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER       PROCEDURE [dbo].[SP_GetChatListCustomer]
(
    @UserId INT = 0,
	@UserType VARCHAR(20) = '',
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

   DECLARE @ChatUserType INT = 0

   IF(@UserType='HospitioUser')
   BEGIN
	SET @ChatUserType = 1
   END
   ELSE IF(@UserType='CustomerUser')
   BEGIN
	SET @ChatUserType = 2
   END
   ELSE IF(@UserType='CustomerGuest')
   BEGIN
	SET @ChatUserType = 3
   END

	INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType],[Status])
	SELECT [US].[Id],CU1.[ChannelId] AS [ChatId],CM.[Message] AS [LastMessage],CU1.[LastMessageReadTime] AS [LastMessageTime] ,[FirstName],[LastName],[ProfilePicture],
			CU1.[IsActive],
			(SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] FROM [dbo].[ChannelMessages] CM 
				WHERE  [CM].[ChannelId] = [CU1].[ChannelId]
				AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
				AND (
					(
							CM.MessageSender = @ChatUserType
							AND CM.MessageSenderId <> @UserId
					)
					OR (
							CM.MessageSender <> @ChatUserType
					)
				)
			) AS [TotalUnReadCount],'HospitioUser' AS [UserType],'' AS [Status]
	FROM [dbo].[ChannelUsers] CU1
	INNER JOIN [dbo].[ChannelUsers] CU2 ON CU1.channelId = CU2.channelId
	INNER JOIN [dbo].[Users] US ON CU1.UserId = US.Id
	LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = cu1.[ChannelId] 
	WHERE cu2.UserId = @UserId  
		AND cu2.UserType = @UserType
		AND
		(
			(
					cu1.UserType = @UserType
					AND cu1.UserId <> @UserId
			)
			OR (
					cu1.UserType <> @UserType
			)
		)
		AND cu1.UserType = 'HospitioUser'
		AND ISNULL(CU1.[ChannelId],'') != ''
		--AND [US].[UserLevelId] = 1
		--AND [CU1].[Id] <> @UserId 
		--AND [CU1].[UserType] = 'HospitioUser'
		AND [CM].[DeletedAt] IS NULL
		AND CU1.ChannelId IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
		AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) WHERE [CU1].[ChannelId] = [CMS].[ChannelId] ORDER BY CMS.Id DESC),'')
		AND @ChatType = 'inbox'
	UNION
	SELECT [CG].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CU1].[LastMessageReadTime] AS [LastMessageTime],[CG].[Firstname] AS [FirstName],[CG].[Lastname] AS [LastName],
				[CG].[Picture],[CU1].[IsActive],
				(SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] FROM [dbo].[ChannelMessages] CM 
				WHERE  [CM].[ChannelId] = [CU1].[ChannelId]
				AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
				AND (
					(
							CM.MessageSender = @ChatUserType
							AND CM.MessageSenderId <> @UserId
					)
					OR (
							CM.MessageSender <> @ChatUserType
					)
				)
			) AS [TotalUnReadCount],'CustomerGuest' AS [UserType],
			CASE WHEN (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
            AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE())) THEN 'In-House'
			WHEN (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
            AND CONVERT(DATE, CR.CheckoutDate) < CONVERT(DATE, GETDATE())) THEN 'Checked-out'
			END AS [Status]
		FROM [dbo].[ChannelUsers] CU1
		INNER JOIN [dbo].[ChannelUsers] CU2 ON CU1.channelId = CU2.channelId
		INNER JOIN [dbo].[CustomerGuests] CG ON CU1.UserId = CG.Id
		INNER JOIN [dbo].[CustomerReservations] CR ON CG.CustomerReservationId = CR.Id 
		LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] 
		WHERE cu2.UserId = @UserId  
			AND cu2.UserType = @UserType
			AND
			(
				(
						cu1.UserType = @UserType
						AND cu1.UserId <> @UserId
				)
				OR (
						cu1.UserType <> @UserType
				)
			)
			AND cu1.UserType = 'CustomerGuest'
			AND ISNULL(CU1.[ChannelId],'') != ''
			AND CR.CustomerId = @UserId
			AND [CM].[DeletedAt] IS NULL
			AND CU1.ChannelId IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
		AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) WHERE [CU1].[ChannelId] = [CMS].[ChannelId] ORDER BY CMS.Id DESC),'')
	AND (@ChatType = ''
		OR (
            @ChatType = 'inbox'
			AND (
				SELECT COUNT(ChannelId)
				FROM ChannelMessages
				WHERE ChannelId = CU1.ChannelId
				GROUP BY ChannelId
			) > 1 -- Users who have received a message and replied to it
        )
        OR (
            @ChatType = 'in-bound'
			AND (
				SELECT COUNT(ChannelId)
				FROM ChannelMessages
				WHERE ChannelId = CU1.ChannelId
				GROUP BY ChannelId
			) = 1
			AND EXISTS (
				SELECT 1
				FROM ChannelMessages
				WHERE ChannelId = CU1.ChannelId
				--AND MessageSenderId <> CU1.UserId
				AND (
					(
							MessageSender = @ChatUserType
							AND MessageSenderId <> CU1.UserId
					)
					OR (
							MessageSender <> @ChatUserType
					)
				)
			)
			AND CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
        )
        OR (
            @ChatType = 'contacted'
			--AND CM.IsRead = 0
			AND (
				SELECT COUNT(ChannelId)
				FROM ChannelMessages
				WHERE ChannelId = CU1.ChannelId
				GROUP BY ChannelId
			) = 1
			AND EXISTS (
				SELECT 1
				FROM ChannelMessages
				WHERE ChannelId = CU1.ChannelId
				--AND MessageSenderId <> CU1.UserId
				AND (
					(
							MessageSender = @ChatUserType
							AND MessageSenderId <> CU1.UserId
					)
					OR (
							MessageSender <> @ChatUserType
					)
				)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

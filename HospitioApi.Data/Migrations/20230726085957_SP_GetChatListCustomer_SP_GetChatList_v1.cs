using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetChatListCustomer_SP_GetChatList_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[SP_GetChatList]
(
    @UserId INT = 0,
    @PageNo INT = 1,
    @PageSize INT = 10,
	@IsDeleted BIT= 0,
	@UserType NVARCHAR(20) = ''
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
		[BusinessName] NVARCHAR(100),
        [ProfilePicture] NVARCHAR(500),
        [IsActive] BIT,
		[TotalUnReadCount] INT,
		[UserType] NVARCHAR(20)
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

	IF(@IsDeleted = 0)
	BEGIN

		INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType])
		SELECT	[US].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CU1].[LastMessageReadTime] AS [LastMessageTime] ,[FirstName],[LastName],NULL AS [BusinessName],
				[ProfilePicture],[CU1].[IsActive],
				(SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] FROM [dbo].[ChannelMessages] CM (NOLOCK) WHERE [CM].[ChannelId] = [CU1].[ChannelId] --AND [CM].[MessageSenderId] <> @UserId 
				AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
				AND (
						(
							[CM].[MessageSender] = @ChatUserType AND [CM].[MessageSenderId] <> @UserId
						)
					OR (
							[CM].[MessageSender] <> @ChatUserType
						)
					)
			) AS [TotalUnReadCount],'HospitioUser' AS [UserType]
		FROM [dbo].[ChannelUsers] CU1 (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU2 (NOLOCK) ON [CU1].[ChannelId] = [CU2].[ChannelId] 
			INNER JOIN [dbo].[Users] US (NOLOCK) ON [CU1].[UserId] = [US].[Id] AND [US].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[ChannelMessages] CM (NOLOCK) ON [CM].[ChannelId] = [CU1].[ChannelId] AND [CM].[DeletedAt] IS NULL
		WHERE [CU2].[UserId] = @UserId  
			AND [CU2].[UserType] = @UserType
			AND ISNULL(CU1.[ChannelId],'') != ''
			AND [CM].[DeletedAt] IS NULL
			AND [CU1].[UserType] = 'HospitioUser'
			AND [CU1].[ChannelId] IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
			AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) [CMS].[Id] FROM [dbo].[ChannelMessages] CMS (NOLOCK) WHERE [CMS].[ChannelId] = [CU1].[ChannelId] ORDER BY [CMS].[Id] DESC),'')
			AND	(
					(
						[CU1].[UserType] = @UserType AND [CU1].[UserId] <> @UserId
					)
				OR (
						[CU1].[UserType] <> @UserType
					)
				)

	UNION

		SELECT	[C].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CU1].[LastMessageReadTime] AS [LastMessageTime],'' AS [FirstName],'' AS [LastName],[C].[BusinessName],
				[US].[ProfilePicture],[CU1].[IsActive],
				(
					SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] FROM [dbo].[ChannelMessages] CM (NOLOCK) WHERE  [CM].[ChannelId] = [CU1].[ChannelId]	--AND [CM].[MessageSenderId] <> @UserId 
						AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
						AND (
								(
									CM.MessageSender = @ChatUserType AND CM.MessageSenderId <> @UserId
								)
							OR (
									CM.MessageSender <> @ChatUserType
								)
							)
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType]
		FROM [dbo].[ChannelUsers] CU1
			INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[ChannelId] = [CU2].[ChannelId]
			INNER JOIN [dbo].[CustomerUsers] US ON [CU1].[UserId] = [US].[Id] AND [US].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] AND [CM].[DeletedAt] IS NULL
			INNER JOIN [dbo].[Customers] C ON [C].[Id] = [US].[CustomerId] AND [C].[DeletedAt] IS NULL
		WHERE CU2.UserId = @UserId  
		AND CU2.UserType = @UserType
		AND ISNULL(CU1.[ChannelId],'') != ''
		AND [CM].[DeletedAt] IS NULL
		AND [CU1].[UserType] = 'CustomerUser'
		AND [CU1].[ChannelId] IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
		AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) [CMS].[Id] FROM [dbo].[ChannelMessages] CMS (NOLOCK) WHERE [CMS].[ChannelId] = [CU1].[ChannelId] ORDER BY [CMS].[Id] DESC),'')
		AND
		(
			(
				[CU1].[UserType] = @UserType AND [CU1].[UserId] <> @UserId
			)
		OR (
				[CU1].[UserType] <> @UserType
			)
		)
	END
	ELSE IF(@IsDeleted = 1)
	BEGIN
		INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType])
		SELECT	[US].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CU1].[LastMessageReadTime] AS [LastMessageTime] ,[FirstName],[LastName],NULL AS [BusinessName],
				[ProfilePicture],[CU1].[IsActive],
				(
					SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] FROM [dbo].[ChannelMessages] CM WHERE  [CM].[ChannelId] = [CU1].[ChannelId]	--AND [CM].[MessageSenderId] <> @UserId 
					AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
					AND (
							(
								[CM].[MessageSender] = @ChatUserType AND [CM].[MessageSenderId] <> @UserId
							)
						OR (
								[CM].[MessageSender] <> @ChatUserType
							)
						)
				) AS [TotalUnReadCount],'HospitioUser' AS [UserType]
	FROM [dbo].[ChannelUsers] CU1 (NOLOCK)
		INNER JOIN [dbo].[ChannelUsers] CU2 (NOLOCK) ON [CU1].[ChannelId] = [CU2].[ChannelId]
		INNER JOIN [dbo].[Users] US (NOLOCK) ON [CU1].[UserId] = [US].[Id]
		LEFT JOIN [dbo].[ChannelMessages] CM (NOLOCK) ON [CM].[ChannelId] = [CU1].[ChannelId]
	WHERE [CU2].[UserId] = @UserId  
		AND [CU2].[UserType] = @UserType
		AND ISNULL([CU1].[ChannelId],'') != ''
		AND [CM].[DeletedAt] IS NOT NULL
		AND [CU1].[UserType] = 'HospitioUser'
		AND [CU1].[ChannelId] IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
		AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) [CMS].[Id] FROM [dbo].[ChannelMessages] CMS (NOLOCK) WHERE [CMS].[ChannelId] = [CU1].[ChannelId] ORDER BY [CMS].[Id] DESC),'')
		AND
		(
			(
				[CU1].[UserType] = @UserType AND [CU1].[UserId] <> @UserId
			)
		OR (
				[CU1].[UserType] <> @UserType
			)
		)

		UNION

		SELECT	[C].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CU1].[LastMessageReadTime] AS [LastMessageTime],'' AS [FirstName],'' AS [LastName],[C].[BusinessName],
				[US].[ProfilePicture],[CU1].[IsActive],
				(
					SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] FROM [dbo].[ChannelMessages] CM (NOLOCK)WHERE  [CM].[ChannelId] = [CU1].[ChannelId]	--AND [CM].[MessageSenderId] <> @UserId 
					AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
					AND (
							(
								[CM].[MessageSender] = @ChatUserType AND [CM].[MessageSenderId] <> @UserId
							)
						OR (
								[CM].[MessageSender] <> @ChatUserType
							)
						)
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType]
		FROM [dbo].[ChannelUsers] CU1 (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU2 (NOLOCK) ON [CU1].[ChannelId] = [CU2].[ChannelId]
			INNER JOIN [dbo].[CustomerUsers] US (NOLOCK) ON [CU1].[UserId] = [US].[Id]
			LEFT JOIN [dbo].[ChannelMessages] CM (NOLOCK) ON [CM].[ChannelId] = [CU1].[ChannelId] 
			INNER JOIN Customers C (NOLOCK) ON [C].[Id] = [US].[CustomerId]
		WHERE [CU2].[UserId] = @UserId  
		AND [CU2].[UserType] = @UserType
		AND ISNULL([CU1].[ChannelId],'') != ''
		AND [CM].[DeletedAt] IS NOT NULL
		AND [CU1].[UserType] = 'CustomerUser'
		AND [CU1].[ChannelId] IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId)
		AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) [CMS].[Id] FROM [dbo].[ChannelMessages] CMS (NOLOCK) WHERE [CMS].[ChannelId] = [CU1].[ChannelId] ORDER BY [CMS].[Id] DESC),'')
		AND
		(
			(
				[CU1].[UserType] = @UserType AND [CU1].[UserId] <> @UserId
			)
		OR (
				[CU1].[UserType] <> @UserType
			)
		)

	END

	  SELECT
        (
            SELECT SUM(CAST([IsActive] AS INT)) AS ActiveUsers,
                   (
                       SELECT DISTINCT [UserId],[ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType]
						FROM #TempTables AS t
						ORDER BY t.[LastMessageTime] DESC
						OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
                       FOR JSON PATH
                   ) AS [ChatList]
            FROM #TempTables AS ActiveUsers
            FOR JSON PATH 
        ) AS [ChatListResponse]
END
");

			migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[SP_GetChatListCustomer]
(
    @UserId INT = 0,
	@UserType VARCHAR(20) = '',
    @PageNo INT = 1,
    @PageSize INT = 10,
	@ChatType VARCHAR(20) = '',
    @Filter VARCHAR(MAX)= ''
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

	;WITH FilterOptionsCTE AS (
        SELECT value AS FilterOption
        FROM STRING_SPLIT(@Filter, ',')
    )

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
				SELECT COUNT(CM.Id)
					FROM ChannelMessages CM
					WHERE ChannelId = CU1.ChannelId
					AND MessageSender = 3
					AND MessageSenderId = CG.Id
				) > 0 -- Users who have received a message and replied to it
        )
        OR (
            @ChatType = 'in-bound'
			AND (
				(SELECT COUNT(CM.Id)
					FROM ChannelMessages CM
					WHERE ChannelId = CU1.ChannelId
					AND MessageSender = 3
					AND MessageSenderId = CG.Id
				) = 0
				AND (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
				AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE()))
			)
        )
        OR (
            @ChatType = 'contacted'
			AND (
				(
				SELECT COUNT(CM.Id)
				FROM ChannelMessages CM
				WHERE ChannelId = CU1.ChannelId
				AND MessageSender = 3
				AND MessageSenderId = CG.Id
				) = 0
				AND
				(
					(CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
					AND CONVERT(DATE, CR.CheckoutDate) < CONVERT(DATE, GETDATE()))
				)
			)
        )
    )
	--AND (
 --       @Filter = ''
 --       OR (
 --           @Filter = 'in-house'
 --           AND CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
 --           AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE())
 --       )
 --       OR (
 --           @Filter = 'checkedout'
 --           AND CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
 --           AND CONVERT(DATE, CR.CheckoutDate) < CONVERT(DATE, GETDATE())
 --       )
	--	OR (
 --           @Filter = 'expected'
 --           AND CONVERT(DATE, CR.CheckinDate) > CONVERT(DATE, GETDATE())
 --       )
 --   )
	 AND (  EXISTS (
            SELECT 1
            FROM FilterOptionsCTE AS f
            WHERE (
                f.FilterOption IN ('cancelled', 'no-show','')
            )
			OR (
                f.FilterOption = 'expected' AND CONVERT(DATE, CR.CheckinDate) > CONVERT(DATE, GETDATE())
            )
			OR (
                f.FilterOption = 'in-house' AND CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE())
            )
            OR (
                f.FilterOption = 'checkedout' AND CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, CR.CheckoutDate) < CONVERT(DATE, GETDATE())
            )
			OR(
				f.FilterOption = 'cancelled'
			)
			OR(
				f.FilterOption = 'no-show'
			)
        )
    )
	

	SELECT
        (
            SELECT SUM(CAST([IsActive] AS INT)) AS ActiveUsers,
                   (
                       SELECT [UserId],[ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType],[Status]
						FROM #TempTables AS t
						ORDER BY t.[LastMessageTime] DESC
						OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
                       FOR JSON PATH
                   ) AS [ChatList]
            FROM #TempTables AS ActiveUsers
            FOR JSON PATH 
        ) AS [ChatListResponse]
	
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

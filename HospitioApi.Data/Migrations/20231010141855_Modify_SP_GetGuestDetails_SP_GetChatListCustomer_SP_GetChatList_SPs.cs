using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_SP_GetGuestDetails_SP_GetChatListCustomer_SP_GetChatList_SPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region SP_GetChatList
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatList]    Script Date: 10/10/2023 7:50:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_GetChatList]
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
		[LastMessage] NVARCHAR(Max),
		[LastMessageType] NVARCHAR(50),
        [LastMessageTime] DATETIME,
        [FirstName] NVARCHAR(50),
        [LastName] NVARCHAR(100),
		[BusinessName] NVARCHAR(100),
        [ProfilePicture] NVARCHAR(500),
        [IsActive] BIT,
		[TotalUnReadCount] INT,
		[UserType] NVARCHAR(20),
		[PhoneNumber] NVARCHAR(20)
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
   ELSE IF(@UserType= 'AnonymousUser')
   BEGIN
     SET @ChatUserType = 4
   END

	IF(@IsDeleted = 0)
	BEGIN

		INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageType],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType] ,[PhoneNumber])
		SELECT	[US].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CM].[MessageType] AS [LastMessageType],[CM].[CreatedAt] AS [LastMessageTime] ,[FirstName],[LastName],NULL AS [BusinessName],
				[US].[ProfilePicture],[CU1].[IsActive],
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
			) AS [TotalUnReadCount],'HospitioUser' AS [UserType],
			[US].[PhoneNumber] AS [PhoneNumber]
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

		SELECT	[US].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CM].[MessageType] AS [LastMessageType],[CM].[CreatedAt] AS [LastMessageTime],'' AS [FirstName],'' AS [LastName],[C].[BusinessName],
				[CGC].[Logo] AS [ProfilePicture],[CU1].[IsActive],
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
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType],
				[US].[PhoneNumber] AS [PhoneNumber]
		FROM [dbo].[ChannelUsers] CU1
			INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[ChannelId] = [CU2].[ChannelId]
			INNER JOIN [dbo].[CustomerUsers] US ON [CU1].[UserId] = [US].[Id] AND [US].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] AND [CM].[DeletedAt] IS NULL
			INNER JOIN [dbo].[Customers] C ON [C].[Id] = [US].[CustomerId] AND [C].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] CGC ON [CGC].[CustomerId] = [C].[Id] AND [CGC].[DeletedAt] IS NULL
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

    UNION
	    SELECT	[AU].[Id],
		        [CU1].[ChannelId] AS [ChatId],
				[CM].[Message] AS [LastMessage],
				[CM].[MessageType] AS [LastMessageType],
				[CM].[CreatedAt] AS [LastMessageTime] ,
				'' AS [FirstName],
				'' AS [LastName],
				NULL AS [BusinessName],
				NULL AS [ProfilePicture],
				[CU1].[IsActive],
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
			) AS [TotalUnReadCount],'AnonymousUser' AS [UserType],
			[AU].[PhoneNumber] AS [PhoneNumber]
		FROM [dbo].[ChannelUsers] CU1 (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU2 (NOLOCK) ON [CU1].[ChannelId] = [CU2].[ChannelId] 
			INNER JOIN [dbo].[AnonymousUsers] AU ON [AU].[Id] = [CU1].[UserId] AND [AU].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[ChannelMessages] CM (NOLOCK) ON [CM].[ChannelId] = [CU1].[ChannelId] AND [CM].[DeletedAt] IS NULL
		WHERE [CU2].[UserId] = @UserId  
			AND [CU2].[UserType] = @UserType
			AND ISNULL(CU1.[ChannelId],'') != ''
			AND [CM].[DeletedAt] IS NULL
            AND [AU].[UserType] = 2
			AND [CU1].[UserType] = 'AnonymousUser'
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
	END
	ELSE IF(@IsDeleted = 1)
	BEGIN
		INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageType],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType], [PhoneNumber])
		SELECT	[US].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CM].[MessageType] AS [LastMessageType],[CM].[CreatedAt] AS [LastMessageTime] ,[FirstName],[LastName],NULL AS [BusinessName],
				[US].[ProfilePicture],[CU1].[IsActive],
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
				) AS [TotalUnReadCount],'HospitioUser' AS [UserType],
				[US].[PhoneNumber] AS [PhoneNumber]
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

		SELECT	[US].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CM].[MessageType] AS [LastMessageType],[CM].[CreatedAt] AS [LastMessageTime],'' AS [FirstName],'' AS [LastName],[C].[BusinessName],
				[CGC].[Logo] AS [ProfilePicture],[CU1].[IsActive],
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
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType],
				[US].[PhoneNumber] AS [PhoneNumber]
		FROM [dbo].[ChannelUsers] CU1 (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU2 (NOLOCK) ON [CU1].[ChannelId] = [CU2].[ChannelId]
			INNER JOIN [dbo].[CustomerUsers] US (NOLOCK) ON [CU1].[UserId] = [US].[Id]
			LEFT JOIN [dbo].[ChannelMessages] CM (NOLOCK) ON [CM].[ChannelId] = [CU1].[ChannelId] 
			INNER JOIN Customers C (NOLOCK) ON [C].[Id] = [US].[CustomerId]
			LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] CGC ON [CGC].[CustomerId] = [C].[Id]
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

     UNION
	  SELECT	[AU].[Id],
	            [CU1].[ChannelId] AS [ChatId],
				[CM].[Message] AS [LastMessage],
				[CM].[MessageType] AS [LastMessageType],
				[CM].[CreatedAt] AS [LastMessageTime] ,
				'' AS [FirstName],
				'' AS [LastName],
				NULL AS [BusinessName],
				NULL AS [ProfilePicture],
				[CU1].[IsActive],
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
				) AS [TotalUnReadCount],'AnonymousUser' AS [UserType],
				[AU].[PhoneNumber] AS [PhoneNumber]
	FROM [dbo].[ChannelUsers] CU1 (NOLOCK)
		INNER JOIN [dbo].[ChannelUsers] CU2 (NOLOCK) ON [CU1].[ChannelId] = [CU2].[ChannelId]
		INNER JOIN [dbo].[AnonymousUsers] AU ON [AU].[Id] = [CU1].[UserId]
		LEFT JOIN [dbo].[ChannelMessages] CM (NOLOCK) ON [CM].[ChannelId] = [CU1].[ChannelId]
	WHERE [CU2].[UserId] = @UserId  
		AND [CU2].[UserType] = @UserType
		AND ISNULL([CU1].[ChannelId],'') != ''
		AND [CM].[DeletedAt] IS NOT NULL
        AND [AU].[UserType] = 2
		AND [CU1].[UserType] = 'AnonymousUser'
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

	  SELECT
        (
            SELECT SUM(CAST([IsActive] AS INT)) AS ActiveUsers,
                   (
                       SELECT DISTINCT [UserId],[ChatId],[LastMessage],[LastMessageType],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType],[PhoneNumber]
						FROM #TempTables AS t
						ORDER BY t.[LastMessageTime] DESC
						OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
                       FOR JSON PATH
                   ) AS [ChatList]
            FROM #TempTables AS ActiveUsers
            FOR JSON PATH 
        ) AS [ChatListResponse]
END");
            #endregion

            #region SP_GetChatListCustome
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatListCustomer]    Script Date: 10/10/2023 7:51:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_GetChatListCustomer] 
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
		[LastMessage] NVARCHAR(Max),
		[LastMessageType] NVARCHAR(50),
        [LastMessageTime] DATETIME,
        [FirstName] NVARCHAR(50),
        [LastName] NVARCHAR(100),
        [ProfilePicture] NVARCHAR(500),
        [IsActive] BIT,
		[TotalUnReadCount] INT,
		[UserType] NVARCHAR(20),
		[Status] NVARCHAR(20),
		[PhoneNumber] NVARCHAR(20)
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
   ELSE IF(@UserType ='AnonymousUser')
   BEGIN
    SET @ChatUserType = 4
   END

	;WITH FilterOptionsCTE AS (
        SELECT value AS FilterOption
        FROM STRING_SPLIT(@Filter, ',')
    )

	INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageType],[LastMessageTime],[FirstName],[LastName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType],[Status] ,[PhoneNumber])
	SELECT [US].[Id],
	       [CU1].[ChannelId] AS [ChatId],
		   [CM].[Message] AS [LastMessage],
		   [CM].[MessageType] AS [LastMessageType],
		   [CM].[CreatedAt] AS [LastMessageTime] ,
		   [FirstName],
		   [LastName],
		   [US].[ProfilePicture],
		   [CU1].[IsActive],
		   ( SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] 
		     FROM [dbo].[ChannelMessages] CM 
				  WHERE [CM].[ChannelId] = [CU1].[ChannelId]
				        AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
				        AND (
					          (
							    [CM].[MessageSender] = @ChatUserType
							    AND [CM].[MessageSenderId] <> @UserId
					          )
					       OR (
							    [CM].[MessageSender] <> @ChatUserType
					         )
				        )
			) AS [TotalUnReadCount],
		   'HospitioUser' AS [UserType],
		   '' AS [Status],
		   [US].[PhoneNumber] AS [PhoneNumber]
	FROM [dbo].[ChannelUsers] CU1
	INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[channelId] = [CU2].[channelId]
	INNER JOIN [dbo].[Users] US ON [CU1].[UserId] = [US].[Id]
	LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [cu1].[ChannelId] 
	WHERE [cu2].[UserId] = @UserId  
		  AND [cu2].[UserType] = @UserType
		  AND
		  (
			(
				[cu1].[UserType] = @UserType
				AND [cu1].[UserId] <> @UserId
			)
			OR (
				[cu1].[UserType] <> @UserType
			)
		 )
		AND [cu1].[UserType] = 'HospitioUser'
		AND ISNULL(CU1.[ChannelId],'') != ''
		--AND [US].[UserLevelId] = 1
		--AND [CU1].[Id] <> @UserId 
		--AND [CU1].[UserType] = 'HospitioUser'
		AND [CM].[DeletedAt] IS NULL
		AND [CU1].[ChannelId] IN ( SELECT [ChannelId] 
		                           FROM [dbo].[ChannelUsers](NOLOCK) 
								        WHERE [UserId] = @UserId 
										      AND [DeletedAt] IS NULL)
		AND ISNULL([CM].[Id],'') = ISNULL( ( SELECT TOP(1) [CMS].[Id] 
		                                     FROM [dbo].[ChannelMessages] CMS (NOLOCK) 
											      WHERE [CU1].[ChannelId] = [CMS].[ChannelId] 
												  ORDER BY [CMS].[Id] DESC),'')
		AND @ChatType = 'inbox'
	UNION
	SELECT [CG].[Id],
	       [CU1].[ChannelId] AS [ChatId],
		   [CM].[Message] AS [LastMessage],
		   [CM].[MessageType] AS [LastMessageType],
		   [CM].[CreatedAt] AS [LastMessageTime],
		   [CG].[Firstname] AS [FirstName],
		   [CG].[Lastname] AS [LastName],
		   [CG].[Picture] AS [ProfilePicture],[CU1].[IsActive],
		   ( SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] 
		     FROM [dbo].[ChannelMessages] CM 
				  WHERE [CM].[ChannelId] = [CU1].[ChannelId]
				        AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
				        AND (
					          (
							    [CM].[MessageSender] = @ChatUserType
							    AND [CM].[MessageSenderId] <> @UserId
					          )
					        OR (
							    [CM].[MessageSender] <> @ChatUserType
					        )
				)
			) AS [TotalUnReadCount],
			'CustomerGuest' AS [UserType],
			CASE 
			  WHEN ( CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE())
                     AND CONVERT(DATE, [CR].[CheckoutDate]) >= CONVERT(DATE, GETDATE()) ) 
			  THEN 'In-House'
			  WHEN ( CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE())
                     AND CONVERT(DATE, [CR].[CheckoutDate]) < CONVERT(DATE, GETDATE()) ) 
			  THEN 'Checked-out'
			END AS [Status],
			[CG].[PhoneNumber] AS [PhoneNumber]
		FROM [dbo].[ChannelUsers] CU1
		INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[channelId] = [CU2].[channelId]
		INNER JOIN [dbo].[CustomerGuests] CG ON [CU1].[UserId] = [CG].[Id]
		INNER JOIN [dbo].[CustomerReservations] CR ON [CG].[CustomerReservationId] = [CR].[Id] 
		LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] 
		WHERE [cu2].[UserId] = @UserId  
			AND [cu2].[UserType] = @UserType
			AND
			(
				(
					[cu1].[UserType] = @UserType
					AND [cu1].[UserId] <> @UserId
				)
				OR (
					 [cu1].[UserType] <> @UserType
				)
			)
			AND [cu1].[UserType] = 'CustomerGuest'
			AND ISNULL([CU1].[ChannelId],'') != ''
			AND [CR].[CustomerId] = (Select CustomerId from CustomerUsers Where Id = @UserId)
			AND [CM].[DeletedAt] IS NULL
			AND [CU1].[ChannelId] IN ( SELECT [ChannelId] 
			                       FROM [dbo].[ChannelUsers](NOLOCK) 
								   WHERE [UserId] = @UserId 
								   AND [DeletedAt] IS NULL)
		    AND ISNULL([CM].[Id],'') = ISNULL( ( SELECT TOP(1) CMS.Id 
			                                     FROM [dbo].[ChannelMessages] CMS (NOLOCK) 
												 WHERE [CU1].[ChannelId] = [CMS].[ChannelId] 
												 ORDER BY [CMS].[Id] DESC),'')
	       AND (@ChatType = ''
		       OR (
                     @ChatType = 'inbox'
			         AND (
				          SELECT COUNT([CM].[Id])
					      FROM [dbo].[ChannelMessages] CM
					           WHERE [ChannelId] = [CU1].[ChannelId]
					                 AND [MessageSender] = 3
					                 AND [MessageSenderId] = [CG].[Id]
				     ) > 0 -- Users who have received a message and replied to it
                 )
              OR (
                   @ChatType = 'in-bound'
			       AND (
				       (SELECT COUNT([CM].[Id])
					    FROM [dbo].[ChannelMessages] CM
					         WHERE [ChannelId] = [CU1].[ChannelId]
					               AND [MessageSender] = 3
					               AND [MessageSenderId] = [CG].[Id]
				       ) = 0
				 AND ( CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE())
				       AND CONVERT(DATE, [CR].[CheckoutDate]) >= CONVERT(DATE, GETDATE()) 
					  )
			         )
                  )
           OR (
                @ChatType = 'contacted'
			     AND (
				       (
				        SELECT COUNT([CM].[Id])
				        FROM [dbo].[ChannelMessages] CM
				             WHERE [ChannelId] = [CU1].[ChannelId]
				                   AND [MessageSender] = 3
				                   AND [MessageSenderId] = [CG].[Id]
				        ) = 0
				    AND
				    (
					  ( CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE())
					    AND CONVERT(DATE, [CR].[CheckoutDate]) < CONVERT(DATE, GETDATE()))
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
	
	UNION
	SELECT [AU].[Id],
	       [CU1].[ChannelId] AS [ChatId],
		   [CM].[Message] AS [LastMessage],
		   [CM].[MessageType] AS [LastMessageType],
		   [CM].[CreatedAt] AS [LastMessageTime] ,
		   '' AS [FirstName],
		   '' AS [LastName],
		   NULL AS [ProfilePicture],
		   [CU1].[IsActive],
		   ( SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] 
		     FROM [dbo].[ChannelMessages] CM 
				  WHERE [CM].[ChannelId] = [CU1].[ChannelId]
				        AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
				        AND (
					          (
							    [CM].[MessageSender] = @ChatUserType
							    AND [CM].[MessageSenderId] <> @UserId
					          )
					       OR (
							    [CM].[MessageSender] <> @ChatUserType
					         )
				        )
			) AS [TotalUnReadCount],
		   'AnonymousUser' AS [UserType],
		   '' AS [Status],
		   [AU].[PhoneNumber] AS [PhoneNumber]
	FROM [dbo].[ChannelUsers] CU1
	INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[channelId] = [CU2].[channelId]
	INNER JOIN [dbo].[AnonymousUsers] AU ON [AU].[Id] = [CU1].[UserId]
	LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [cu1].[ChannelId] 
	WHERE [cu2].[UserId] = @UserId  
		  AND [cu2].[UserType] = @UserType
		  AND
		  (
			(
				[cu1].[UserType] = @UserType
				AND [cu1].[UserId] <> @UserId
			)
			OR (
				[cu1].[UserType] <> @UserType
			)
		 )
 		AND [cu1].[UserType] = 'AnonymousUser'
        AND [AU].[UserType] = 3
		AND ISNULL([CU1].[ChannelId],'') != ''
		AND [CM].[DeletedAt] IS NULL
		AND [CU1].[ChannelId] IN ( SELECT [ChannelId] 
		                           FROM [dbo].[ChannelUsers](NOLOCK) 
								        WHERE [UserId] = @UserId 
										      AND [DeletedAt] IS NULL)
		AND ISNULL([CM].[Id],'') = ISNULL( ( SELECT TOP(1) [CMS].[Id] 
		                                     FROM [dbo].[ChannelMessages] CMS (NOLOCK) 
											      WHERE [CU1].[ChannelId] = [CMS].[ChannelId] 
												  ORDER BY [CMS].[Id] DESC),'')
		AND @ChatType = 'inbox'


	SELECT
        (
            SELECT SUM(CAST([IsActive] AS INT)) AS ActiveUsers,
                   (
                        SELECT [UserId],
						       [ChatId],
							   [LastMessage],
							   [LastMessageType],
							   [LastMessageTime],
							   [FirstName],
							   [LastName],
							   [ProfilePicture],
							   [IsActive],
							   [TotalUnReadCount],
							   [UserType],
							   [Status],
							   [PhoneNumber]
						FROM #TempTables AS t
						ORDER BY [t].[LastMessageTime] DESC
						OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
                       FOR JSON PATH
                   ) AS [ChatList]
            FROM #TempTables AS ActiveUsers
            FOR JSON PATH 
        ) AS [ChatListResponse]
	
END");
            #endregion

            #region SP_GetGuestDetails
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetGuestDetails]    Script Date: 10/10/2023 7:28:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   Proc [dbo].[SP_GetGuestDetails]
(
@GuestId int = 0,
@UserType VARCHAR(20) = 'CustomerGuest'
)

AS
BEGIN
DECLAre @ChannelId  Int = 0
 Select @ChannelId = IsNULL(CU.ChannelId,0) from Channels C
	INNEr JOIN ChannelUsers CU
	ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
	Where CU.UserId = @GuestId
	AND CU.UserType = @UserType

	If (@ChannelId = 0)	
		BEGIN
			DECLARE @CustomerUserId INT = 0
			DECLARE @CustomerUserType Varchar(20) = 'CustomerUser'
			
			Select @CustomerUserId = CU.Id
			from CustomerGuests CG
			INNER JOIN CustomerReservations CR
			ON CG.CustomerReservationId = CR.Id AND CR.DeletedAt Is NULL
			INNER JOIN Customers C
			ON  CR.CustomerId = C.Id AND C.DeletedAt Is NULL
			INNER JOIN CustomerUsers CU
			ON CU.CustomerId = C.Id AND CU.DeletedAt Is NULL
			Where CG.Id = @GuestId AND CG.DeletedAt Is NULL
			print @CustomerUserId
			INSERT INTO Channels (Uuid,CreateForm,channelUserID,IsActive,CreatedAt,UpdateAt) 
			Values (Lower(NEWID()),@UserType,@GuestId,1,GETUTCDATE(),GETUTCDATE())		
			
			SET @ChannelId = SCOPE_IDENTITY()

			INSERT INTO ChannelUsers (ChannelId,LastMessageReadTime,LastMessageReadId,UserType,UserId,IsActive,CreatedAt,UpdateAt)
			Values (@ChannelId,GETUTCDATE(),NULL,@UserType,@GuestId,1,GETUTCDATE(),GETUTCDATE())
					   
			INSERT INTO ChannelUsers (ChannelId,LastMessageReadTime,LastMessageReadId,UserType,UserId,IsActive,CreatedAt,UpdateAt)
			Values (@ChannelId,GETUTCDATE(),NULL,@CustomerUserType,@CustomerUserId,0,GETUTCDATE(),GETUTCDATE())
		END
	Select Distinct CG.Firstname as GuestFirstName, CG.Lastname AS GuestLastName ,CG.Picture as GuestProfile, CU.FirstName as CustomerFirstName, CU.LastName AS CustomerLastName, 
	CGC.Logo AS CustomerProfile,@ChannelId AS ChannelId
	from CustomerGuests CG
	INNER JOIN CustomerReservations CR
	ON CG.CustomerReservationId = CR.Id AND CR.DeletedAt Is NULL
	INNER JOIN Customers C
	ON  CR.CustomerId = C.Id AND C.DeletedAt Is NULL
	INNER JOIN CustomerUsers CU
	ON CU.CustomerId = C.Id AND CU.DeletedAt Is NULL
	LEFT JOIN CustomerGuestsCheckInFormBuilders CGC
	ON [CGC].[CustomerId] = [C].[Id] AND [CGC].[DeletedAt] IS NULL
	Where CG.Id = @GuestId AND CG.DeletedAt Is NULL
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

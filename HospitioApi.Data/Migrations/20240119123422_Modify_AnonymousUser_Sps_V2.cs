using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_AnonymousUser_Sps_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region SP_GetChatList
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatList]    Script Date: 19-01-2024 18:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER    PROCEDURE [dbo].[SP_GetChatList]
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
				AND (
                    [US].[UserLevelId] <> 1 
                    OR (
                        [US].[UserLevelId] = 1 AND [CM].[Source] <> 3
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
			AND ISNULL([CM].[Id],'') = ISNULL(
											(
												CASE WHEN [US].[UserLevelId] = 1 THEN
													(SELECT TOP(1) [CMS].[Id]
													 FROM [dbo].[ChannelMessages] CMS (NOLOCK)
													 WHERE [CU1].[ChannelId] = [CMS].[ChannelId]
													 AND [CMS].[Source] <> 3
													 ORDER BY [CMS].[Id] DESC)
												ELSE
													(SELECT TOP(1) [CMS].[Id]
													 FROM [dbo].[ChannelMessages] CMS (NOLOCK)
													 WHERE [CU1].[ChannelId] = [CMS].[ChannelId]
													 ORDER BY [CMS].[Id] DESC)
												END
											), '')
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
					--AND (
                    --[US].[CustomerLevelId] <> 1 
                    --OR (
                    --    [US].[CustomerLevelId] = 1 AND [CM].[Source] <> 3
                    --)
                  --)
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
		AND ISNULL([CM].[Id],'') =  ISNULL(
											(
												CASE WHEN (SELECT UU.[UserLevelId] FROM Users UU where UU.[Id] = @UserId ) <> 1 THEN
													(SELECT TOP(1) [CMS].[Id]
													 FROM [dbo].[ChannelMessages] CMS (NOLOCK)
													 WHERE [CU1].[ChannelId] = [CMS].[ChannelId]
													 AND [CMS].[Source] <> 3
													 ORDER BY [CMS].[Id] DESC)
												ELSE
													(SELECT TOP(1) [CMS].[Id]
													 FROM [dbo].[ChannelMessages] CMS (NOLOCK)
													 WHERE [CU1].[ChannelId] = [CMS].[ChannelId]
													 ORDER BY [CMS].[Id] DESC)
												END
											), '')
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
	 INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageType],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType] ,[PhoneNumber])
	    SELECT	[AU].[Id],
		        [CU1].[ChannelId] AS [ChatId],
				[CM].[Message] AS [LastMessage],
				[CM].[MessageType] AS [LastMessageType],
				[CM].[CreatedAt] AS [LastMessageTime] ,
				'' AS [FirstName],
				'' AS [LastName],
				NULL AS [BusinessName],
				NULL AS [ProfilePicture],
				0 AS [IsActive],
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

            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatListByChatId]    Script Date: 19-01-2024 18:06:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER       PROCEDURE [dbo].[SP_GetChatListByChatId]
(
	@ChatId INT = 0,
    @Id INT = 0,
	@Type NVARCHAR(20) = ''    
)
AS
BEGIN
 
    SET NOCOUNT ON
    SET XACT_ABORT ON
		DECLARE @UserType VARCHAR(50)
 
	SELECT @UserType = [CU].[UserType]
	FROM [dbo].[ChannelUsers] CU
	WHERE [CU].[ChannelId] = @ChatId AND [CU].[DeletedAt] IS NULL 
		AND (
		(
				[CU].[UserType] = @Type
				AND [CU].[UserId] <> @Id
		)
		OR (
				[CU].[UserType] <> @Type
		)
	)
	print @UserType
    DECLARE @ChatUserType INT = 0
 
    IF(@Type='HospitioUser')
      BEGIN
	   SET @ChatUserType = 1
      END
    ELSE IF(@Type='CustomerUser')
      BEGIN
	    SET @ChatUserType = 2
      END
    ELSE IF(@Type='CustomerGuest')
      BEGIN
	    SET @ChatUserType = 3
      END
	ELSE IF (@Type = 'ChatWidgetUser')
	  BEGIN
	    SET @ChatUserType= 5
      END
	ELSE
	  BEGIN
	    SET @ChatUserType = 4
      END
 
	BEGIN
 
	IF(@UserType = 'HospitioUser')
	BEGIN
		SELECT	[US].[Id] AS [UserId],
		        [CU1].[ChannelId] AS [ChatId],
				[CM].[Message] AS [LastMessage],
				[CM].[MessageType] AS [LastMessageType],
				[CU1].[LastMessageReadTime] AS [LastMessageTime] ,
				[FirstName],
				[LastName],
				NULL AS [BusinessName],
				[ProfilePicture],
				[CU1].[IsActive],
				( SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] 
				  FROM [dbo].[ChannelMessages] CM 
				  WHERE [CM].[ChannelId] = [CU1].[ChannelId] --AND [CM].[MessageSenderId] <> @UserId 
				        AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
			            AND (
						       (
							      [CM].[MessageSender] = @ChatUserType AND [CM].[MessageSenderId] <> @Id
					           )
					        OR (
							      [CM].[MessageSender] <> @ChatUserType
						       )
					        )
					AND (
                    [US].[UserLevelId] <> 1 
                    OR (
                        [US].[UserLevelId] = 1 AND [CM].[Source] <> 3
                    )
                )
			    ) AS [TotalUnReadCount],
				'HospitioUser' AS [UserType],
                [US].[PhoneNumber],
				 'inbox' AS [Status]
		FROM [dbo].[ChannelUsers] CU1 (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU2 (NOLOCK) ON [CU1].[ChannelId] = [CU2].[ChannelId] 
			INNER JOIN [dbo].[Users] US (NOLOCK) ON [CU1].[UserId] = [US].[Id]
			LEFT JOIN [dbo].[ChannelMessages] CM (NOLOCK) ON [CM].[ChannelId] = [CU1].[ChannelId]			
			             AND ISNULL([CM].[Id],'') = ISNULL(
											(
												CASE WHEN [US].[UserLevelId] = 1 THEN
													(SELECT TOP(1) [CMS].[Id]
													 FROM [dbo].[ChannelMessages] CMS (NOLOCK)
													 WHERE [CU1].[ChannelId] = [CMS].[ChannelId]
													 AND [CMS].[Source] <> 3
													 ORDER BY [CMS].[Id] DESC)
												ELSE
													(SELECT TOP(1) [CMS].[Id]
													 FROM [dbo].[ChannelMessages] CMS (NOLOCK)
													 WHERE [CU1].[ChannelId] = [CMS].[ChannelId]
													 ORDER BY [CMS].[Id] DESC)
												END
											), '')
		WHERE [CU2].[UserId] = @Id  
			AND [CU2].[UserType] = @Type
			AND ISNULL(CU1.[ChannelId],'') != ''
			AND [CM].[DeletedAt] IS NULL
			AND [CU1].[UserType] = 'HospitioUser'
			AND [CU1].[ChannelId] =@ChatId --IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
			AND	(
					(
						[CU1].[UserType] = @Type AND [CU1].[UserId] <> @Id
					)
				OR (
						[CU1].[UserType] <> @Type
					)
				)
    END
 
   ELSE IF (@UserType ='CustomerUser')
   BEGIN
		SELECT	[US].[Id] AS [UserId],
		        [CU1].[ChannelId] AS [ChatId],
				[CM].[Message] AS [LastMessage],
				[CM].[MessageType] AS [LastMessageType],
				[CU1].[LastMessageReadTime] AS [LastMessageTime],
				[US].[FirstName] AS [FirstName],
				[US].[LastName] AS [LastName],
				[C].[BusinessName],
				CASE 
					WHEN [US].[CustomerLevelId] = 1
						THEN [CGC].[Logo]
					ELSE [US].[ProfilePicture]
				END AS [ProfilePicture], 
				[CU1].[IsActive],
				(
					SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] 
					FROM [dbo].[ChannelMessages] CM 
					WHERE [CM].[ChannelId] = [CU1].[ChannelId]	--AND [CM].[MessageSenderId] <> @UserId 
						  AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
						  AND (
								(
									[CM].[MessageSender] = @ChatUserType AND [CM].[MessageSenderId] <> @Id
								)
							OR (
									[CM].[MessageSender] <> @ChatUserType
								)
							)
					  -- AND (
       --                [US].[CustomerLevelId] <> 1 -- No additional condition for UserLevelId = 1
       --                 OR (
       --                    [US].[CustomerLevelId] = 1 AND [CM].[Source] <> 3
       --                  )
					  --)
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType],
               [US].[PhoneNumber],
			   'inbox' AS [Status]
		FROM [dbo].[ChannelUsers] CU1
			INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[ChannelId] = [CU2].[ChannelId]
			INNER JOIN [dbo].[CustomerUsers] US ON [CU1].[UserId] = [US].[Id]
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] 
			          AND ISNULL([CM].[Id],'') = ISNULL(
											(
												CASE WHEN [US].[CustomerLevelId] = 1 THEN
													(SELECT TOP(1) [CMS].[Id]
													 FROM [dbo].[ChannelMessages] CMS (NOLOCK)
													 WHERE [CU1].[ChannelId] = [CMS].[ChannelId]
													 AND [CMS].[Source] <> 3
													 ORDER BY [CMS].[Id] DESC)
												ELSE
													(SELECT TOP(1) [CMS].[Id]
													 FROM [dbo].[ChannelMessages] CMS (NOLOCK)
													 WHERE [CU1].[ChannelId] = [CMS].[ChannelId]
													 ORDER BY [CMS].[Id] DESC)
												END
											), '')
			INNER JOIN [dbo].[Customers] C ON [C].[Id] = [US].[CustomerId]
			LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] CGC
			    ON [CGC].[CustomerId]  = [C].[Id]
		WHERE [CU2].[UserId] = @Id  
		AND [CU2].[UserType] = @Type
		AND ISNULL(CU1.[ChannelId],'') != ''
		AND [CM].[DeletedAt] IS NULL
		AND [CU1].[UserType] = 'CustomerUser'
		AND [CU1].[ChannelId] = @ChatId
		AND
		(
			(
				[CU1].[UserType] = @Type AND [cu1].[UserId] <> @Id
			)
		OR (
				[cu1].[UserType] <> @Type
			)
		)
	END
 
	ELSE IF (@UserType = 'CustomerGuest')
	BEGIN
	  SELECT [CG].[Id],
	         [CU1].[ChannelId] AS [ChatId],
			 [CM].[Message] AS [LastMessage],
			 [CM].[MessageType] AS [LastMessageType],
			 [CU1].[LastMessageReadTime] AS [LastMessageTime],
			 [CG].[Firstname] AS [FirstName],
			 [CG].[Lastname] AS [LastName],
			 [CG].[Picture]  AS [ProfilePicture],
			 [CU1].[IsActive],
			 (SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] 
			         FROM [dbo].[ChannelMessages] CM 
				     WHERE  [CM].[ChannelId] = [CU1].[ChannelId]
				            AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
				            AND (
					             (
					 	        	[CM].[MessageSender] = @ChatUserType
						        	AND [CM].[MessageSenderId] <> @Id
					             )
				               	OR 
								(
						    	[CM].[MessageSender] <> @ChatUserType
					             )
				             )
			) AS [TotalUnReadCount],
			'CustomerGuest' AS [UserType],
            [CG].[PhoneNumber],
			CASE WHEN ((  SELECT COUNT([CM].[Id])
					      FROM [dbo].[ChannelMessages] CM
					           WHERE [ChannelId] = [CU1].[ChannelId]
					                 AND [MessageSender] = 3
					                 AND [MessageSenderId] = [CG].[Id]
				     ) > 0)  THEN 'inbox'
			WHEN (  (   SELECT COUNT([CM].[Id])
					    FROM [dbo].[ChannelMessages] CM
					         WHERE [ChannelId] = [CU1].[ChannelId]
					               AND [MessageSender] = 3
					               AND [MessageSenderId] = [CG].[Id]
				       ) = 0
				 AND ( CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE())
				       AND CONVERT(DATE, [CR].[CheckoutDate]) >= CONVERT(DATE, GETDATE()) 
					  )) THEN 'in-bound'
			WHEN (  (
				        SELECT COUNT([CM].[Id])
				        FROM [dbo].[ChannelMessages] CM
				             WHERE [ChannelId] = [CU1].[ChannelId]
				                   AND [MessageSender] = 3
				                   AND [MessageSenderId] = [CG].[Id]
				        ) = 0
				    AND
				    (
					  ( CONVERT(DATE, [CR].[CheckinDate]) > CONVERT(DATE, GETDATE())
					    AND CONVERT(DATE, [CR].[CheckoutDate]) > CONVERT(DATE, GETDATE()))
				     )) THEN 'contacted'
			END AS [Status]
		FROM [dbo].[ChannelUsers] CU1
		INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[channelId] = [CU2].[channelId]
		INNER JOIN [dbo].[CustomerGuests] CG ON [CU1].[UserId] = [CG].[Id]
		INNER JOIN [dbo].[CustomerReservations] CR ON [CG].[CustomerReservationId] = [CR].[Id] 
		LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] 
		WHERE [cu2].[UserId] = @Id
		    AND [CU2].[UserType] <> 'CustomerGuest'
			AND
			(
				(
						[cu1].[UserType] = @Type
						AND [cu1].[UserId] <> @Id
				)
				OR (
						[cu1].[UserType] <> @Type
				)
			)
			AND cu1.UserType = 'CustomerGuest'
			AND ISNULL(CU1.[ChannelId],'') != ''
			AND CR.CustomerId = CR.CustomerId
			AND [CM].[DeletedAt] IS NULL
			AND CU1.ChannelId = @ChatId
		    AND ISNULL([CM].[Id],'') = ISNULL( ( SELECT TOP(1) [CMS].[Id] 
			                                     FROM [dbo].[ChannelMessages] CMS (NOLOCK) 
												 WHERE [CU1].[ChannelId] = [CMS].[ChannelId] 
												 ORDER BY [CMS].[Id] DESC),'')
	END
 
	ELSE IF(@UserType ='AnonymousUser')
	BEGIN
	SELECT [AU].[Id],
	       [CU1].[ChannelId] AS [ChatId],
	       [CM].[Message] AS [LastMessage],
		   [CM].[MessageType] AS [LastMessageType],
		   [CU1].[LastMessageReadTime] AS [LastMessageTime],
		   '' AS [FirstName],
		   '' AS [LastName],
		   NULL AS [BusinessName],
		   NULL AS [ProfilePicture],
		   [CU1].[IsActive],
		   ( SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] 
		     FROM [dbo].[ChannelMessages] CM 
				WHERE  [CM].[ChannelId] = [CU1].[ChannelId]
				AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
				AND (
					(
							[CM].[MessageSender] = @ChatUserType
							AND [CM].[MessageSenderId] <> @Id
					)
					OR (
							[CM].[MessageSender] <> @ChatUserType
					)
				)
			) AS [TotalUnReadCount],
		   'AnonymousUser' AS [UserType],
           [AU].[PhoneNumber],
		   	CASE WHEN ( @Type = 'CustomerUser' ) THEN 'inbox'
			     WHEN ( @Type = 'HospitioUser' ) THEN 'deleted'
			END AS [Status]
	FROM [dbo].[ChannelUsers] CU1
	INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[ChannelId] = [CU2].[ChannelId]
	INNER JOIN [dbo].[AnonymousUsers] AU ON [AU].[Id] = [CU1].[UserId] AND [AU].[DeletedAt] IS NULL
	LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId]
	WHERE [CU2].[UserId] = @Id
		  AND [CU1].[UserType] = 'AnonymousUser'
		  AND ISNULL(CU1.[ChannelId], '') != ''
		  AND [CM].[DeletedAt] IS NULL
		  AND [CU1].[ChannelId] = @ChatId
		  AND ISNULL([CM].[Id],'') = ISNULL( ( SELECT TOP(1) [CMS].[Id] 
		                                       FROM [dbo].[ChannelMessages] CMS (NOLOCK) 
											   WHERE [CU1].[ChannelId] = [CMS].[ChannelId] 
											   ORDER BY [CMS].[Id] DESC),'')
          AND
		  (
		    ( [CU1].[UserType] = @Type
			   AND [CU1].[UserId] <> @Id
			)
			OR
			(
			 [CU1].[UserType] <> @Type
			)
		  )
	END
 
	ELSE IF(@UserType ='ChatWidgetUser')
	BEGIN
	SELECT [CWU].[Id],
	       [CU1].[ChannelId] AS [ChatId],
	       [CM].[Message] AS [LastMessage],
		   [CM].[MessageType] AS [LastMessageType],
		   [CU1].[LastMessageReadTime] AS [LastMessageTime],
		   'ChatWidgetUser' AS [FirstName],
		   CONVERT(NVARCHAR(MAX), [CWU].[Id]) AS [LastName],
		   NULL AS [BusinessName],
		   NULL AS [ProfilePicture],
		   [CU1].[IsActive],
		   ( SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] 
		     FROM [dbo].[ChannelMessages] CM 
				WHERE  [CM].[ChannelId] = [CU1].[ChannelId]
				AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
				AND (
					(
							[CM].[MessageSender] = @ChatUserType
							AND [CM].[MessageSenderId] <> @Id
					)
					OR (
							[CM].[MessageSender] <> @ChatUserType
					)
				)
			) AS [TotalUnReadCount],
		   'ChatWidgetUser' AS [UserType],
           NULL AS [PhoneNumber],
           'inbox' AS [Status]
	FROM [dbo].[ChannelUsers] CU1
	INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[ChannelId] = [CU2].[ChannelId]
	INNER JOIN [dbo].[ChatWidgetUsers] [CWU] ON [CWU].[Id] = [CU1].[UserId] AND [CWU].[DeletedAt] IS NULL
	LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId]
	WHERE [CU2].[UserId] = @Id
		  AND [CU1].[UserType] = 'ChatWidgetUser'
		  AND ISNULL(CU1.[ChannelId], '') != ''
		  AND [CM].[DeletedAt] IS NULL
		  AND [CU1].[ChannelId] = @ChatId
		  AND ISNULL([CM].[Id],'') = ISNULL( ( SELECT TOP(1) [CMS].[Id] 
		                                       FROM [dbo].[ChannelMessages] CMS (NOLOCK) 
											   WHERE [CU1].[ChannelId] = [CMS].[ChannelId] 
											   ORDER BY [CMS].[Id] DESC),'')
          AND
		  (
		    ( [CU1].[UserType] = @Type
			   AND [CU1].[UserId] <> @Id
			)
			OR
			(
			 [CU1].[UserType] <> @Type
			)
		  )
	END
 
END
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_Anonymous_Chat_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region SP_GetChatListCustomer
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatListCustomer]    Script Date: 27/09/2023 5:12:28 PM ******/
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
		   [ProfilePicture],
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
		   [CG].[Picture],[CU1].[IsActive],
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
	
END

");
            #endregion

            #region SP_GetChatList
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatList]    Script Date: 27/09/2023 5:20:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[SP_GetChatList]
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
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType],
				[US].[PhoneNumber] AS [PhoneNumber]
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
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType],
				[US].[PhoneNumber] AS [PhoneNumber]
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

            #region SP_GetChatMessageList
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatMessageList]    Script Date: 27/09/2023 5:21:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SP_GetChatMessageList]
(
    @ChatId INT = 0,
    @PageNo INT = 1,
    @PageSize INT = 10
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

SELECT(
    SELECT [CM].[Id],
           [CM].[ChannelId],
           [CM].[MessageType],
           [CM].[MessageSender],
           [CM].[Source],
           [CM].[MsgReqType],
           [CM].[Attachment],
           [CM].[RequestId],
           [CM].[Url],
           [CM].[Message],
           [CM].[TranslateMessage],
           [CM].[IsActive],
           [CM].[CreatedAt],
		   [CM].[IsRead],
		   [CM].[MessageSenderId],
		   [CM].[RequestType],
		   CASE 
			WHEN MessageSender = 1  THEN 'HospitioUser'
			WHEN MessageSender = 2  THEN 'CustomerUser'
			WHEN MessageSender = 3  THEN 'CustomerGuest'
			WHEN MessageSender = 4  THEN 'AnonymousUser'
			END AS [UserType],
		   CASE 
			WHEN [CM].[RequestType] = 1 THEN [GR].[Status] 
			WHEN [CM].[RequestType] = 2 THEN [ER].[Status]
			END AS [RequestStatus]
    FROM [dbo].[ChannelMessages] CM (NOLOCK)
		LEFT JOIN GuestRequests GR
			ON [CM].[RequestId] = [GR].[Id]
		LEFT JOIN EnhanceStayItemExtraGuestRequests ER
			ON [CM].[RequestId] = [ER].[Id]
    WHERE [CM].[DeletedAt] IS NULL
          AND [CM].[ChannelId] = @ChatId
    ORDER BY [CM].[CreatedAt] DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
	FOR JSON PATH
	)

END");
            #endregion

            #region SP_GetTotalUnReadMessageCount
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetTotalUnReadMessageCount]    Script Date: 27/09/2023 5:22:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[SP_GetTotalUnReadMessageCount]   
(
	 @UserId INT = 0,
	 @UserType INT = 0,
	 @ChatUserType VARCHAR(20) = ''
)
AS
BEGIN
	
	SET NOCOUNT ON
    SET XACT_ABORT ON
	
	IF(@UserType = 3 OR @UserType = 4)
	BEGIN
		SELECT COUNT(DISTINCT([CM].[Id])) AS [TotalUnreadCount]
		FROM [dbo].[Channels] C (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU 
				ON [C].[Id] = [CU].[ChannelId] 
					AND [CU].[UserId] = @UserId
					AND [CU].[UserType] = @ChatUserType
			INNER JOIN [dbo].[ChannelMessages] CM  
				ON  [CM].[ChannelId] = [C].[Id] 
					AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0)
					AND (
						(
								CM.MessageSender = @UserType
								AND CM.MessageSenderId <> @UserId
						)
						OR (
								CM.MessageSender <> @UserType
						)
					)
	END
	ELSE 
	BEGIN
		SELECT COUNT(DISTINCT([C].[Id])) AS [TotalUnreadCount]
		FROM [dbo].[Channels] C (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU 
				ON [C].[Id] = [CU].[ChannelId] 
					AND [CU].[UserId] = @UserId
					AND [CU].[UserType] = @ChatUserType
			INNER JOIN [dbo].[ChannelMessages] CM  
				ON  [CM].[ChannelId] = [C].[Id] 
					AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0)
					AND (
						(
								CM.MessageSender = @UserType
								AND CM.MessageSenderId <> @UserId
						)
						OR (
								CM.MessageSender <> @UserType
						)
					)
	END
END");
            #endregion

            #region SP_ReadChatMessage
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_ReadChatMessage]    Script Date: 27/09/2023 5:24:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[SP_ReadChatMessage]
(
	 @ChatId INT = 0,
	 @UserId INT = 0,
	 @UserType INT = 0
)
AS
BEGIN
	
SET NOCOUNT ON;
SET XACT_ABORT ON;

	DECLARE @LastMsgReadId INT = 0
	DECLARE @TotalUnreadCount INT = 0

	DECLARE @ChatUserType VARCHAR(50)

   IF(@UserType = 1 )
   BEGIN
	SET @ChatUserType = 'HospitioUser'
   END
   ELSE IF(@UserType = 2)
   BEGIN
	SET @ChatUserType = 'CustomerUser'
   END
   ELSE IF(@UserType=3)
   BEGIN
	SET @ChatUserType = 'CustomerGuest'
   END
   ELSE IF(@UserType=4)
   BEGIN
    SET @ChatUserType = 'AnonymousUser'
   END

	IF EXISTS (SELECT * FROM [dbo].[ChannelMessages](NOLOCK) CM 
				INNER JOIN [dbo].[ChannelUsers](NOLOCK) CU 
				ON [CU].[ChannelId] = [CM].[ChannelId] 
				   AND [CU].[DeletedAt] IS NULL
				WHERE [CM].[ChannelId] = @ChatId AND [CU].[DeletedAt] IS NULL
				AND (
						(
							[CM].[MessageSender] = @UserType AND [CM].[MessageSenderId] <> @UserId
						)
					OR (
							[CM].[MessageSender] <> @UserType
						)
					)
		)
		BEGIN
			SELECT TOP(1) @LastMsgReadId = [CM].[Id] 
			FROM [dbo].[ChannelMessages](NOLOCK) CM 
				INNER JOIN [dbo].[ChannelUsers](NOLOCK) CU 
					ON [CU].[ChannelId] = [CM].[ChannelId] 
					AND [CU].[DeletedAt] IS NULL
			WHERE [CM].[ChannelId] = @ChatId 
				AND [CU].[DeletedAt] IS NULL
				AND (
						(
							[CM].[MessageSender] = @UserType AND [CM].[MessageSenderId] <> @UserId
						)
					OR (
							[CM].[MessageSender] <> @UserType
						)
					)
				ORDER BY [CM].[Id] DESC

			IF(ISNULL(@LastMsgReadId,0) > 0)
			BEGIN
				UPDATE [dbo].[ChannelUsers] SET 
					[LastMessageReadId] = @LastMsgReadId, 
					[LastMessageReadTime] = GETUTCDATE()
				WHERE [ChannelId] = @ChatId
					AND [DeletedAt] IS NULL
					AND UserType = @ChatUserType 
					AND UserId = @UserId

				SELECT @TotalUnreadCount = COUNT([CM].[Id])  
				FROM [dbo].[ChannelMessages] CM 
					INNER JOIN [dbo].[ChannelUsers] CU ON [CU].[ChannelId] = [CM].[ChannelId]
					AND (
							(
								[CU].[UserType] = @ChatUserType AND [CU].[UserId] <> @UserId
							)
						OR (
								[CU].[UserType] <> @ChatUserType
							)
						)
				WHERE [CM].[ChannelId] = @ChatId
				   -- AND [CU].[UserId] = @UserId
					AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
					AND (
							(
								[CM].[MessageSender] = @UserType AND [CM].[MessageSenderId] <> @UserId
							)
						OR (
								[CM].[MessageSender] <> @UserType
							)
						)

				SELECT @ChatId AS [ChatId], @TotalUnreadCount AS [TotalUnreadCount]
			END
		END
		ELSE
		BEGIN
			SELECT 0 AS [ChatId],0 AS [TotalUnReadCount]
		END
 END");
            #endregion

            #region SP_GETUserDetailByChatId
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GETUserDetailByChatId]    Script Date: 27/09/2023 5:25:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_GETUserDetailByChatId]
(
	@ChatId INT = 0,
	@Id INT = 0,
	@Type VARCHAR(20) = ''
)
AS
BEGIN
	SET NOCOUNT ON;
    SET XACT_ABORT ON

	DECLARE @UserType NVARCHAR(100) 
	DECLARE @UserId INT
	DECLARE @IsActive BIT

	SELECT @UserType = [CU].[UserType], @UserId = [CU].[UserId],@IsActive = ISNULL([CU].[IsActive],0) FROM ChannelUsers (NOLOCK) CU
	WHERE CU.ChannelId = @ChatId 
	AND (
		(
				CU.UserType = @Type
				AND CU.UserId <> @Id
		)
		OR (
				CU.UserType <> @Type
		)
	)

	IF(@UserType = 'CustomerUser')
	BEGIN
		SELECT [C].[BusinessName],
		       NULL AS[FirstName],
			   NULL AS [LastName],
			   [C].[Email],
			   NULL AS [ProfilePicture],
			   [C].[PhoneCountry],
			   [C].[PhoneNumber],
			   [C].[IncomingTranslationLangage],
			   [C].[NoOfRooms],
			   [BT].[BizType],
			   [P].[Name] AS [ServicePackageName],
			   [C].[CreatedAt],
			   @UserType AS [UserType] ,
			   @UserId AS [UserId], 
			   @ChatId AS [ChatId],
			   @IsActive AS [IsActive],
			   [C].[DeActivated],
			   NULL AS [Status],
			   NUll AS [CheckinDate],
			   NULL AS [CheckoutDate],
			   NULL AS [BlePinCode]
		FROM [dbo].[Customers](NOLOCK) C 
			LEFT JOIN [dbo].[BusinessTypes] BT (NOLOCK) 
				ON [BT].[Id] = [C].[BusinessTypeId] 
					AND [BT].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[Products] P (NOLOCK) 
				ON [P].[Id] = [C].[ProductId] 
				AND  [P].[DeletedAt] IS NULL
			INNER JOIN [dbo].[CustomerUsers] CU
				ON [CU].CustomerId = [C].[Id]
					AND  [CU].[DeletedAt] IS NULL
		WHERE [CU].[Id] = @UserId
			AND [C].[DeletedAt] IS NULL
	END
	ELSE IF(@UserType = 'HospitioUser')
	BEGIN
		SELECT	NULL AS [BusinessName],
		        [U].[FirstName],
				[U].[LastName],
				[U].[Email],
				[U].[ProfilePicture],
				[U].[PhoneCountry],
				[U].[PhoneNumber],
				NULL AS [IncomingTranslationLangage],
				NULL AS [NoOfRooms],
				NULL AS [BizType],
				NULL AS [ServicePackageName],
				[U].[CreatedAt],
				@UserType AS [UserType] ,
				@UserId AS [UserId], 
				@ChatId AS [ChatId],
				@IsActive AS [IsActive],
				[U].[DeActivated], 
				NULL AS [Status],
				NUll AS [CheckinDate], 
				NULL AS [CheckoutDate],
				NULL AS [BlePinCode]
			FROM [dbo].[Users] U (NOLOCK)
			WHERE [U].[DeletedAt] IS NULL
				AND [U].[Id] = @UserId
	END
	ELSE IF(@UserType = 'CustomerGuest')
	BEGIN
		SELECT	NULL AS [BusinessName],
		        [CG].[Firstname] AS [FirstName],
		        [CG].[Lastname] AS [LastName],
		        [CG].[Email],
		        [CG].[Picture] AS [ProfilePicture],
		        [CG].[PhoneCountry],
		        [CG].[PhoneNumber],
		        NULL AS [IncomingTranslationLangage],
		        [CG].[RoomNumber] AS [NoOfRooms],
		        NULL AS [BizType],
				NULL AS [ServicePackageName],
				[CR].[CreatedAt],
				@UserType AS [UserType] ,
				@UserId AS [UserId], 
				@ChatId AS [ChatId],
				@IsActive AS [IsActive],
				[C].[DeActivated],
				CASE 
					WHEN (CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, [CR].[CheckoutDate]) >= CONVERT(DATE, GETDATE())) 
						THEN 'In-House'
					WHEN (CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, [CR].[CheckoutDate]) < CONVERT(DATE, GETDATE())) 
						THEN 'Checked-out'
				END AS [Status], 
				[CR].[CheckinDate], 
				[CR].[CheckoutDate],
				[CG].[BlePinCode]
        FROM [dbo].[CustomerGuests](NOLOCK) CG
            INNER JOIN [dbo].[CustomerReservations](NOLOCK) CR ON [CR].[Id] = [CG].[CustomerReservationId] AND [CR].[DeletedAt] IS NULL
			INNER JOIN [Customers](NOLOCK) C ON [C].[Id] = [CR].[CustomerId] AND [C].DeletedAt IS NULL
        WHERE [CG].[DeletedAt] IS NULL
			AND [CG].[Id] = @UserId
	END
	ELSE IF(@UserType = 'AnonymousUser')
	BEGIN
		SELECT	NULL AS [BusinessName],
		        NULL AS [FirstName],
			    NULL AS [LastName],
				NULL AS [Email],
			    NULL AS [ProfilePicture],
				NULL AS [PhoneCountry],
				[AU].[PhoneNumber],
				NULL AS [IncomingTranslationLangage],
				NULL AS [NoOfRooms],
				NULL AS [BizType],
				NULL AS [ServicePackageName],
				[AU].[CreatedAt],
				@UserType AS [UserType] ,
				@UserId AS [UserId], 
				@ChatId AS [ChatId],
				@IsActive AS [IsActive],
				NULL AS [DeActivated], 
				NULL AS [Status],
				NUll AS [CheckinDate], 
				NULL AS [CheckoutDate],
				NULL AS [BlePinCode]
			FROM [dbo].[AnonymousUsers] AU (NOLOCK)
			WHERE [AU].[DeletedAt] IS NULL
				AND [AU].[Id] = @UserId
	END
END");
            #endregion

            #region SP_GetChatListByChatId
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatListByChatId]    Script Date: 27/09/2023 5:27:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_GetChatListByChatId]
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
			    ) AS [TotalUnReadCount],
				'HospitioUser' AS [UserType],
                [US].[PhoneNumber]
		FROM [dbo].[ChannelUsers] CU1 (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU2 (NOLOCK) ON [CU1].[ChannelId] = [CU2].[ChannelId] 
			INNER JOIN [dbo].[Users] US (NOLOCK) ON [CU1].[UserId] = [US].[Id]
			LEFT JOIN [dbo].[ChannelMessages] CM (NOLOCK) ON [CM].[ChannelId] = [CU1].[ChannelId]
		WHERE [CU2].[UserId] = @Id  
			AND [CU2].[UserType] = @Type
			AND ISNULL(CU1.[ChannelId],'') != ''
			AND [CM].[DeletedAt] IS NULL
			AND [CU1].[UserType] = 'HospitioUser'
			AND [CU1].[ChannelId] =@ChatId --IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
			AND ISNULL([CM].[Id],'') = ISNULL( ( SELECT TOP(1) [CMS].[Id] 
			                                     FROM [dbo].[ChannelMessages] CMS (NOLOCK) 
												 WHERE [CMS].[ChannelId] = [CU1].[ChannelId] 
												 ORDER BY [CMS].[Id] DESC),'')
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
				'' AS [FirstName],
				'' AS [LastName],
				[C].[BusinessName],
				[US].[ProfilePicture],
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
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType],
               [US].[PhoneNumber]
		FROM [dbo].[ChannelUsers] CU1
			INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[ChannelId] = [CU2].[ChannelId]
			INNER JOIN [dbo].[CustomerUsers] US ON [CU1].[UserId] = [US].[Id]
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] 
			INNER JOIN [dbo].[Customers] C ON [C].[Id] = [US].[CustomerId]
		WHERE [CU2].[UserId] = @Id  
		AND [CU2].[UserType] = @Type
		AND ISNULL(CU1.[ChannelId],'') != ''
		AND [CM].[DeletedAt] IS NULL
		AND [CU1].[UserType] = 'CustomerUser'
		AND [CU1].[ChannelId] = @ChatId
		AND ISNULL([CM].[Id],'') = ISNULL( ( SELECT TOP(1) [CMS].[Id] 
		                                     FROM [dbo].[ChannelMessages] CMS (NOLOCK) 
											 WHERE [CMS].[ChannelId] = [CU1].[ChannelId] 
											 ORDER BY [CMS].[Id] DESC),'')
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
			 [CG].[Picture],
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
			CASE WHEN (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
            AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE())) THEN 'In-House'
			WHEN (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
            AND CONVERT(DATE, CR.CheckoutDate) < CONVERT(DATE, GETDATE())) THEN 'Checked-out'
			END AS [Status]
		FROM [dbo].[ChannelUsers] CU1
		INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[channelId] = [CU2].[channelId]
		INNER JOIN [dbo].[CustomerGuests] CG ON [CU1].[UserId] = [CG].[Id]
		INNER JOIN [dbo].[CustomerReservations] CR ON [CG].[CustomerReservationId] = [CR].[Id] 
		LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] 
		WHERE [cu2].[UserId] = @Id
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
           [AU].[PhoneNumber]
	FROM [dbo].[ChannelUsers] CU1
	INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[ChannelId] = [CU2].[ChannelId]
	INNER JOIN [dbo].[AnonymousUsers] AU ON [AU].[Id] = [CU1].[UserId]
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

 END
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

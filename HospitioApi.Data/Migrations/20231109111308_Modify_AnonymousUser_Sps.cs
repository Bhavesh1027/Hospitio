using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_AnonymousUser_Sps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region SP_GetUserDetailFromPhoneNumber
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserDetailFromPhoneNumber]    Script Date: 9/11/2023 4:45:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER    PROCEDURE [dbo].[SP_GetUserDetailFromPhoneNumber]
    @PhoneNumber NVARCHAR(50),
	@AnonymousUsersType NVARCHAR(50) =0,
	@AnonymousUserId INT = 0
AS
BEGIN
	DECLARE @UserId INT = null;
	DECLARE @UserType VARCHAR(20) = '';

    DECLARE @NormalizedPhoneNumber NVARCHAR(50)
    SET @NormalizedPhoneNumber = REPLACE(REPLACE(@PhoneNumber, '+', ''), ' ', '')

	IF(@AnonymousUsersType = 'HospitioUser')
	BEGIN
	  -- Find matching records in the 'users' table
		SELECT TOP 1 @UserId = Id, @UserType = '1'
		FROM Users
		WHERE REPLACE(REPLACE(PhoneNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
		AND DeletedAt IS NULL
		AND UserLevelId NOT IN (1)

	 -- Find matching records in the 'customers' table
     IF @UserId IS NULL
      BEGIN
        SELECT TOP 1 @UserId = cu.Id, @UserType = '2'
        FROM Customers c
        INNER JOIN CustomerUsers cu ON c.Id = cu.CustomerId
        WHERE REPLACE(REPLACE(c.WhatsappNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
		AND cu.DeletedAt IS NULL
		AND cu.CustomerLevelId = 1
      END
	END
	ELSE IF(@AnonymousUsersType = 'CustomerUser')
	BEGIN
	 -- Find matching records in the 'users' table
			SELECT TOP 1 @UserId = Id, @UserType = '1'
			FROM Users
			WHERE REPLACE(REPLACE(PhoneNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
			AND DeletedAt IS NULL

	-- Find matching records in the 'customers' table
		 IF @UserId IS NULL
			BEGIN
				SELECT TOP 1 @UserId = cu.Id, @UserType = '2'
				FROM CustomerUsers CU
				WHERE REPLACE(REPLACE(PhoneNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
				AND cu.DeletedAt IS NULL
				AND CU.CustomerLevelId NOT IN (1)
				AND CU.CustomerId = (SELECT [CUS].[CustomerId] FROM CustomerUsers CUS where CUS.[Id] = @AnonymousUserId )
			END

	 -- Find matching records in the 'customerGuest' table
			IF @UserId IS NULL
				BEGIN
					SELECT TOP 1 @UserId = C.Id, @UserType = '3'
					FROM CustomerGuests C
						 INNER JOIN CustomerReservations CR
						 ON CR.[Id] = C.[CustomerReservationId]
					WHERE REPLACE(REPLACE(C.PhoneNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
						  AND C.DeletedAt IS NULL
						  AND [CR].[CustomerId] = (SELECT [CUS].[CustomerId] FROM CustomerUsers CUS where CUS.[Id] = @AnonymousUserId )
				END

	END
	ELSE 
	BEGIN
	 -- Find matching records in the 'users' table
		SELECT TOP 1 @UserId = Id, @UserType = '1'
		FROM HospitioOnboardings
		WHERE REPLACE(REPLACE(WhatsappNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
		AND DeletedAt IS NULL

    -- Find matching records in the 'customers' table
		IF @UserId IS NULL
		BEGIN
			SELECT TOP 1 @UserId = cu.Id, @UserType = '2'
			FROM Customers c
			INNER JOIN CustomerUsers cu ON c.Id = cu.CustomerId
			WHERE REPLACE(REPLACE(c.WhatsappNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
			AND cu.DeletedAt IS NULL
		END
	END

	IF @UserId IS NULL
    BEGIN
	  IF(@AnonymousUsersType = 'HospitioUser')
		  BEGIN
			SELECT TOP 1 @UserId = c.Id, @UserType = '4'
			FROM AnonymousUsers c
			WHERE REPLACE(REPLACE(c.PhoneNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
				  AND c.UserType = 2
				  AND DeletedAt IS NULL
		  END
	  ELSE 
		  BEGIN
		  	SELECT TOP 1 @UserId = c.Id, @UserType = '4'
			FROM AnonymousUsers c
			WHERE REPLACE(REPLACE(c.PhoneNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
				  AND c.UserType = 3
				  AND DeletedAt IS NULL
		  END

    END

	select @UserId As UserId, @UserType As UserType
END");
            #endregion

            #region SP_GetReceiverUserForWP
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetReceiverUserForWP]    Script Date: 9/11/2023 4:46:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[SP_GetReceiverUserForWP] 
	@ChatId int = 0,
	@UserId int =0,
	@UserType varchar(50)
AS
BEGIN

	SET NOCOUNT ON;
	IF(@UserType = 'HospitioUser')
	BEGIN
		SELECT CU.Id,CU.UserId, CU.UserType,U.PhoneNumber AS Phonenumber FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN Users U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		WHERE C.Id = @ChatId AND C.DeletedAt IS NULL 
		AND CU.UserType = 'HospitioUser'
		AND CU.UserId NOT IN (@UserId)

		UNION

		SELECT CU.Id,CU.UserId, CU.UserType,C1.WhatsappNumber AS Phonenumber FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN CustomerUsers U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		INNER JOIN Customers C1 ON C1.Id = U.CustomerId AND U.DeletedAt IS NULL
		WHERE C.Id = @ChatId AND C.DeletedAt IS NULL 
		AND CU.UserType = 'CustomerUser'

		UNION

		SELECT CU.Id,CU.UserId, CU.UserType,U.PhoneNumber AS Phonenumber FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN AnonymousUsers U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		WHERE C.Id = @ChatId AND C.DeletedAt IS NULL AND U.UserType = 2
		AND CU.UserType = 'AnonymousUser'
	END

	IF(@UserType = 'CustomerUser')
	BEGIN
		SELECT CU.Id,CU.UserId, CU.UserType
		,U.PhoneNumber AS Phonenumber 
		FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN Users U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		WHERE C.Id = @ChatId AND C.DeletedAt IS NULL 
		AND CU.UserType = 'HospitioUser'

		UNION

		SELECT CU.Id,CU.UserId, CU.UserType,U.PhoneNumber AS Phonenumber FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN CustomerUsers U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		WHERE C.Id = @ChatId AND C.DeletedAt IS NULL
		AND CU.UserType = 'CustomerUser'
		AND CU.UserId NOT IN (@UserId)

		UNION

		SELECT CU.Id,CU.UserId, CU.UserType,U.PhoneNumber AS Phonenumber FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN AnonymousUsers U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		WHERE C.Id = @ChatId AND C.DeletedAt IS NULL
		AND CU.UserType = 'AnonymousUser'

		UNION

		SELECT CU.Id,CU.UserId, CU.UserType,U.PhoneNumber AS Phonenumber FROM Channels C
		INNER JOIN ChannelUsers CU ON CU.ChannelId = C.Id AND CU.DeletedAt IS NULL
		INNER JOIN CustomerGuests U ON U.Id = CU.UserId AND U.DeletedAt IS NULL
		WHERE C.Id = @ChatId AND C.DeletedAt IS NULL
		AND CU.UserType = 'CustomerGuest'
	END
END");
            #endregion

            #region SP_GetChatMessageList
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatMessageList]    Script Date: 9/11/2023 4:48:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SP_GetChatMessageList]
(
    @ChatId INT = 0,
    @PageNo INT = 1,
    @PageSize INT = 10,
	@UserType NVARCHAR(50) ,
	@UserId INT = 0
)
AS
BEGIN

    DECLARE @query NVARCHAR(MAX) = ''
    
	SET @query = '
	SELECT (
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
			WHEN MessageSender = 1  THEN '''+'HospitioUser'+'''
			WHEN MessageSender = 2  THEN '''+'CustomerUser'+'''
			WHEN MessageSender = 3  THEN '''+'CustomerGuest'+'''
			WHEN MessageSender = 4  THEN '''+'AnonymousUser'+'''
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
          AND [CM].[ChannelId] = CAST(''' + CAST(@ChatId AS  NVARCHAR(MAX))  + ''' AS INT )'
	IF(@UserType = 'CustomerGuest')
	BEGIN
	 SET @query += ' AND [CM].[Source] =  1'
	END

	IF(@UserType = 'HospitioUser' AND ( SELECT [U].[UserLevelId] FROM Users U WHERE U.Id = @UserId ) NOT IN (1) )
	BEGIN
	  SET @query += ' AND [CM].[Source] NOT IN (3) '
	END

	IF(@UserType = 'CustomerUser' AND ( SELECT [CU].[CustomerLevelId] FROM CustomerUsers CU WHERE CU.Id = @UserId ) NOT IN (1))
	BEGIN
	  SET @query += ' AND [CM].[Source] NOT IN (3) ' 
	END

	SET @query += ' 	ORDER BY [CM].[CreatedAt] DESC OFFSET CAST(''' + CAST(@PageSize AS NVARCHAR(MAX))  + ''' AS INT ) * (  CAST(''' + CAST(@PageNo AS NVARCHAR(MAX)) + ''' AS INT)  - 1) ROWS FETCH NEXT CAST(''' + CAST(@PageSize AS NVARCHAR(MAX))  + ''' AS INT ) ROWS ONLY
	FOR JSON PATH 
	)'

    EXEC sp_executesql @query

END");
            #endregion

            #region SP_GetTotalUnReadMessageCount
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetTotalUnReadMessageCount]    Script Date: 9/11/2023 4:48:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_GetTotalUnReadMessageCount]   
(
	 @UserId INT = 0,
	 @UserType INT = 0,
	 @ChatUserType VARCHAR(20) = ''
)
AS
BEGIN
	
	SET NOCOUNT ON
    SET XACT_ABORT ON
	
	IF(@UserType = 3)
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
					AND [CM].[Source] = 1
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
	ELSE IF ( @UserType = 4 )
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
	ELSE IF ( @UserType = 2)
	BEGIN
		SELECT COUNT(DISTINCT([C].[Id])) AS [TotalUnreadCount]
		FROM [dbo].[Channels] C (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU 
				ON [C].[Id] = [CU].[ChannelId] 
					AND [CU].[UserId] = @UserId
					AND [CU].[UserType] = @ChatUserType
			INNER JOIN [dbo].[ChannelMessages] CM  
				ON  [CM].[ChannelId] = [C].[Id] 
            INNER JOIN [dbo].[CustomerUsers] CUS
			    ON [CUS].[Id] = [CU].[UserId]
				AND [CUS].[DeletedAt] IS NULL
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
		  WHERE ( [CUS].[CustomerLevelId] <> 1 AND [CM].[Source] <> 3)
                   OR ([CUS].[CustomerLevelId] = 1)
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
                INNER JOIN [dbo].[Users] U
				    ON [U].[Id] = [CU].[UserId]
					   AND [U].[DeletedAt] IS NULL
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
                WHERE ( [U].[UserLevelId] <> 1 AND [CM].[Source] <> 3)
                   OR ( [U].[UserLevelId] = 1)
	END
END");
            #endregion

            #region SP_GetChatList
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatList]    Script Date: 9/11/2023 4:49:43 PM ******/
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
					AND (
                    [US].[CustomerLevelId] <> 1 
                    OR (
                        [US].[CustomerLevelId] = 1 AND [CM].[Source] <> 3
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

            #region SP_GetChatListCustomer
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatListCustomer]    Script Date: 9/11/2023 4:51:24 PM ******/
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
			END AS [Status],
			[CG].[PhoneNumber] AS [PhoneNumber]
		FROM [dbo].[ChannelUsers] CU1
		INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[channelId] = [CU2].[channelId]
		INNER JOIN [dbo].[CustomerGuests] CG ON [CU1].[UserId] = [CG].[Id] AND [CG].[DeletedAt] IS NULL
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
					  ( CONVERT(DATE, [CR].[CheckinDate]) > CONVERT(DATE, GETDATE())
					    AND CONVERT(DATE, [CR].[CheckoutDate]) > CONVERT(DATE, GETDATE()))
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
		   0 AS [IsActive],
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

		UNION
			SELECT [CUS].[Id],
	       [CU1].[ChannelId] AS [ChatId],
		   [CM].[Message] AS [LastMessage],
		   [CM].[MessageType] AS [LastMessageType],
		   [CM].[CreatedAt] AS [LastMessageTime] ,
		   [CUS].[FirstName],
		   [CUS].[LastName],
		   	CASE 
			 WHEN [CUS].[CustomerLevelId] = 1
				  THEN [CGC].[Logo]
			 ELSE [CUS].[ProfilePicture]
				END AS [ProfilePicture], 
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
					AND (
                       [CUS].[CustomerLevelId] <> 1 -- No additional condition for UserLevelId = 1
                        OR (
                           [CUS].[CustomerLevelId] = 1 AND [CM].[Source] <> 3
                         )
					  )
			) AS [TotalUnReadCount],
		   'CustomerUser' AS [UserType],
		   '' AS [Status],
		   [CUS].[PhoneNumber] AS [PhoneNumber]
		FROM [dbo].[ChannelUsers] CU1
		INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[channelId] = [CU2].[channelId]
		INNER JOIN [dbo].[CustomerUsers] CUS ON [CU1].[UserId] = [CUS].[Id]
		LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [cu1].[ChannelId] 
		LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] CGC
			    ON [CGC].[CustomerId]  = [CUS].[CustomerId]
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
		AND [cu1].[UserType] = 'CustomerUser'
		AND ISNULL(CU1.[ChannelId],'') != ''
		--AND [US].[UserLevelId] = 1
		--AND [CU1].[Id] <> @UserId 
		--AND [CU1].[UserType] = 'HospitioUser'
		AND [CM].[DeletedAt] IS NULL
		AND [CU1].[ChannelId] IN ( SELECT [ChannelId] 
		                           FROM [dbo].[ChannelUsers](NOLOCK) 
								        WHERE [UserId] = @UserId 
										      AND [DeletedAt] IS NULL)
		AND ISNULL([CM].[Id], '') = ISNULL(
											(
												CASE WHEN [CUS].[CustomerLevelId] = 1 THEN
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

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

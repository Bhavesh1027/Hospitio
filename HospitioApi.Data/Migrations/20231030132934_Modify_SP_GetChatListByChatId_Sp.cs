using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_SP_GetChatListByChatId_Sp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region SP_GetChatListByChatId
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatListByChatId]    Script Date: 30/10/2023 5:30:22 PM ******/
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
                [US].[PhoneNumber],
				 'inbox' AS [Status]
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
               [US].[PhoneNumber],
			   'inbox' AS [Status]
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

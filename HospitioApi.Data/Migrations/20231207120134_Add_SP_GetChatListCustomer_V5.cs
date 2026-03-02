using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_SP_GetChatListCustomer_V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatListCustomer]    Script Date: 07-12-2023 15:44:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER    PROCEDURE [dbo].[SP_GetChatListCustomer]
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
   ELSE IF(@UserType = 'ChatWidgetUser')
   BEGIN
    SET @ChatUserType = 5
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

		UNION
 
	SELECT [CWU].[Id],
			   [CU1].[ChannelId] AS [ChatId],
			   [CM].[Message] AS [LastMessage],
			   [CM].[MessageType] AS [LastMessageType],
			   [CM].[CreatedAt] AS [LastMessageTime] ,
			   'ChatWidgetUser' AS [FirstName],
			   CONVERT(NVARCHAR(MAX), [CWU].[Id]) AS [LastName],
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
			   'ChatWidgetUser' AS [UserType],
			   'inbox' AS [Status],
			   NULL AS [PhoneNumber]
		FROM [dbo].[ChannelUsers] CU1
		INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[channelId] = [CU2].[channelId]
		INNER JOIN [dbo].[ChatWidgetUsers] CWU ON [CWU].[Id] = [CU1].[UserId] and [CWU].[DeletedAt] IS NULL
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
			AND [cu1].[UserType] = 'ChatWidgetUser'
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

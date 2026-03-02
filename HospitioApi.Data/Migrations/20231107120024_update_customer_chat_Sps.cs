using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class update_customer_chat_Sps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region SP_Get_Communication_CustomerGuest
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_Get_Communication_CustomerGuest]    Script Date: 7/11/2023 5:31:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SP_Get_Communication_CustomerGuest] 
(
	@CustomerId INT = 0,
	@SearchString NVARCHAR(50) = NULL,
	@PageNo INT = 1,
    @PageSize INT = 10,
	@CustomerUserLevel NVARCHAR(100) = NULL,
	@CustomerUserId INT =0
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

	SET @SearchString = LTRIM(RTRIM(@SearchString));

	IF OBJECT_ID('tempdb..#TempTables') IS NOT NULL
        DROP TABLE #TempTables;

    CREATE TABLE #TempTables
    (
        [Id] INT,
		[CustomerId] INT,
        [CustomerReservationId] NVARCHAr(100),
        [FirstName] NVARCHAR(50),
        [LastName] NVARCHAR(50),
        [Email] NVARCHAR(100),
        [Picture] NVARCHAR(500),
        [PhoneCountry] NVARCHAR(3),
        [PhoneNumber] NVARCHAR(20),
        [Country] NVARCHAR(10),
        [Language] NVARCHAR(10),
		[UserType] NVARCHAR(20),
		[ChatType] NVARCHAR(20),
		[FullName] NVARCHAR(Max)
   )

   INSERT INTO #TempTables ([Id],[CustomerId],[CustomerReservationId],[FirstName],[LastName],[Email],[Picture],[PhoneCountry],[PhoneNumber],[Country],[Language],[UserType],[ChatType],[FullName])
    SELECT [CG].[Id],
           [CR].[CustomerId],
		   [CG].[CustomerReservationId],
           [CG].[Firstname],
           [CG].[Lastname],
           [CG].[Email],
           [CG].[Picture],
           [CG].[PhoneCountry],
           [CG].[PhoneNumber],
           [CG].[Country],
           [CG].[Language],
		   'CustomerGuest',
		   (CASE
				WHEN (
						SELECT COUNT(CM.Id)
						FROM ChannelMessages CM
						WHERE ChannelId = CU1.ChannelId
						AND MessageSender = 3
						AND MessageSenderId = CG.Id
					) > 0
				THEN 'inbox'
				WHEN 
					(SELECT COUNT(CM.Id)
						FROM ChannelMessages CM
						WHERE ChannelId = CU1.ChannelId
						AND MessageSender = 3
						AND MessageSenderId = CG.Id
					) = 0
					AND (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
					AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE()))
				
				THEN 'in-bound'
				WHEN (
						(
						SELECT COUNT(CM.Id)
						FROM ChannelMessages CM
						WHERE ChannelId = CU1.ChannelId
						AND MessageSender = 3
						AND MessageSenderId = CG.Id
						) = 0
						AND
						(
							(CONVERT(DATE, CR.CheckinDate) > CONVERT(DATE, GETDATE())
							AND CONVERT(DATE, CR.CheckoutDate) > CONVERT(DATE, GETDATE()))
						)
				)
				THEN 'contacted'
				ELSE ''
			END) AS ChatType,
			[Firstname] +' '+[Lastname] AS [FullName]
    FROM [dbo].[CustomerGuests] CG (NOLOCK)
        INNER JOIN [dbo].[CustomerReservations] CR (NOLOCK) ON [CR].[Id] = [CG].[CustomerReservationId] AND [CG].[DeletedAt] IS NULL
		LEFT JOIN [dbo].[ChannelUsers] CU1 ON CU1.UserId = CG.Id AND CU1.UserType = 'CustomerGuest'
		LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] 
    WHERE [CR].[DeletedAt] IS NULL 
		AND [CR].[CustomerId] = @CustomerId
		AND @CustomerUserLevel = 'Super Admin'
    UNION
    SELECT [CU].[Id],
           [CU].[CustomerId] AS [CustomerId],
		   NULL AS [CustomerReservationId],
           [CU].[FirstName],
           [CU].[LastName],
           [CU].[Email],
		   CASE 
				WHEN [CU].[CustomerLevelId] = 1
					THEN [CGC].[Logo]
				ELSE [CU].[ProfilePicture]
		   END AS [ProfilePicture], 
           [CU].[PhoneCountry],
           [CU].[PhoneNumber],
           NULL AS [Country],
           NULL AS [Language],
		   'CustomerUser',
		   '' AS [ChatType],
		   [Firstname] +' '+[Lastname] AS [FullName]
    FROM [dbo].[CustomerUsers] CU (NOLOCK)
	     LEFT JOIN [dbo].[Customers] C		
		 LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] CGC
			    ON [CGC].[CustomerId]  = [C].[Id]
		 ON [C].Id = [CU].[CustomerId]
		    AND [C].[DeletedAt] IS NULL
			AND [CU].[CustomerLevelId] = 1
    WHERE [CU].[DeletedAt] IS NULL
	      AND [CU].[CustomerId] = @CustomerId
		  AND [CU].[Id] NOT IN (@CustomerUserId)

    UNION

	 SELECT [U].[Id],
           NULL AS [CustomerId],
		   NULL AS [CustomerReservationId],
           [U].[FirstName],
           [U].[LastName],
           [U].[Email],
           [U].[ProfilePicture],
           [U].[PhoneCountry],
           [U].[PhoneNumber],
           NULL AS [Country],
           NULL AS [Language],
		   'HospitioUser',
		   '' AS [ChatType],
		   [Firstname] +' '+[Lastname] AS [FullName]
    FROM [dbo].[Users] U (NOLOCK)
    WHERE [U].[DeletedAt] IS NULL 
	      AND UserLevelId = 1
	      AND @CustomerUserLevel = 'Super Admin'


            SELECT	[Id],
					[CustomerId],
					[CustomerReservationId],
					[FirstName],
					[LastName],
					[Email],
					[Picture],
					[PhoneCountry],
					[PhoneNumber],
					[Country],
					[Language],
					[UserType],
					[ChatType],
					[FullName]
            FROM #TempTables
			WHERE 
			(
				[FirstName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[LastName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR
				[FullName] LIKE '%' + ISNULL(@SearchString,'') + '%'
			)
			ORDER BY [CustomerId] ASC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY


END");
            #endregion

            #region SP_Get_AdminUserCustomersSearch
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_Get_AdminUserCustomersSearch]    Script Date: 7/11/2023 5:34:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_Get_AdminUserCustomersSearch] 
(
	@SearchString NVARCHAR(50) = NULL,
	@PageNo INT = 1,
    @PageSize INT = 10,
	@UserId INT = 0
)
AS
BEGIN
	
	SET NOCOUNT ON
    SET XACT_ABORT ON

	SET @SearchString = LTRIM(RTRIM(@SearchString));

	IF OBJECT_ID('tempdb..#TempTables') IS NOT NULL
        DROP TABLE #TempTables;

    CREATE TABLE #TempTables
    (
        [CustomerId] INT,
		[BusinessName] NVARCHAR(100),
        [FirstName] NVARCHAR(50),
        [LastName] NVARCHAR(50),
        [Email] NVARCHAR(100),
        [Title] NVARCHAR(5),
        [ProfilePicture] NVARCHAR(500),
        [PhoneCountry] NVARCHAR(3),
        [PhoneNumber] NVARCHAR(20),
        [UserName] NVARCHAR(100),
        [UserType] NVARCHAR(255)
   )

	INSERT INTO #TempTables ([CustomerId],[BusinessName],[FirstName],[LastName],[Email],[Title],[ProfilePicture],[PhoneCountry],[PhoneNumber],[UserName],[UserType])
	SELECT	[C].[Id],[C].[BusinessName],NULL AS [FirstName],NULL AS [LastName],[C].[Email],NULL AS [Title],[CGC].[Logo] AS [ProfilePicture],[C].[PhoneCountry],[C].[PhoneNumber],NULL AS [UserName],'CustomerUser'
	FROM [dbo].[Customers] C (NOLOCK) 
    LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] CGC
			    ON [CGC].[CustomerId]  = [C].[Id]
	WHERE [C].[DeletedAt] IS NULL 
	UNION ALL
	SELECT	[U].[Id],NULL AS [BusinessName],[U].[FirstName],[U].[LastName],[U].[Email],[U].[Title],[U].[ProfilePicture],[U].[PhoneCountry],[U].[PhoneNumber],[U].[UserName],'HospitioUser'
	FROM [dbo].[Users] U (NOLOCK)
	WHERE [U].[DeletedAt] IS NULL AND [U].[Id] != @UserId

	SELECT (
            SELECT	[CustomerId] AS [Id],
					[BusinessName],
					[FirstName],
					[LastName],
					[Email],
					[Title],
					[ProfilePicture],
					[PhoneCountry],
					[PhoneNumber],
					[UserName],
					[UserType],
                   JSON_QUERY(
                   (
                       SELECT	[CU].[Id],NULL AS [BusinessName],[CU].[FirstName],[CU].[LastName],[CU].[Email],[CU].[ProfilePicture],[CU].[PhoneCountry],[CU].[PhoneNumber]
                       FROM [dbo].[CustomerUsers] CU (NOLOCK)
                       WHERE [CU].[DeletedAt] IS NULL 
						AND [CU].[CustomerId] = #TempTables.[CustomerId]
                       FOR JSON PATH
                   )
                 ) AS [UserOuts]
            FROM #TempTables
			WHERE 
			(
				[BusinessName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[FirstName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[LastName] LIKE '%' + ISNULL(@SearchString,'') + '%'
			)
			ORDER BY [CustomerId] ASC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
            FOR JSON PATH
        );
END
");
            #endregion

            #region SP_GetChatListCustomer
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatListCustomer]    Script Date: 7/11/2023 5:35:04 PM ******/
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

            #region SP_GETUserDetailByChatId
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GETUserDetailByChatId]    Script Date: 7/11/2023 5:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SP_GETUserDetailByChatId]
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
		       [CU].[FirstName] AS[FirstName],
			   [CU].[LastName] AS [LastName],
			   [CU].[Email],
				CASE 
					WHEN [CU].[CustomerLevelId] = 1
						THEN [CGC].[Logo]
					ELSE [CU].[ProfilePicture]
				END AS [ProfilePicture], 
			   [CU].[PhoneCountry],
			   [CU].[PhoneNumber],
			   [C].[IncomingTranslationLangage],
			   [C].[NoOfRooms],
			   [BT].[BizType],
			   [P].[Name] AS [ServicePackageName],
			   [CU].[CreatedAt],
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
			LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] CGC
			    ON [CGC].[CustomerId]  = [C].[Id]
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
				0 AS [IsActive],
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
/****** Object:  StoredProcedure [dbo].[SP_GetChatListByChatId]    Script Date: 7/11/2023 5:38:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SP_GetChatListByChatId]
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
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType],
               [US].[PhoneNumber],
			   'inbox' AS [Status]
		FROM [dbo].[ChannelUsers] CU1
			INNER JOIN [dbo].[ChannelUsers] CU2 ON [CU1].[ChannelId] = [CU2].[ChannelId]
			INNER JOIN [dbo].[CustomerUsers] US ON [CU1].[UserId] = [US].[Id]
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] 
			INNER JOIN [dbo].[Customers] C ON [C].[Id] = [US].[CustomerId]
			LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] CGC
			    ON [CGC].[CustomerId]  = [C].[Id]
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

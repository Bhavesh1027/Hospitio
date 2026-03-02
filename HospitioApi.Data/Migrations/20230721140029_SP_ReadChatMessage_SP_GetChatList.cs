using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_ReadChatMessage_SP_GetChatList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetChatList]
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
		SELECT	[US].[Id],CU1.[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CU1].[LastMessageReadTime] AS [LastMessageTime] ,[FirstName],[LastName],NULL AS [BusinessName],
				[ProfilePicture],[CU1].[IsActive],
				(SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] FROM [dbo].[ChannelMessages] CM WHERE  [CM].[ChannelId] = [CU1].[ChannelId] --AND [CM].[MessageSenderId] <> @UserId 
				AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
				AND (
						(
							[CM].[MessageSender] = @ChatUserType AND [CM].[MessageSenderId] <> @UserId
						)
					OR (
							[CM].[MessageSender] <> @ChatUserType
						)
					)
			) 
			
			AS [TotalUnReadCount],'HospitioUser' AS [UserType]
		FROM [dbo].[ChannelUsers] CU1 (NOLOCK)
			INNER JOIN [dbo].[ChannelUsers] CU2 (NOLOCK) ON [CU1].[ChannelId] = [CU2].[ChannelId] 
			INNER JOIN [dbo].[Users] US (NOLOCK) ON [CU1].[UserId] = [US].[Id]
			LEFT JOIN [dbo].[ChannelMessages] CM (NOLOCK) ON [CM].[ChannelId] = [CU1].[ChannelId]
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
					SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] FROM [dbo].[ChannelMessages] CM WHERE  [CM].[ChannelId] = [CU1].[ChannelId]	--AND [CM].[MessageSenderId] <> @UserId 
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
			INNER JOIN [dbo].[CustomerUsers] US ON [CU1].[UserId] = [US].[Id]
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] 
			INNER JOIN [dbo].[Customers] C ON [C].[Id] = [US].[CustomerId]
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
				[CU1].[UserType] = @UserType AND [cu1].[UserId] <> @UserId
			)
		OR (
				[cu1].[UserType] <> @UserType
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
						ORDER BY t.[ChatId] ASC
						OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
                       FOR JSON PATH
                   ) AS [ChatList]
            FROM #TempTables AS ActiveUsers
            FOR JSON PATH 
        ) AS [ChatListResponse]
END");
			migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_ReadChatMessage]
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

	IF EXISTS (SELECT * FROM [dbo].[ChannelMessages](NOLOCK) CM 
				INNER JOIN [dbo].[ChannelUsers](NOLOCK) CU 	ON [CU].[ChannelId] = [CM].[ChannelId] AND [CU].[DeletedAt] IS NULL
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

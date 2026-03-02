using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetChatListCustomer_SP_GetChatList_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[SP_GetChatList]    Script Date: 07-08-2023 17:11:15 ******/
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
        [LastMessageTime] DATETIME,
        [FirstName] NVARCHAR(50),
        [LastName] NVARCHAR(100),
		[BusinessName] NVARCHAR(100),
        [ProfilePicture] NVARCHAR(500),
        [IsActive] BIT,
		[TotalUnReadCount] INT,
		[UserType] NVARCHAR(20),
		[LastMessageType] NVARCHAR(MAX)
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

		INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageTime],[LastMessageType],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType])
		SELECT	[US].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CM].[CreatedAt] AS [LastMessageTime] ,[CM].[MessageType] AS [LastMessageType],[FirstName],[LastName],NULL AS [BusinessName],
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

		SELECT	[C].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CM].[MessageType] AS [LastMessageType],[CM].[CreatedAt] AS [LastMessageTime],'' AS [FirstName],'' AS [LastName],[C].[BusinessName],
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
		INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageTime],[LastMessageType],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType])
		SELECT	[US].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CM].[CreatedAt] AS [LastMessageTime] ,[CM].[MessageType] AS [LastMessageType],[FirstName],[LastName],NULL AS [BusinessName],
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

		SELECT	[C].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CM].[CreatedAt] AS [LastMessageTime],[CM].[MessageType] AS [LastMessageType],'' AS [FirstName],'' AS [LastName],[C].[BusinessName],
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
                       SELECT DISTINCT [UserId],[ChatId],[LastMessage],[LastMessageTime],[LastMessageType],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType]
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

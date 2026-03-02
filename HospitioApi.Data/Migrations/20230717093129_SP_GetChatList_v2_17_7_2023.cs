using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetChatList_v2_17_7_2023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetChatList]
(
    @UserId INT = 0,
    @PageNo INT = 1,
    @PageSize INT = 10,
	@IsDeleted BIT= 0
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

	IF(@IsDeleted = 0)
	BEGIN
		INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType])
		SELECT [US].[Id],[CT].[ChannelId] AS [ChatId],CM.[Message] AS [LastMessage],[CT].[LastMessageReadTime] AS [LastMessageTime] ,[FirstName],[LastName],'' AS [BusinessName],[ProfilePicture],
			   [CT].[IsActive],
			   (SELECT COUNT([C].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
				INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
				INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
				) AS [TotalUnReadCount],'HospitioUser' AS [UserType]
			FROM [dbo].[Users] (NOLOCK) US
			LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [US].[Id]
			--LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[Id] = [CT].[LastMessageReadId] 
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CT].[ChannelId] AND CT.UserId = CM.MessageSenderId
		WHERE ISNULL([CT].[ChannelId],'') != ''
			AND [US].[Id] <> @UserId 
			AND [CT].[UserType] = 'HospitioUser'
			AND [CM].[DeletedAt] IS NULL
			AND CT.ChannelId IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
			AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) INNER JOIN [dbo].[ChannelUsers] CU (NOLOCK) ON [CU].[ChannelId] = [CMS].[ChannelId] AND [CU].[UserId] = [CMS].[MessageSenderId] AND CU.UserId <> @UserId ORDER BY CMS.Id DESC),'')
		UNION ALL
		SELECT	[CU].[Id],[CT].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CT].[LastMessageReadTime] AS [LastMessageTime],'' AS [FirstName],'' AS [LastName],[C].[BusinessName],
				[CU].[ProfilePicture],[CT].[IsActive],
				(SELECT COUNT([C].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
					INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
					INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType]
		FROM [dbo].[Customers] C 
			LEFT JOIN [dbo].[CustomerUsers] CU ON [CU].[CustomerId] = [C].[Id]
			--LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [C].[Id]
			LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [CU].[Id]
			--LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[Id] = [CT].[LastMessageReadId] 
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CT].[ChannelId] AND CT.UserId = CM.MessageSenderId
		WHERE ISNULL([CT].[ChannelId],'') != ''
			AND [CU].[Id] <> @UserId
			AND [CT].[UserType] = 'CustomerUser'
			AND [CM].[DeletedAt] IS NULL
			AND CT.ChannelId IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
			AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) INNER JOIN [dbo].[ChannelUsers] CU (NOLOCK) ON [CU].[ChannelId] = [CMS].[ChannelId] AND [CU].[UserId] = [CMS].[MessageSenderId] AND CU.UserId <> @UserId ORDER BY CMS.Id DESC),'')
	END
	ELSE IF(@IsDeleted = 1)
	BEGIN
		INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType])
		SELECT	[US].[Id],[CT].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage], [CT].[LastMessageReadTime] AS [LastMessageTime] ,[FirstName],[LastName],'' AS [BusinessName],[ProfilePicture],
				[CT].[IsActive],
				(SELECT COUNT([C].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
					INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
					INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
				) AS [TotalUnReadCount],'HospitioUser' AS [UserType]
			FROM [dbo].[Users] (NOLOCK) US
			LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [US].[Id]
			--LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[Id] = [CT].[LastMessageReadId]
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CT].[ChannelId] AND CT.UserId = CM.MessageSenderId
		WHERE ISNULL([CT].[ChannelId],'') != ''
			AND [US].[Id] <> @UserId 
			AND [CT].[UserType] = 'HospitioUser'
			AND [CM].[DeletedAt] IS NOT NULL
			AND CT.ChannelId IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers] WHERE [UserId] = @UserId)
			AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) INNER JOIN [dbo].[ChannelUsers] CU (NOLOCK) ON [CU].[ChannelId] = [CMS].[ChannelId] AND [CU].[UserId] = [CMS].[MessageSenderId] AND CU.UserId <> @UserId ORDER BY CMS.Id DESC),'')
		UNION
		SELECT [C].[Id],[CT].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CT].[LastMessageReadTime] AS [LastMessageTime],'' AS [FirstName],'' AS [LastName],[C].[BusinessName],
			   [CU].[ProfilePicture],[CT].[IsActive],
			   (SELECT COUNT([C].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
					INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
					INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType]
		FROM [dbo].[Customers] C 
			LEFT JOIN [dbo].[CustomerUsers] (NOLOCK) CU ON [CU].[CustomerId] = [C].[Id]
			--LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [C].[Id]
			LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [CU].[Id]
			--LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[Id] = [CT].[LastMessageReadId]
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CT].[ChannelId] AND CT.UserId = CM.MessageSenderId
		WHERE ISNULL([CT].[ChannelId],'') != ''
			AND [C].[Id] <> @UserId
			AND [CT].[UserType] = 'CustomerUser'
			AND [CM].[DeletedAt] IS NOT NULL
			AND [CT].[ChannelId] IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers] (NOLOCK) WHERE [UserId] = @UserId)
			AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) INNER JOIN [dbo].[ChannelUsers] CU (NOLOCK) ON [CU].[ChannelId] = [CMS].[ChannelId] AND [CU].[UserId] = [CMS].[MessageSenderId] AND CU.UserId <> @UserId ORDER BY CMS.Id DESC),'')
	END

	  SELECT
        (
            SELECT SUM(CAST([IsActive] AS INT)) AS ActiveUsers,
                   (
                       SELECT [UserId],[ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType]
						FROM #TempTables AS t
						ORDER BY t.[ChatId] ASC
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

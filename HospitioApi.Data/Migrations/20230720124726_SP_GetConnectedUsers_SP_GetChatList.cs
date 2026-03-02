using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetConnectedUsers_SP_GetChatList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER       PROCEDURE [dbo].[SP_GetConnectedUsers]
	@UserId int =0,
	@UserType nvarchar(20)
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT  cu1.UserId, cu1.ChannelId AS ChatId,
		CASE
			WHEN (cu1.UserType = 'HospitioUser') THEN 1
			WHEN (cu1.UserType = 'CustomerUser') THEN 2
			WHEN (cu1.UserType = 'CustomerGuest') THEN 3
		END AS [UserType]
	FROM channelUsers cu1
	INNER JOIN channelUsers cu2 ON cu1.channelId = cu2.channelId
	WHERE cu2.UserId = @UserId  
		AND cu2.UserType = @UserType
		AND
		(
					(
							cu1.UserType = @UserType
							AND cu1.UserId <> @UserId
					)
					OR (
							cu1.UserType <> @UserType
					)
				)

END");

			migrationBuilder.Sql(@"CREATE OR ALTER     PROCEDURE [dbo].[SP_GetChatList]
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

	IF(@IsDeleted = 0)
	BEGIN
		INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType])
		SELECT [US].[Id],CU1.[ChannelId] AS [ChatId],CM.[Message] AS [LastMessage],CU1.[LastMessageReadTime] AS [LastMessageTime] ,[FirstName],[LastName],NULL AS [BusinessName],[ProfilePicture],
			CU1.[IsActive],
			(SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
			INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
			INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
			) AS [TotalUnReadCount],'HospitioUser' AS [UserType]
	FROM [dbo].[ChannelUsers] CU1
	INNER JOIN [dbo].[ChannelUsers] CU2 ON CU1.channelId = CU2.channelId
	INNER JOIN [dbo].[Users] US ON CU1.UserId = US.Id
	LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = cu1.[ChannelId]
	WHERE CU2.UserId = @UserId  
		AND CU2.UserType = @UserType
		AND ISNULL(CU1.[ChannelId],'') != ''
		AND [CM].[DeletedAt] IS NULL
		AND CU1.UserType = 'HospitioUser'
		AND CU1.ChannelId IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
		AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) where CMs.ChannelId = CU1.ChannelId ORDER BY CMS.Id DESC),'')
		AND
		(
			(
				cu1.UserType = @UserType
				AND cu1.UserId <> @UserId
			)
		OR (
				cu1.UserType <> @UserType
			)
		)

	UNION

		SELECT	[C].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CU1].[LastMessageReadTime] AS [LastMessageTime],'' AS [FirstName],'' AS [LastName],[C].[BusinessName],
				[US].[ProfilePicture],[CU1].[IsActive],
				(SELECT COUNT([C].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
				INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
				INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType]
		FROM [dbo].[ChannelUsers] CU1
			INNER JOIN [dbo].[ChannelUsers] CU2 ON CU1.channelId = CU2.channelId
			INNER JOIN [dbo].[CustomerUsers] US ON CU1.UserId = US.Id
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = cu1.[ChannelId] 
			INNER JOIN Customers C ON C.Id = US.CustomerId
			WHERE CU2.UserId = @UserId  
		AND CU2.UserType = @UserType
		AND ISNULL(CU1.[ChannelId],'') != ''
		AND [CM].[DeletedAt] IS NULL
		AND CU1.UserType = 'CustomerUser'
		AND CU1.ChannelId IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
		AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) where CMs.ChannelId = CU1.ChannelId ORDER BY CMS.Id DESC),'')
		AND
		(
			(
				cu1.UserType = @UserType
				AND cu1.UserId <> @UserId
			)
		OR (
				cu1.UserType <> @UserType
			)
		)
	END
	ELSE IF(@IsDeleted = 1)
	BEGIN
		INSERT INTO #TempTables ([UserId],[ChatId],[LastMessage],[LastMessageTime],[FirstName],[LastName],[BusinessName],[ProfilePicture],[IsActive],[TotalUnReadCount],[UserType])
		SELECT [US].[Id],CU1.[ChannelId] AS [ChatId],CM.[Message] AS [LastMessage],CU1.[LastMessageReadTime] AS [LastMessageTime] ,[FirstName],[LastName],NULL AS [BusinessName],[ProfilePicture],
			CU1.[IsActive],
			(SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
			INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
			INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
			) AS [TotalUnReadCount],'HospitioUser' AS [UserType]
	FROM [dbo].[ChannelUsers] CU1
	INNER JOIN [dbo].[ChannelUsers] CU2 ON CU1.channelId = CU2.channelId
	INNER JOIN [dbo].[Users] US ON CU1.UserId = US.Id
	LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = cu1.[ChannelId]
	WHERE CU2.UserId = @UserId  
		AND CU2.UserType = @UserType
		AND ISNULL(CU1.[ChannelId],'') != ''
		AND [CM].[DeletedAt] IS NOT NULL
		AND CU1.UserType = 'HospitioUser'
		AND CU1.ChannelId IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
		AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) where CMs.ChannelId = CU1.ChannelId ORDER BY CMS.Id DESC),'')
		AND
		(
			(
				cu1.UserType = @UserType
				AND cu1.UserId <> @UserId
			)
		OR (
				cu1.UserType <> @UserType
			)
		)
		UNION
		SELECT	[C].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CU1].[LastMessageReadTime] AS [LastMessageTime],'' AS [FirstName],'' AS [LastName],[C].[BusinessName],
				[US].[ProfilePicture],[CU1].[IsActive],
				(SELECT COUNT([C].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
				INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
				INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
				) AS [TotalUnReadCount],'CustomerUser' AS [UserType]
		FROM [dbo].[ChannelUsers] CU1
			INNER JOIN [dbo].[ChannelUsers] CU2 ON CU1.channelId = CU2.channelId
			INNER JOIN [dbo].[CustomerUsers] US ON CU1.UserId = US.Id
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = cu1.[ChannelId] 
			INNER JOIN Customers C ON C.Id = US.CustomerId
			WHERE CU2.UserId = @UserId  
		AND CU2.UserType = @UserType
		AND ISNULL(CU1.[ChannelId],'') != ''
		AND [CM].[DeletedAt] IS NULL
		AND CU1.UserType = 'CustomerUser'
		AND CU1.ChannelId IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
		AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) where CMs.ChannelId = CU1.ChannelId ORDER BY CMS.Id DESC),'')
		AND
		(
			(
				cu1.UserType = @UserType
				AND cu1.UserId <> @UserId
			)
		OR (
				cu1.UserType <> @UserType
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
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

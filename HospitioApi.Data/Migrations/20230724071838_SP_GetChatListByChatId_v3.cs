using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetChatListByChatId_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetChatListByChatId]
(
	@ChatId INT = 0,
	@Id INT = 0,
	@Type VARCHAR(20) = ''
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

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

	DECLARE @UserId INT
	DECLARE @UserType VARCHAR(50)

	SELECT @UserType = [CU].[UserType], @UserId = [CU].[UserId] 
	FROM ChannelUsers CU
	WHERE CU.ChannelId = @ChatId AND CU.DeletedAt IS NULL 
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
		SELECT [C].[Id] AS [UserId],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS LastMessage,CU1.LastMessageReadTime AS LastMessageTime,'' AS [FirstName],
		'' AS [LastName],[C].[BusinessName],[ProfilePicture],[CU1].[IsActive],
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
	END
	ELSE IF(@UserType = 'HospitioUser')
	BEGIN
		SELECT [US].[Id] AS [UserId],[CU1].[ChannelId] AS [ChatId],CM.[Message] AS [LastMessage],[CU1].[LastMessageReadTime] AS [LastMessageTime] ,[FirstName],
						 [LastName],'' AS [BusinessName],[ProfilePicture], [CU1].[IsActive],
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
	END
	ELSE IF(@UserType = 'CustomerGuest')
	BEGIN
		SELECT [CG].[Id],[CU1].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CU1].[LastMessageReadTime] AS [LastMessageTime],[CG].[Firstname] AS [FirstName],[CG].[Lastname] AS [LastName],
				[CG].[Picture],[CU1].[IsActive],
				(SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] FROM [dbo].[ChannelMessages] CM 
				WHERE  [CM].[ChannelId] = [CU1].[ChannelId]
				AND [CM].[Id] > ISNULL([CU2].[LastMessageReadId],0)
				AND (
					(
							CM.MessageSender = @ChatUserType
							AND CM.MessageSenderId <> @UserId
					)
					OR (
							CM.MessageSender <> @ChatUserType
					)
				)
			) AS [TotalUnReadCount],'CustomerGuest' AS [UserType],
			CASE WHEN (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
            AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE())) THEN 'In-House'
			WHEN (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
            AND CONVERT(DATE, CR.CheckoutDate) < CONVERT(DATE, GETDATE())) THEN 'Checked-out'
			END AS [Status]
		FROM [dbo].[ChannelUsers] CU1
		INNER JOIN [dbo].[ChannelUsers] CU2 ON CU1.channelId = CU2.channelId
		INNER JOIN [dbo].[CustomerGuests] CG ON CU1.UserId = CG.Id
		INNER JOIN [dbo].[CustomerReservations] CR ON CG.CustomerReservationId = CR.Id 
		LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] 
		WHERE cu2.UserId = @UserId  
			AND cu2.UserType = @UserType
			AND cu1.UserType = 'CustomerGuest'
			AND ISNULL(CU1.[ChannelId],'') != ''
			AND CR.CustomerId = @UserId
			AND [CM].[DeletedAt] IS NULL
			AND CU1.ChannelId IN (SELECT [ChannelId] FROM [dbo].[ChannelUsers](NOLOCK) WHERE [UserId] = @UserId AND DeletedAt IS NULL)
		AND ISNULL([CM].[Id],'') = ISNULL((SELECT TOP(1) CMS.Id FROM [dbo].[ChannelMessages] CMS (NOLOCK) WHERE [CU1].[ChannelId] = [CMS].[ChannelId] ORDER BY CMS.Id DESC),'')
				
	END
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

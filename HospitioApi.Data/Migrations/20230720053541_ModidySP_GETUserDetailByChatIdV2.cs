using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModidySP_GETUserDetailByChatIdV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //update SP_GETUserDetailByChatId
            migrationBuilder.Sql(@"CREATE OR ALTER     PROCEDURE [dbo].[SP_GETUserDetailByChatId]
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

	SELECT @UserType = [CU].[UserType], @UserId = [CU].[UserId] FROM ChannelUsers (NOLOCK) CU
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
		SELECT [C].[BusinessName],NULL AS[FirstName],NULL AS [LastName],[C].[Email],NULL AS [ProfilePicture],[C].[PhoneCountry],[C].[PhoneNumber],
			   [C].[IncomingTranslationLangage],[C].[NoOfRooms],[BT].[BizType],[P].[Name] AS [ServicePackageName],[C].[CreatedAt],
			   @UserType AS [UserType] ,@UserId AS [UserId], @ChatId AS [ChatId],[C].[IsActive],[C].[DeActivated]
		FROM [dbo].[Customers](NOLOCK) C 
			LEFT JOIN [dbo].[BusinessTypes] BT (NOLOCK) 
				ON [BT].[Id] = [C].[BusinessTypeId] 
					AND [BT].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[Products] P (NOLOCK) 
				ON [P].[Id] = [C].[ProductId] 
					AND  [P].[DeletedAt] IS NULL
		WHERE [C].[Id] = @UserId
			AND [C].[DeletedAt] IS NULL
	END
	ELSE IF(@UserType = 'HospitioUser')
	BEGIN
		SELECT	NULL AS [BusinessName],[U].[FirstName],[U].[LastName],[U].[Email],[U].[ProfilePicture],[U].[PhoneCountry],[U].[PhoneNumber],
				NULL AS [IncomingTranslationLangage],NULL AS [NoOfRooms],NULL AS [BizType],NULL AS [ServicePackageName],[U].[CreatedAt],
				@UserType AS [UserType] ,@UserId AS [UserId], @ChatId AS [ChatId],[U].[IsActive],[U].[DeActivated]
			FROM [dbo].[Users] U (NOLOCK)
			WHERE [U].[DeletedAt] IS NULL
				AND [U].[Id] = @UserId
	END
	ELSE IF(@UserType = 'CustomerGuest')
	BEGIN
		SELECT	NULL AS [BusinessName],[CG].[Firstname] AS [FirstName],[CG].[Lastname] AS [LastName],[CG].[Email],[CG].[Picture] AS [ProfilePicture],[CG].[PhoneCountry],[CG].[PhoneNumber],
				NULL AS [IncomingTranslationLangage],[CG].[RoomNumber] AS [NoOfRooms],NULL AS [BizType],NULL AS [ServicePackageName],[CR].[CreatedAt],
				@UserType AS [UserType] ,@UserId AS [UserId], @ChatId AS [ChatId],[CG].[IsActive],[C].[DeActivated],
				CASE 
					WHEN (CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, [CR].[CheckoutDate]) >= CONVERT(DATE, GETDATE())) 
						THEN 'In-House'
					WHEN (CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, [CR].[CheckoutDate]) < CONVERT(DATE, GETDATE())) 
						THEN 'Checked-out'
				END AS [Status]
        FROM [dbo].[CustomerGuests](NOLOCK) CG
            INNER JOIN [dbo].[CustomerReservations](NOLOCK) CR ON [CR].[Id] = [CG].[CustomerReservationId] AND [CR].[DeletedAt] IS NULL
			INNER JOIN [Customers](NOLOCK) C ON [C].[Id] = [CR].[CustomerId] AND [C].DeletedAt IS NULL
        WHERE [CG].[DeletedAt] IS NULL
			AND [CG].[Id] = @UserId
	END
END
");

            //update SP_GetChatListByChatId
            migrationBuilder.Sql(@"CREATE OR ALTER     PROCEDURE [dbo].[SP_GetChatListByChatId]
(
	@ChatId INT = 0,
	@Id INT = 0,
	@Type VARCHAR(20) = ''
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

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
		SELECT [CU].[Id] AS [UserId],[CT].[ChannelId] AS [ChatId],[CM].[Message] AS LastMessage,CT.LastMessageReadTime AS LastMessageTime,'' AS [FirstName],
		'' AS [LastName],[C].[BusinessName],[CU].[ProfilePicture],[CT].[IsActive],
		(SELECT COUNT([C].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
			INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
			INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
		) AS [TotalUnReadCount],[CT].[UserType]
		FROM [dbo].[Customers](NOLOCK) C 
			LEFT JOIN [dbo].[CustomerUsers] CU ON [CU].[CustomerId] = [C].[Id] AND [CU].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [CU].[Id] AND [CT].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CT].[ChannelId] AND [CM].[MessageSenderId] = @UserId  AND [CM].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[Products] P (NOLOCK) ON [P].[Id] = [C].[ProductId] AND  [P].[DeletedAt] IS NULL
		WHERE [CU].[Id] = @UserId
			AND [C].[DeletedAt] IS NULL
			AND [CT].[ChannelId] = @ChatId
			AND (
				(
						CT.UserType = @Type
						AND CT.UserId <> @Id
				)
				OR (
						CT.UserType <> @Type
				)
			)
	END
	ELSE IF(@UserType = 'HospitioUser')
	BEGIN
		SELECT [U].[Id] AS [UserId],[CT].[ChannelId] AS [ChatId],CM.[Message] AS [LastMessage],[CT].[LastMessageReadTime] AS [LastMessageTime] ,[FirstName],
						 [LastName],'' AS [BusinessName],[ProfilePicture], [CT].[IsActive],
			(SELECT COUNT([C].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
				INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
				INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
			) AS [TotalUnReadCount],[CT].[UserType]
			FROM [dbo].[Users] U (NOLOCK)
				LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [U].[Id] AND [CT].[DeletedAt] IS NULL
				LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CT].[ChannelId] AND [CM].[MessageSenderId] = @UserId  AND [CM].[DeletedAt] IS NULL
			WHERE [U].[DeletedAt] IS NULL
				AND [U].[Id] = @UserId
				AND [CT].[ChannelId] = @ChatId
				AND (
					(
							CT.UserType = @Type
							AND CT.UserId <> @Id
					)
					OR (
							CT.UserType <> @Type
					)
				)
				
	END
	ELSE IF(@UserType = 'CustomerGuest')
	BEGIN
		SELECT [CG].[Id],[CT].[ChannelId] AS [ChatId],[CM].[Message] AS [LastMessage],[CT].[LastMessageReadTime] AS [LastMessageTime],[CG].[Firstname] AS [FirstName],[CG].[Lastname] AS [LastName],
				[CG].[Picture],[CT].[IsActive],
				(SELECT COUNT([CM].[Id]) AS [TotalUnreadCount] FROM [dbo].[Channels] C (NOLOCK) 
					INNER JOIN [dbo].[ChannelUsers] CU ON [C].[Id] = [CU].[ChannelId] AND [CU].[UserId] = @UserId
					INNER JOIN [dbo].[ChannelMessages] CM ON  [CM].[ChannelId] = [C].[Id] AND [CM].[MessageSenderId] <> @UserId AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 
				) AS [TotalUnReadCount],'CustomerGuest' AS [UserType],
			CASE WHEN (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
            AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE())) THEN 'In-House'
			WHEN (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
            AND CONVERT(DATE, CR.CheckoutDate) < CONVERT(DATE, GETDATE())) THEN 'Checked-out'
			END AS [Status]
		FROM [dbo].[CustomerReservations] CR
			LEFT JOIN [dbo].[CustomerGuests] CG  ON CG.CustomerReservationId = CR.Id 
			LEFT JOIN [dbo].[ChannelUsers] CT ON [CT].[UserId] = [CG].[Id]
			LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CT].[ChannelId] 
		WHERE ISNULL([CT].[ChannelId],'') != '' 
			AND [CM].[DeletedAt] IS NULL
			AND [CT].[ChannelId] = @ChatId
				AND (
					(
							CT.UserType = @Type
							AND CT.UserId <> @Id
					)
					OR (
							CT.UserType <> @Type
					)
				)
				
	END
END
");

            //update SP_GetTotalUnReadMessageCount

            migrationBuilder.Sql(@"CREATE OR ALTER    PROCEDURE [dbo].[SP_GetTotalUnReadMessageCount]
(
	 @UserId INT = 0,
	 @UserType INT = 0
)
AS
BEGIN
	
	SET NOCOUNT ON
    SET XACT_ABORT ON
	
	SELECT COUNT(DISTINCT([C].[Id])) AS [TotalUnreadCount]
	FROM [dbo].[Channels] C (NOLOCK)
		INNER JOIN [dbo].[ChannelUsers] CU 
			ON [C].[Id] = [CU].[ChannelId] 
				AND [CU].[UserId] = @UserId
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
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

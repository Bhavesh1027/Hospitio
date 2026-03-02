using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_SP_GETUserDetailByChatId_SPs_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
CREATE OR ALTER   PROCEDURE [dbo].[SP_GETUserDetailByChatId]
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
	ELSE IF(@UserType = 'ChatWidgetUser')
	BEGIN
		SELECT	NULL AS [BusinessName],
		        'ChatWidgetUser' AS [FirstName],
			    CAST(@Id AS NVARCHAR(255)) AS [LastName],
				NULL AS [Email],
			    NULL AS [ProfilePicture],
				NULL AS [PhoneCountry],
				NULL AS [PhoneNumber],
				NULL AS [IncomingTranslationLangage],
				NULL AS [NoOfRooms],
				NULL AS [BizType],
				NULL AS [ServicePackageName],
				[CW].[CreatedAt],
				@UserType AS [UserType] ,
				@UserId AS [UserId], 
				@ChatId AS [ChatId],
				0 AS [IsActive],
				NULL AS [DeActivated], 
				NULL AS [Status],
				NUll AS [CheckinDate], 
				NULL AS [CheckoutDate],
				NULL AS [BlePinCode]
			FROM [dbo].[ChatWidgetUsers] CW (NOLOCK)
			WHERE [CW].[DeletedAt] IS NULL
				AND [CW].[Id] = @UserId
	END
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GETUserDetailByChatId_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GETUserDetailByChatId]
(
	@ChatId INT = 0,
	@Id INT = 0
)
AS
BEGIN
	SET NOCOUNT ON;
    SET XACT_ABORT ON

	DECLARE @UserType NVARCHAR(100) 
	DECLARE @UserId INT

	SELECT @UserType = CU.[UserType],@UserId = [CU].[UserId] FROM [dbo].[ChannelUsers] (NOLOCK) CU
	WHERE [CU].[ChannelId] = @ChatId
	AND [CU].[UserId] <> @Id

	PRINT @UserType
	PRINT @UserId

	IF(@UserType = 'CustomerUser')
	BEGIN
		SELECT [C].[BusinessName],NULL AS[FirstName],NULL AS [LastName],[C].[Email],NULL AS [ProfilePicture],[C].[PhoneCountry],[C].[PhoneNumber],
			   [C].[IncomingTranslationLangage],[C].[NoOfRooms],[BT].[BizType],[P].[Name] AS [ServicePackageName],[C].[CreatedAt],
			   @UserType AS [UserType] ,@UserId AS [UserId], @ChatId AS [ChatId],[C].[IsActive],[C].[DeActivated],'' AS [Status]
		FROM [dbo].[Customers](NOLOCK) C 
			INNER JOIN [dbo].[CustomerUsers] CU (NOLOCK) ON CU.[CustomerId] = [C].[Id] AND [CU].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[BusinessTypes] BT (NOLOCK) ON [BT].[Id] = [C].[BusinessTypeId] AND [BT].[DeletedAt] IS NULL
			LEFT JOIN [dbo].[Products] P (NOLOCK) ON [P].[Id] = [C].[ProductId] AND  [P].[DeletedAt] IS NULL
		WHERE [CU].[Id] = @UserId
			AND [C].[DeletedAt] IS NULL
	END
	ELSE IF(@UserType = 'HospitioUser')
	BEGIN
		SELECT	NULL AS [BusinessName],[U].[FirstName],[U].[LastName],[U].[Email],[U].[ProfilePicture],[U].[PhoneCountry],[U].[PhoneNumber],
				NULL AS [IncomingTranslationLangage],NULL AS [NoOfRooms],NULL AS [BizType],NULL AS [ServicePackageName],[U].[CreatedAt],
				@UserType AS [UserType] ,@UserId AS [UserId], @ChatId AS [ChatId],[U].[IsActive],[U].[DeActivated],'' AS [Status]
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
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

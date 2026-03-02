using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetCustomerGuestDetailByChatId_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetCustomerGuestDetailByChatId]
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

	SELECT @UserType = [CU].[UserType],@UserId = [CU].[UserId] FROM [dbo].[ChannelUsers] (NOLOCK) CU
	WHERE [CU].[ChannelId] = @ChatId
	AND [CU].[UserId] <> @Id
	AND [CU].[DeletedAt] IS NULL

	IF(@UserType = 'CustomerGuest')
	BEGIN
		SELECT	[CG].[Firstname] AS [FirstName],[CG].[Lastname] AS [LastName],[CG].[Email],[CG].[Picture] AS [ProfilePicture],[CG].[PhoneCountry],[CG].[PhoneNumber],
				@ChatId AS [ChatId],[CG].[Country],[CG].[Language],[CG].[RoomNumber],[CR].[CheckinDate],[CR].[CheckoutDate],'CustomerGuest' AS [UserType],
				CASE 
					WHEN (CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, [CR].[CheckoutDate]) >= CONVERT(DATE, GETDATE())) 
						THEN 'In-House'
					WHEN (CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, [CR].[CheckoutDate]) < CONVERT(DATE, GETDATE())) 
						THEN 'Checked-out'
				END AS [Status],[C].[DeActivated],[C].[IncomingTranslationLangage],[C].[CreatedAt],@UserId AS [UserId],[CG].[IsActive]
        FROM [dbo].[CustomerGuests](NOLOCK) CG
            INNER JOIN [dbo].[CustomerReservations](NOLOCK) CR
                ON [CR].[Id] = [CG].[CustomerReservationId] 
				AND [CR].[DeletedAt] IS NULL
			INNER JOIN [Customers](NOLOCK) C 
				ON [C].[Id] = [CR].[CustomerId] 
				AND C.DeletedAt IS NULL
        WHERE [CG].[DeletedAt] IS NULL
			AND [CG].[Id] = @UserId
	END
	ELSE IF(@UserType = 'HospitioUser')
	BEGIN
		SELECT	[U].[FirstName],[U].[LastName],[U].[Email],[U].[ProfilePicture],[U].[PhoneCountry],[U].[PhoneNumber],@ChatId AS [ChatId],NULL AS [Country],
		NULL AS [Language],NULL AS [RoomNumber],NULL AS [CheckinDate], NULL AS [CheckoutDate],'HospitioUser' AS [UserType],'' AS [Status],[U].[DeActivated],
		NULL AS [IncomingTranslationLangage],[U].[CreatedAt],@UserId AS [UserId],[U].[IsActive]
			FROM [dbo].[Users] U (NOLOCK)
			WHERE [U].[DeletedAt] IS NULL
				AND [U].[Id] = @UserId
				AND [U].[UserLevelId] = 1
	END
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

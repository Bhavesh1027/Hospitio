using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetCustomerGuestDetailByChatId_v1 : Migration
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

	IF(@UserType = 'CustomerGuest')
	BEGIN
		SELECT	[CG].[Firstname] AS [FirstName],[CG].[Lastname] AS [LastName],[CG].[Email],[CG].[Picture] AS [ProfilePicture],[CG].[PhoneCountry],[CG].[PhoneNumber],
				[CG].[Country],[CG].[Language],[CG].[RoomNumber],[CR].[CheckinDate],[CR].[CheckoutDate],'CustomerGuest' AS [UserType],
				CASE WHEN (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE())) 
						THEN 'In-House'
					WHEN (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, CR.CheckoutDate) < CONVERT(DATE, GETDATE())) 
						THEN 'Checked-out'
				END AS [Status]
        FROM [dbo].[CustomerGuests](NOLOCK) CG
            INNER JOIN [dbo].[CustomerReservations](NOLOCK) CR
                ON [CR].[Id] = [CG].[CustomerReservationId] 
				AND [CR].[DeletedAt] IS NULL
        WHERE [CG].[DeletedAt] IS NULL
			AND [CG].[Id] = @UserId
	END
	ELSE IF(@UserType = 'HospitioUser')
	BEGIN
		SELECT	[U].[FirstName],[U].[LastName],[U].[Email],[U].[ProfilePicture],[U].[PhoneCountry],[U].[PhoneNumber],
				NULL AS [Country],NULL AS [Language],NULL AS [RoomNumber] ,NULL AS [CheckinDate], NULL AS [CheckoutDate],'HospitioUser' AS [UserType],'' AS [Status]
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

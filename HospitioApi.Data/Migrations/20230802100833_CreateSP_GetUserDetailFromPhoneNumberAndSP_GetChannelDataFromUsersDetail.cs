using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CreateSP_GetUserDetailFromPhoneNumberAndSP_GetChannelDataFromUsersDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[SP_GetUserDetailFromPhoneNumber]
    @PhoneNumber NVARCHAR(50)
AS
BEGIN
	DECLARE @UserId INT = null;
	DECLARE @UserType VARCHAR(20) = '';

    DECLARE @NormalizedPhoneNumber NVARCHAR(50)
    SET @NormalizedPhoneNumber = REPLACE(REPLACE(@PhoneNumber, '+', ''), ' ', '')

    -- Find matching records in the 'users' table
    SELECT TOP 1 @UserId = Id, @UserType = '1'
    FROM HospitioOnboardings
    WHERE REPLACE(REPLACE(WhatsappNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber

    -- Find matching records in the 'customerGuest' table
    IF @UserId IS NULL
    BEGIN
        SELECT TOP 1 @UserId = Id, @UserType = '3'
        FROM CustomerGuests
        WHERE REPLACE(REPLACE(PhoneNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
    END
    -- Find matching records in the 'customers' table
    IF @UserId IS NULL
    BEGIN
        SELECT TOP 1 @UserId = cu.Id, @UserType = '2'
        FROM Customers c
        INNER JOIN CustomerUsers cu ON c.Id = cu.CustomerId
        WHERE REPLACE(REPLACE(c.WhatsappNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
    END

	select @UserId As UserId, @UserType As UserType
END
");


            migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[SP_GetChannelDataFromUsersDetail]
	@UserIdTo INT,
    @UserTypeTo NVARCHAR(50),
    @UserIdFrom INT,
    @UserTypeFrom NVARCHAR(50)
AS
BEGIN


	SET NOCOUNT ON;
	SELECT DISTINCT c.*
    FROM Channels c
    INNER JOIN ChannelUsers cu1 ON c.Id = cu1.ChannelId
    INNER JOIN ChannelUsers cu2 ON c.Id = cu2.ChannelId
    WHERE cu1.UserId = @UserIdTo
        AND cu1.UserType = @UserTypeTo
        AND cu2.UserId = @UserIdFrom
        AND cu2.UserType = @UserTypeFrom;
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

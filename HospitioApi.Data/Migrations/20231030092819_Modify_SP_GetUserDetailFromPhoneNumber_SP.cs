using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_SP_GetUserDetailFromPhoneNumber_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region SP_GetUserDetailFromPhoneNumber
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserDetailFromPhoneNumber]    Script Date: 30/10/2023 1:19:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER    PROCEDURE [dbo].[SP_GetUserDetailFromPhoneNumber]  --'919023728519' , '0'
    @PhoneNumber NVARCHAR(50),
	@AnonymousUsersType NVARCHAR(50) =0
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
	 IF @UserId IS NULL
    BEGIN
	  IF(@AnonymousUsersType = '1')
		  BEGIN
			SELECT TOP 1 @UserId = c.Id, @UserType = '4'
			FROM AnonymousUsers c
			WHERE REPLACE(REPLACE(c.PhoneNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
				  AND c.UserType = 2
		  END
	  ELSE 
		  BEGIN
		  	SELECT TOP 1 @UserId = c.Id, @UserType = '4'
			FROM AnonymousUsers c
			WHERE REPLACE(REPLACE(c.PhoneNumber, '+', ''), ' ', '') = @NormalizedPhoneNumber
				  AND c.UserType = 3
		  END

    END

	select @UserId As UserId, @UserType As UserType
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

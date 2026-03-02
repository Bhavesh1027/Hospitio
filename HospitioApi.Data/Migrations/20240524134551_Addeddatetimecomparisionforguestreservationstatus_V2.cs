using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Addeddatetimecomparisionforguestreservationstatus_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS [GetGuestsStatus]");
            migrationBuilder.Sql(@"
GO
/***** Object:  UserDefinedFunction [dbo].[GetGuestsStatus]    Script Date: 24/05/2024 5:51:09 PM *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   FUNCTION [dbo].[GetGuestsStatus](@CustomerGuestId int)  
RETURNS int   --returns int type value of GuestsStatus ""1. Expected 2. InHouse 3. Checkout""
AS 
BEGIN
                                        
	DECLARE @TimeZone varchar(500);
	DECLARE @CurrentUtcTime datetime;
	DECLARE @CustomerLocalTime datetime;
	DECLARE @Offset varchar(6); -- Adjusted to store offset in +HH:MM or -HH:MM format

	-- Get the current UTC time
	SET @CurrentUtcTime = GETUTCDATE();

	-- Retrieve the customer's time zone
	SELECT @TimeZone = TimeZone 
	FROM dbo.Customers 
	inner join dbo.CustomerReservations on dbo.CustomerReservations.CustomerId = dbo.Customers.Id
	inner join dbo.CustomerGuests on dbo.CustomerGuests.CustomerReservationId = dbo.CustomerReservations.Id
	WHERE dbo.CustomerGuests.Id = @CustomerGuestId;

	-- Extract the offset part from the TimeZone string
	SET @Offset = SUBSTRING(@TimeZone, CHARINDEX('UTC', @TimeZone) + 3, 6);

	-- Convert UTC time to customer's local time
	SET @CustomerLocalTime = SWITCHOFFSET(@CurrentUtcTime, @Offset);


	DECLARE @GuestStatus int = 0;
                                    
	-- retrieves guests status based on reservation checkin and checkout date 
	select 
	@GuestStatus =  
	CASE
	WHEN CONVERT(datetime, CONVERT(varchar, dbo.CustomerReservations.CheckinDate, 120)) > CONVERT(datetime, @CustomerLocalTime, 120) THEN 1
	WHEN CONVERT(datetime, @CustomerLocalTime, 120) BETWEEN
		CONVERT(datetime, CONVERT(varchar, dbo.CustomerReservations.CheckinDate, 120)) AND
		CONVERT(datetime, CONVERT(varchar, dbo.CustomerReservations.CheckoutDate, 120)) THEN 2
	WHEN CONVERT(datetime, CONVERT(varchar, dbo.CustomerReservations.CheckoutDate, 120)) < CONVERT(datetime, @CustomerLocalTime, 120) THEN 3
	End
	from dbo.CustomerGuests
	inner join dbo.CustomerReservations on dbo.CustomerReservations.Id = dbo.CustomerGuests.CustomerReservationId 
	where dbo.CustomerGuests.Id = @CustomerGuestId
                                    
	RETURN @GuestStatus; --returns a value
END
");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

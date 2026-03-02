using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Addeddatetimecomparisionforguestreservationstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS [GetGuestsStatus]");
            migrationBuilder.Sql(@"
GO
/****** Object:  UserDefinedFunction [dbo].[GetGuestsStatus]    Script Date: 07-11-2023 10:41:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                                CREATE   FUNCTION [dbo].[GetGuestsStatus](@CustomerGuestId int)  
                                RETURNS int   --returns int type value of GuestsStatus ""1. Expected 2. InHouse 3. Checkout""
                                    AS 
                                    BEGIN
                                        
                                        DECLARE @GuestStatus int = 0;
                                    
                                        -- retrieves guests status based on reservation checkin and checkout date 
                                        select 
                                		@GuestStatus =  
                                		CASE
                                        WHEN CONVERT(datetime, CONVERT(varchar, dbo.CustomerReservations.CheckinDate, 120)) > CONVERT(datetime, CURRENT_TIMESTAMP, 120) THEN 1
                                       WHEN CONVERT(datetime, CURRENT_TIMESTAMP, 120) BETWEEN
									        CONVERT(datetime, CONVERT(varchar, dbo.CustomerReservations.CheckinDate, 120)) AND
											CONVERT(datetime, CONVERT(varchar, dbo.CustomerReservations.CheckoutDate, 120)) THEN 2
                                		WHEN CONVERT(datetime, CONVERT(varchar, dbo.CustomerReservations.CheckoutDate, 120)) < CONVERT(datetime, CURRENT_TIMESTAMP, 120) THEN 3
                                		End
                                		from dbo.CustomerGuests
                                		inner join dbo.CustomerReservations on dbo.CustomerReservations.Id = dbo.CustomerGuests.CustomerReservationId 
                                		where dbo.CustomerGuests.Id = @CustomerGuestId
                                    
                                        RETURN @GuestStatus; --returns a value
                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

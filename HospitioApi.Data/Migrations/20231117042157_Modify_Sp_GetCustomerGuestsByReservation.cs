using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_Sp_GetCustomerGuestsByReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/***** Object:  StoredProcedure [dbo].[GetCustomerGuestsByReservation]    Script Date: 11/16/2023 8:00:21 PM *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerGuestsByReservation]
(
	@ReservationId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerReservationId],
           [Firstname],
           [Lastname],
           [Email],
           [Picture],
           [PhoneCountry],
           [PhoneNumber],
           [AgeCategory],
		   [isCheckInCompleted],
           [IsCoGuest]
    FROM  [dbo].[CustomerGuests] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [CustomerReservationId] = @ReservationId
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

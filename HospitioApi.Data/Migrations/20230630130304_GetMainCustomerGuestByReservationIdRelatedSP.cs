using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetMainCustomerGuestByReservationIdRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetMainCustomerGuestByReservationId]    Script Date: 30-06-2023 18:31:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROCEDURE [dbo].[GetMainCustomerGuestByReservationId] -- 1
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
           [Country],
           [Language],
           [IdProof],
           [IdProofType],
           [IdProofNumber],
           [BlePinCode],
           [Pin],
           [Street],
           [StreetNumber],
           [City],
           [Postalcode],
           [ArrivalFlightNumber],
           [DepartureAirline],
           [DepartureFlightNumber],
           [Signature],
           [RoomNumber],
           [TermsAccepted],
           [FirstJourneyStep],
           [Rating],
           [IsActive],
           [CreatedAt],
           [UpdateAt],
           [DeletedAt],
           [CreatedBy],
           [DateOfBirth],
           [BookingChannel],
           [DepartingFlightDate],
		   [Vat]
    FROM [dbo].[CustomerGuests] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [CustomerReservationId] = @ReservationId AND IsCoGuest = 0
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

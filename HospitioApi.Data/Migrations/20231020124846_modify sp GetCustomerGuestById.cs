using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class modifyspGetCustomerGuestById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"/****** Object:  StoredProcedure [dbo].[GetCustomerGuestById]    Script Date: 20-10-2023 18:15:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER         PROCEDURE [dbo].[GetCustomerGuestById] -- 1
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT G.[Id],
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
           G.[IsActive],
           G.[CreatedAt],
           G.[UpdateAt],
           G.[DeletedAt],
           G.[CreatedBy],
		   G.isCheckInCompleted,
		   G.isSkipCheckIn,
           [DateOfBirth],
           [BookingChannel],
           [DepartingFlightDate],
		   [Vat],
		   [AgeCategory],
		   R.[CheckinDate],
		   R.[CheckoutDate],
		   R.[ReservationNumber]
    FROM [dbo].[CustomerGuests] G (NOLOCK)
	INNER JOIN CustomerReservations R ON CustomerReservationId = R.Id
    WHERE G.[DeletedAt] IS NULL
          AND G.[Id] = @Id
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

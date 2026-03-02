using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddCustomerGuestRelatedSpV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuestById]    Script Date: 22-06-2023 13:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROCEDURE [dbo].[GetCustomerGuestById] -- 1
(
	@Id INT = 0
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
		   [Vat],
		   [AgeCategory]
    FROM [dbo].[CustomerGuests] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

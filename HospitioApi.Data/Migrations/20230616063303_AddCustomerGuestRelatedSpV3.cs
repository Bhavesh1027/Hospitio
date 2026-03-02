using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddCustomerGuestRelatedSpV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuestById]    Script Date: 16-06-2023 11:55:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerGuestById] 
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

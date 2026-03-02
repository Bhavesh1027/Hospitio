using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddCustomerGuestRelatedSpV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuestById]    Script Date: 13-06-2023 11:56:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

    CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerGuestById] -- 1
    @Id int = 0
    AS
    BEGIN
        SELECT [Id],[CustomerReservationId],[Firstname],[Lastname],[Email],[Picture],[PhoneCountry],[PhoneNumber],[Country],[Language],[IdProof],[IdProofType],[IdProofNumber],[BlePinCode],[Pin],[Street],[StreetNumber],[City],[Postalcode],[ArrivalFlightNumber],[DepartureAirline],[DepartureFlightNumber],[Signature],[RoomNumber],[TermsAccepted],[FirstJourneyStep],[Rating],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy],[DateOfBirth],[BookingChannel],[DepartingFlightDate]
		FROM [dbo].[CustomerGuests]
        where Id = @Id and DeletedAt is null
    END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

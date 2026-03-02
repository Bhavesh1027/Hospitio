using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerReservationByNumberRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerReservationByNumber]    Script Date: 30-06-2023 18:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerReservationByNumber] --  'KOJ'
(
	@ReservationNumber NVARCHAR(100) = NULL
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerId],
           [Uuid],
           [ReservationNumber],
           [Source],
           [NoOfGuestAdults],
           [NoOfGuestChildrens],
           [CheckinDate],
           [CheckoutDate],
           [IsActive]
    FROM [dbo].[CustomerReservations] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [ReservationNumber] = @ReservationNumber
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

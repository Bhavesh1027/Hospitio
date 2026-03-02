using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerGuestsByReservationRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuestsByReservation]    Script Date: 12-06-2023 12:36:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   Procedure [dbo].[GetCustomerGuestsByReservation]
(
@ReservationId int = 0
)
as
SELECT
       [Id]
      ,[CustomerReservationId]
      ,[Firstname]
      ,[Lastname]
      ,[Email]
      ,[Picture]
      ,[PhoneCountry]
      ,[PhoneNumber]
      ,[AgeCategory]
      ,[IsCoGuest]
  FROM [dbo].[CustomerGuests]
  Where CustomerReservationId = @ReservationId and DeletedAt is null
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

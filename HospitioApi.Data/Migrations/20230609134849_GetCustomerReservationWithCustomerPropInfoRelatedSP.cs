using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerReservationWithCustomerPropInfoRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerReservationWithCustomerPropInfo]    Script Date: 09-06-2023 19:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER       Procedure [dbo].[GetCustomerReservationWithCustomerPropInfo] 
  @GuestId Int=1
 as
 
SELECT 
( 
SELECT g.[IsCoGuest],JSON_QUERY(( 
SELECT [Id]
      ,[CustomerId]
      ,[Uuid]
      ,[ReservationNumber]
      ,[Source]
      ,[NoOfGuestAdults]
      ,[NoOfGuestChildrens]
      ,[CheckinDate]
      ,[CheckoutDate]
	  ,[isCheckInCompleted]
	  ,[isSkipCheckIn]
      ,[IsActive]
	  ,JSON_QUERY(( 
SELECT [Id]
      ,[CustomerId]
      ,[Color]
      ,[Name]
      ,[Stars]
      ,[Logo]
      ,[AppImage]
      ,[SplashScreen]
      ,[IsOnlineCheckInFormEnable]
      ,[IsRedirectToGuestAppEnable]
      ,[SubmissionMail]
      ,[TermsLink]
      ,[IsActive]
FROM CustomerGuestsCheckInFormBuilders ff where fb.CustomerId = ff.CustomerId
FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
)) as [GetCustomerGuestsCheckInFormBuilderResponseOut]
FROM CustomerReservations fb where fb.Id=g.CustomerReservationId and fb.DeletedAt is null
FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
)) as [GetCustomerReservationResponseOut]
FROM CustomerGuests g where g.Id = @GuestId and g.DeletedAt is null
FOR JSON PATH )
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

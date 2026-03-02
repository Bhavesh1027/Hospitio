using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerReservationWithCustomerPropInfoRelatedSPV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerReservationWithCustomerPropInfo]    Script Date: 12-06-2023 11:03:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER       Procedure [dbo].[GetCustomerReservationWithCustomerPropInfo] 
  @GuestId Int=1,
  @ReservationId Int =1
 as
 
SELECT 
( 
SELECT g.[IsCoGuest],ISNULL(g.[Firstname],'') + ' ' + ISNULL(g.[Lastname],'') AS Name,JSON_QUERY(( 
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
SELECT ff.[Id]
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
      ,ff.[IsActive]
	  ,bt.[BizType] AS [BusinessType]
FROM CustomerGuestsCheckInFormBuilders ff join Customers c ON ff.[CustomerId] = c.[Id]
JOIN [dbo].[BusinessTypes] bt ON c.[BusinessTypeId] = bt.[Id]
where fb.CustomerId = ff.CustomerId
FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
)) as [GetCustomerGuestsCheckInFormBuilderResponseOut]
FROM CustomerReservations fb where fb.Id=@ReservationId and fb.DeletedAt is null
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

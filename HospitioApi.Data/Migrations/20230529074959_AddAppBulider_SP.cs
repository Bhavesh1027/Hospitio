using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddAppBulider_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region Add APP Builder SP For Null Data and Get Data

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[AddAppBulider]    Script Date: 29-05-2023 13:20:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[AddAppBulider]
(
	@RoomId int = 0,
	@CustomerId int  = 0 
)
AS BEGIN
	SET NOCOUNT ON;
	Declare @RoomNameId int
	Declare @CustomerAppBuliderId int

	Select @RoomNameId = COUNT(CustomerRoomNameId)
	From CustomerGuestAppBuilders With (NOLOCK)
	Where CustomerId = @CustomerId And
		CustomerRoomNameId = @RoomId
	IF(@RoomNameId = 0)
	BEGIN 
		INSERT INTO CustomerGuestAppBuilders (
	   [CustomerId]
      ,[CustomerRoomNameId]
      ,[Message]
      ,[SecondaryMessage]
      ,[LocalExperience]
      ,[Ekey]
      ,[PropertyInfo]
      ,[EnhanceYourStay]
      ,[Reception]
      ,[Housekeeping]
      ,[RoomService]
      ,[Concierge]
      ,[TransferServices])
	   Values
	   (@CustomerId , @RoomId,null,null,0,0,0,0,0,0,0,0,0)
	End

	BEGIN
		Select  
	   [CustomerRoomNameId]
      ,[Message]
      ,[SecondaryMessage]
      ,[LocalExperience]
      ,[Ekey]
      ,[PropertyInfo]
      ,[EnhanceYourStay]
      ,[Reception]
      ,[Housekeeping]
      ,[RoomService]
      ,[Concierge]
      ,[TransferServices] 
	  From CustomerGuestAppBuilders With (NOLOCK)
	  Where CustomerRoomNameId = @RoomId
	  AND CustomerId = @CustomerId
	END

END");
            #endregion

            #region Add Property Info SP for Add Null data and Get Data

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[AddPropertyInfo]    Script Date: 29-05-2023 13:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[AddPropertyInfo]
(
	@AppBuilderId int = 0,
	@CustomerId int  = 0 
)
AS BEGIN
	SET NOCOUNT ON;
	Declare @CustomerAppBuliderId int

	Select @CustomerAppBuliderId = COUNT(CustomerGuestAppBuilderId)
	From CustomerPropertyInformations With (NOLOCK)
	Where CustomerId = @CustomerId And
		CustomerGuestAppBuilderId = @AppBuilderId
	IF(@CustomerAppBuliderId = 0)
	BEGIN 
		INSERT INTO CustomerPropertyInformations (
	   [CustomerId]
      ,[CustomerGuestAppBuilderId]
      ,[WifiUsername]
      ,[WifiPassword]
      ,[Overview]
      ,[CheckInPolicy]
      ,[TermsAndConditions]
      ,[Street]
      ,[StreetNumber]
      ,[City]
      ,[Postalcode]
      ,[Country])
	   Values
	   (@CustomerId ,@AppBuilderId ,null,null,null,null,null,null,null,null,null,null)
	End

	BEGIN
		Select  
	   [CustomerGuestAppBuilderId]
      ,[WifiUsername]
      ,[WifiPassword]
      ,[Overview]
      ,[CheckInPolicy]
      ,[TermsAndConditions]
      ,[Street]
      ,[StreetNumber]
      ,[City]
      ,[Postalcode]
      ,[Country]
	  From CustomerPropertyInformations With (NOLOCK)
	  Where CustomerGuestAppBuilderId = @AppBuilderId
	  AND CustomerId = @CustomerId
	END

END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

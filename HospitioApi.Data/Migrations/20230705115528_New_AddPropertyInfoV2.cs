using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class New_AddPropertyInfoV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[AddPropertyInfo]    Script Date: 05-07-2023 15:16:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER    PROCEDURE [dbo].[AddPropertyInfo] --  1,1
(
	@AppBuilderId int = 0,
	@CustomerId int  = 0 
)
AS BEGIN
	SET NOCOUNT ON;
	Declare @CustomerAppBuliderId int
	Declare @DisplayOrder int 
	Declare @PropertyId int
	DECLARE @RoomName NVARCHAR(100)

	Select @CustomerAppBuliderId = COUNT(CustomerGuestAppBuilderId)
	From CustomerPropertyInformations With (NOLOCK)
	Where CustomerId = @CustomerId And
		CustomerGuestAppBuilderId = @AppBuilderId and DeletedAt is null

	SELECT @RoomName = crn.Name
	FROM CustomerGuestAppBuilders cga
	JOIN CustomerRoomNames crn ON cga.CustomerRoomNameId = crn.Id
	WHERE cga.Id = @AppBuilderId

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

	Select @PropertyId=Id 
	From CustomerPropertyInformations With (NOLOCK)
	Where CustomerId = @CustomerId And
		CustomerGuestAppBuilderId = @AppBuilderId

	Select @DisplayOrder = COUNT(RefrenceId)
	From ScreenDisplayOrderAndStatuses With (NOLOCK)
	where RefrenceId = @CustomerAppBuliderId
	AND ScreenName = 2

	IF(@DisplayOrder = 0)
	BEGIN
		INSERT INTO dbo.ScreenDisplayOrderAndStatuses(ScreenName,JsonData,RefrenceId,IsActive)
		Values(1,'{
    ""Address"": {
      ""IsActive"": false
    },
    ""Wifi"": {
      ""IsActive"": false
    },
    ""Service"": {
      ""IsActive"": false
    },
    ""Overview"": {
      ""IsActive"": false
    },
    ""Gallary"": {
      ""IsActive"": false
    },
    ""EmergencyNumber"": {
      ""IsActive"": false,
      ""Names"": [
        {
          ""Id"": 0,
          ""Name"": """"
        }
      ]
    },
    ""RecommendedforYou"": {
      ""IsActive"": false,
      ""Names"": [
        {
          ""Id"": 0,
          ""Name"": """"
        }
      ]
    },
    ""AroundYou"": {
      ""IsActive"": false,
      ""Names"": [
        {
          ""Id"": 0,
          ""Name"": """"
        }
      ]
    },
    ""Checkin&CheckoutPolicies"": {
      ""IsActive"": false
    },
    ""Terms&Conditions"": {
      ""IsActive"": false
    }
  }',@PropertyId,1)
	END

	BEGIN
		 SELECT(
		 SELECT
			CI.Id
		   ,CI.[CustomerGuestAppBuilderId]
		  ,CI.[WifiUsername]
		  ,CI.[WifiPassword]
		  ,CI.[Overview]
		  ,CI.[CheckInPolicy]
		  ,CI.[TermsAndConditions]
		  ,CI.[Street]
		  ,CI.[StreetNumber]
		  ,CI.[City]
		  ,CI.[Postalcode]
		  ,CI.[Country]
		  ,@RoomName AS RoomName
		,JSON_QUERY(( 
		SELECT CS.[Id]
			  ,CS.[CustomerPropertyInformationId]
			  ,CS.[Name]
			  ,CS.[Icon]
			  ,CS.[Description]
			  ,JSON_QUERY((
			  Select  CSI.[Id]
					,CSI.[CustomerPropertyServiceId]
					,CSI.[ServiceImages]
			  From CustomerPropertyServiceImages CSI With (NOLOCK)
			  WHERE CSI.CustomerPropertyServiceId = CS.Id and DeletedAt is null
			  FOR JSON PATH
			  )) as [CustomerPropertyInfoServiceImagesOuts]
		FROM CustomerPropertyServices CS With (NOLOCK) where CustomerPropertyInformationId=CI.Id and DeletedAt is null
		FOR JSON PATH
		)) as [CustomerPropertyInfoServicesOuts]
		,JSON_QUERY(( 
		SELECT CG.[Id]
			  ,CG.[CustomerPropertyInformationId]
			  ,CG.[PropertyImage]		  
		FROM CustomerPropertyGalleries CG With (NOLOCK) where CG.CustomerPropertyInformationId=CI.Id and DeletedAt is null
		FOR JSON PATH
		)) as [CustomerPropertyInfoGalleriesOuts]
		,JSON_QUERY(( 
		SELECT CEN.[Id]
		  ,CEN.[CustomerPropertyInformationId]
		  ,CEN.[Name]
		  ,CEN.[PhoneCountry]
		  ,CEN.[PhoneNumber]		  
		FROM CustomerPropertyEmergencyNumbers CEN With (NOLOCK) where CEN.CustomerPropertyInformationId=CI.Id and DeletedAt is null
		FOR JSON PATH
		)) as [CustomerPropertyInfoEmergencyNumbersOuts]
		,JSON_QUERY(( 
		SELECT CE.[Id]
		  ,CE.[CustomerPropertyInformationId]
		  ,CE.[ExtraType]
		  ,CE.[Name]
		   ,JSON_QUERY((
			  Select  CED.[Id]
				  ,CED.[CustomerPropertyExtraId]
				  ,CED.[Description]
				  ,CED.[Link]
			  From CustomerPropertyExtraDetails CED With (NOLOCK)
			  WHERE CED.CustomerPropertyExtraId = CE.Id and DeletedAt is null
			  FOR JSON PATH
			  )) as [customerPropertyExtraDetailsOuts]
		FROM CustomerPropertyExtras CE With (NOLOCK) where CE.CustomerPropertyInformationId=CI.Id and DeletedAt is null
		FOR JSON PATH
		)) as [CustomerPropertyInfoExtrasOuts]
		FROM CustomerPropertyInformations CI With (NOLOCK)
		Where CI.CustomerId = @CustomerId
		AND CI.CustomerGuestAppBuilderId = @AppBuilderId
		AND CI.DeletedAt is null
		FOR JSON PATH )
	END
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

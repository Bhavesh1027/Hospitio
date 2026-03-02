using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddPropertyInfo_SP_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region AddPropertyInfo SP
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[AddPropertyInfo]    Script Date: 31-05-2023 17:56:13 ******/
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
			  WHERE CSI.CustomerPropertyServiceId = CS.Id
			  FOR JSON PATH
			  )) as [CustomerPropertyInfoServiceImagesOuts]
		FROM CustomerPropertyServices CS With (NOLOCK) where CustomerPropertyInformationId=CI.Id
		FOR JSON PATH
		)) as [CustomerPropertyInfoServicesOuts]
		,JSON_QUERY(( 
		SELECT CG.[Id]
			  ,CG.[CustomerPropertyInformationId]
			  ,CG.[PropertyImage]		  
		FROM CustomerPropertyGalleries CG With (NOLOCK) where CG.CustomerPropertyInformationId=CI.Id
		FOR JSON PATH
		)) as [CustomerPropertyInfoGalleriesOuts]
		,JSON_QUERY(( 
		SELECT CEN.[Id]
		  ,CEN.[CustomerPropertyInformationId]
		  ,CEN.[Name]
		  ,CEN.[PhoneCountry]
		  ,CEN.[PhoneNumber]		  
		FROM CustomerPropertyEmergencyNumbers CEN With (NOLOCK) where CEN.CustomerPropertyInformationId=CI.Id
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
			  WHERE CED.CustomerPropertyExtraId = CE.Id
			  FOR JSON PATH
			  )) as [CustomerPropertyInfoExtraDetailsOuts]
		FROM CustomerPropertyExtras CE With (NOLOCK) where CE.CustomerPropertyInformationId=CI.Id
		FOR JSON PATH
		)) as [CustomerPropertyInfoExtrasOuts]
		FROM CustomerPropertyInformations CI With (NOLOCK)
		Where CI.CustomerId = @CustomerId 
		AND CI.CustomerGuestAppBuilderId = @AppBuilderId
		AND CI.DeletedAt is null
		FOR JSON PATH )
	END

END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

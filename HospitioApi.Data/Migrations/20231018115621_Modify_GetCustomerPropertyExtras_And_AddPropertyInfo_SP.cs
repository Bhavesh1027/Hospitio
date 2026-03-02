using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_GetCustomerPropertyExtras_And_AddPropertyInfo_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerPropertyExtras
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPropertyExtras]    Script Date: 18/10/2023 4:29:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPropertyExtras]
(
	@CustomerPropertyInformationId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @ISDELETE BIT = 0;

	WITH Property_Extra AS (
		SELECT [Id],
			[CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.ExtraType') AS [ExtraType],
			JSON_VALUE(JsonData, '$.Name') AS [Name],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted],
			JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder]

		FROM CustomerPropertyExtras CE 
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			

		UNION ALL

		SELECT CE.[Id]
		  ,CE.[CustomerPropertyInformationId]
		  ,CE.[ExtraType]
		  ,CE.[Name]
		  ,@ISDELETE As [IsDeleted]
		  ,CE.[DisplayOrder]
		FROM CustomerPropertyExtras CE 
		WHERE [DeletedAt] IS NULL
		AND JsonData IS NULL
		
	),
	Property_Extra_Detail AS
    (
		SELECT 
			[Id],
            [CustomerPropertyExtraId],
            JSON_VALUE(JsonData, '$.Description') AS [Description],
            JSON_VALUE(JsonData, '$.Latitude') AS [Latitude],
			JSON_VALUE(JsonData, '$.Longitude') AS [Longitude],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyExtraDetails CED
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			

		UNION ALL

		SELECT  
			CED.[Id],
			CED.[CustomerPropertyExtraId],
			CED.[Description],
			CED.[Latitude],
			CED.[Longitude],
			@ISDELETE As [IsDeleted]
		FROM CustomerPropertyExtraDetails CED
		WHERE [DeletedAt] IS NULL
		AND JsonData IS NULL
			
	)

	SELECT 
	(
		SELECT CE.[Id]
		  ,CE.[CustomerPropertyInformationId]
		  ,CE.[ExtraType]
		  ,CE.[Name]
		  ,CE.[IsDeleted]
		  ,CE.[DisplayOrder]
		   ,JSON_QUERY((
			  Select  CED.[Id]
				  ,CED.[CustomerPropertyExtraId]
				  ,CED.[Description]
				  ,CED.[Latitude]
				  ,CED.[Longitude]
				  ,CED.[IsDeleted]
			  From Property_Extra_Detail CED With (NOLOCK)
			  WHERE CED.CustomerPropertyExtraId = CE.Id
			  AND CED.IsDeleted != 1
			  FOR JSON PATH
			  )) as [customerPropertyExtraDetailsOuts]
		FROM Property_Extra CE With (NOLOCK) where CE.CustomerPropertyInformationId = @CustomerPropertyInformationId
		AND CE.IsDeleted != 1
		ORDER BY [CE].[DisplayOrder]
		FOR JSON PATH
	) AS [CustomerPropertyInfoExtrasOuts]
    
END");
            #endregion

            #region AddPropertyInfo
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[AddPropertyInfo]    Script Date: 18/10/2023 5:09:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[AddPropertyInfo]
(
	@AppBuilderId int = 0,
	@CustomerId int  = 0,
	@UserType INT = 0
)
AS BEGIN
	SET NOCOUNT ON;
	Declare @CustomerAppBuliderId int
	Declare @LAT NVARCHAR(Max)
	Declare @LONG NVARCHAR(Max)
	Declare @BizType NVARCHAR(Max)
	Declare @BizTypeId int
	Declare @DisplayOrder int 
	Declare @PropertyId int
	DECLARE @RoomName NVARCHAR(100)
	Declare @ISDELETE BIT = 0;
	Select @LAT = Latitude,@LONG = Longitude,@BizTypeId = BusinessTypeId from Customers Where Id = @CustomerId;
	IF (@BizTypeId IS NOT NULL)
    BEGIN
        Select @BizType = BizType from BusinessTypes Where Id = @BizTypeId
    END
	Select @CustomerAppBuliderId = COUNT(CustomerGuestAppBuilderId)
	From CustomerPropertyInformations With (NOLOCK)
	Where CustomerId = @CustomerId And
		CustomerGuestAppBuilderId = @AppBuilderId and DeletedAt is null

	SELECT @RoomName = crn.Name
	FROM CustomerGuestAppBuilders cga
	JOIN CustomerRoomNames crn ON cga.CustomerRoomNameId = crn.Id
	WHERE cga.Id = @AppBuilderId

	IF (@CustomerAppBuliderId = 0)
    BEGIN
        IF (@BizType = 'Hotel' Or @BizType = 'Hostel')
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
            ,[Country]
            ,[Latitude]
            ,[Longitude])
            Values
            (@CustomerId, @AppBuilderId, null, null, null, null, null, null, null, null, null,null, @LAT, @LONG)
        END
        ELSE
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
            ,[Country]
            ,[Latitude]
            ,[Longitude])
            Values
            (@CustomerId, @AppBuilderId, null, null, null, null, null, null, null, null, null, null, null, null)
        END
    END
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

	WITH Property_Information AS (
		SELECT
			CI.Id,
			CI.CustomerId,
			MAX(CASE WHEN [key] = 'CustomerGuestAppBuilderId' THEN [value] END) AS [CustomerGuestAppBuilderId],
            MAX(CASE WHEN [key] = 'WifiUsername' THEN [value] END) AS [WifiUsername],
            MAX(CASE WHEN [key] = 'WifiPassword' THEN [value] END) AS [WifiPassword],
            MAX(CASE WHEN [key] = 'Overview' THEN [value] END) AS [Overview],
            MAX(CASE WHEN [key] = 'CheckInPolicy' THEN [value] END) AS [CheckInPolicy],
            MAX(CASE WHEN [key] = 'TermsAndConditions' THEN [value] END) AS [TermsAndConditions],
            MAX(CASE WHEN [key] = 'Street' THEN [value] END) AS [Street],
            MAX(CASE WHEN [key] = 'StreetNumber' THEN [value] END) AS [StreetNumber],
            MAX(CASE WHEN [key] = 'City' THEN [value] END) AS [City],
            MAX(CASE WHEN [key] = 'Postalcode' THEN [value] END) AS [Postalcode],
            MAX(CASE WHEN [key] = 'Country' THEN [value] END) AS [Country],
			MAX(CASE WHEN [key] = 'Latitude' THEN [value] END) AS [Latitude],
			MAX(CASE WHEN [key] = 'Longitude' THEN [value] END) AS [Longitude],
			@RoomName AS RoomName,
			@BizType AS BusinessType
		FROM CustomerPropertyInformations CI
		CROSS APPLY OPENJSON(JsonData)
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)
	    GROUP BY CI.Id, CI.CustomerId
		
		UNION ALL
		
		SELECT
			CI.Id,
			CI.CustomerId,
			CI.[CustomerGuestAppBuilderId],
			CI.[WifiUsername],
			CI.[WifiPassword],
			CI.[Overview],
			CI.[CheckInPolicy],
			CI.[TermsAndConditions],
			CI.[Street],
			CI.[StreetNumber],
			CI.[City],
			CI.[Postalcode],
			CI.[Country],
			CI.[Latitude],
			CI.[Longitude],
			@RoomName AS RoomName,
			@BizType AS BusinessType
		FROM CustomerPropertyInformations CI
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	), 
	Property_Extra AS (
		SELECT [Id],
			[CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.ExtraType') AS [ExtraType],
			JSON_VALUE(JsonData, '$.Name') AS [Name],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted],
			JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder]
		FROM CustomerPropertyExtras CE 
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)

		UNION ALL

		SELECT CE.[Id]
		  ,CE.[CustomerPropertyInformationId]
		  ,CE.[ExtraType]
		  ,CE.[Name]
		  ,@ISDELETE As [IsDeleted]
		  ,CE.[DisplayOrder]
		FROM CustomerPropertyExtras CE 
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	),
	Property_Extra_Detail AS
    (
		SELECT 
			[Id],
            [CustomerPropertyExtraId],
            JSON_VALUE(JsonData, '$.Description') AS [Description],
			JSON_VALUE(JsonData, '$.Latitude') AS [Latitude],
            JSON_VALUE(JsonData, '$.Longitude') AS [Longitude],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyExtraDetails CED
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)

		UNION ALL

		SELECT  
			CED.[Id],
			CED.[CustomerPropertyExtraId],
			CED.[Description],
			CED.[Latitude],
			CED.[Longitude],
			@ISDELETE As [IsDeleted]
		FROM CustomerPropertyExtraDetails CED
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	),
	
	Emergency_Numbers AS (
		SELECT 
			[Id],
			[CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.Name') AS [Name],
			JSON_VALUE(JsonData, '$.PhoneCountry') AS [PhoneCountry],
			JSON_VALUE(JsonData, '$.PhoneNumber') AS [PhoneNumber],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted],
			JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder]
		FROM CustomerPropertyEmergencyNumbers CEN
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)
		UNION ALL

		SELECT 
			CEN.[Id],
			CEN.[CustomerPropertyInformationId],
			CEN.[Name],
			CEN.[PhoneCountry],
			CEN.[PhoneNumber],
			@ISDELETE As [IsDeleted],
			CEN.[DisplayOrder]
		FROM CustomerPropertyEmergencyNumbers CEN
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	),
	Property_Gallery AS (
		SELECT 
			[Id],
			[CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.PropertyImage') AS [PropertyImage],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyGalleries CG
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)

		UNION ALL

		SELECT CG.[Id]
			  ,CG.[CustomerPropertyInformationId]
			  ,CG.[PropertyImage],
			  @ISDELETE As [IsDeleted]
		FROM CustomerPropertyGalleries CG
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	),
	Property_Services AS (
		SELECT 
			CS.[Id],
			[CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.Name') AS [Name],
			JSON_VALUE(JsonData, '$.Icon') AS [Icon],
			JSON_VALUE(JsonData, '$.Description') AS [Description],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyServices CS
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)

		UNION ALL

		SELECT 
			CS.[Id],
			CS.[CustomerPropertyInformationId],
			CS.[Name],
			CS.[Icon],
			CS.[Description],
			 @ISDELETE As [IsDeleted]
		FROM CustomerPropertyServices CS
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	),
	Property_Service_Images AS (
		SELECT 
			CSI.[Id],
			[CustomerPropertyServiceId],
			JSON_VALUE(JsonData, '$.ServiceImages') AS [ServiceImages],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyServiceImages CSI
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)

		UNION ALL

		Select  CSI.[Id]
				,CSI.[CustomerPropertyServiceId]
				,CSI.[ServiceImages],
				@ISDELETE As [IsDeleted]
			From CustomerPropertyServiceImages CSI
			WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	)
	

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
		  ,CI.[Latitude]
		  ,CI.[Longitude]
		  ,@RoomName AS RoomName
		  ,@BizType AS BusinessType
		,JSON_QUERY(( 
		SELECT CS.[Id]
			  ,CS.[CustomerPropertyInformationId]
			  ,CS.[Name]
			  ,CS.[Icon]
			  ,CS.[Description]
			  ,CS.[IsDeleted]
			  ,JSON_QUERY((
			  Select  CSI.[Id]
					,CSI.[CustomerPropertyServiceId]
					,CSI.[ServiceImages]
					,CSI.[IsDeleted]
			  From Property_Service_Images CSI With (NOLOCK)
			  WHERE CSI.CustomerPropertyServiceId = CS.Id
			  AND CSI.IsDeleted != 1
			  FOR JSON PATH
			  )) as [CustomerPropertyInfoServiceImagesOuts]
		FROM Property_Services CS With (NOLOCK) where CustomerPropertyInformationId=CI.Id 
		AND CS.IsDeleted != 1
		FOR JSON PATH
		)) as [CustomerPropertyInfoServicesOuts]
		,JSON_QUERY(( 
		SELECT CG.[Id]
			  ,CG.[CustomerPropertyInformationId]
			  ,CG.[PropertyImage]
			  ,CG.[IsDeleted]
		FROM Property_Gallery CG With (NOLOCK) 
		where CG.CustomerPropertyInformationId=CI.Id
		AND CG.IsDeleted != 1
		Order By CG.[ID]
		FOR JSON PATH
		)) as [CustomerPropertyInfoGalleriesOuts]
		,JSON_QUERY(( 
		SELECT CEN.[Id]
		  ,CEN.[CustomerPropertyInformationId]
		  ,CEN.[Name]
		  ,CEN.[PhoneCountry]
		  ,CEN.[PhoneNumber]
		  ,CEN.[IsDeleted]
		  ,CEN.[DisplayOrder]
		FROM Emergency_Numbers CEN With (NOLOCK) where CEN.CustomerPropertyInformationId=CI.Id 
		AND CEN.IsDeleted != 1
		ORDER BY CEN.[DisplayOrder]
		FOR JSON PATH
		)) as [CustomerPropertyInfoEmergencyNumbersOuts]
		,JSON_QUERY(( 
		SELECT CE.[Id]
		  ,CE.[CustomerPropertyInformationId]
		  ,CE.[ExtraType]
		  ,CE.[Name]
		  ,CE.[IsDeleted]
		  ,CE.[DisplayOrder]
		   ,JSON_QUERY((
			  Select  CED.[Id]
				  ,CED.[CustomerPropertyExtraId]
				  ,CED.[Description]
				  ,CED.[Latitude]
				  ,CED.[Longitude]
				  ,CED.[IsDeleted]
			  From Property_Extra_Detail CED With (NOLOCK)
			  WHERE CED.CustomerPropertyExtraId = CE.Id
			  AND CED.IsDeleted != 1
			  FOR JSON PATH
			  )) as [customerPropertyExtraDetailsOuts]
		FROM Property_Extra CE With (NOLOCK) where CE.CustomerPropertyInformationId=CI.Id
		AND CE.IsDeleted != 1
		ORDER BY CE.[DisplayOrder]
		FOR JSON PATH
		)) as [CustomerPropertyInfoExtrasOuts]
		FROM Property_Information CI With (NOLOCK)
		Where CI.CustomerId = @CustomerId
		AND CI.CustomerGuestAppBuilderId = @AppBuilderId
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

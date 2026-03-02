using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModiFy_SpForLatLongInPropertyInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region AddPropertyInfo
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[AddPropertyInfo]    Script Date: 10/17/2023 3:00:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[AddPropertyInfo] --1,20,2
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
            JSON_VALUE(JsonData, '$.Link') AS [Link],
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
			CED.[Link],
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
				  ,CED.[Link]
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
END
");
			#endregion

			#region GetCustomersPropertiesInfoById
			migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomersPropertiesInfoById]    Script Date: 10/17/2023 12:33:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomersPropertiesInfoById] --45,2
(
	@Id INT = 0,
	@UserType INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON
 
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
    MAX(CASE WHEN [key] = 'IsActive' THEN [value] END) AS [IsActive]
    FROM CustomerPropertyInformations CI
    CROSS APPLY OPENJSON(JsonData)
         WHERE ISJSON(JsonData) = 1
               AND [DeletedAt] IS NULL
               AND (@UserType = 2)
               AND Id = @Id
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
			CI.[IsActive]
		FROM CustomerPropertyInformations CI
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
			AND Id = @Id
END");
			#endregion

			#region GetGuestAppBuilder
			migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGuestAppBuilder]    Script Date: 10/17/2023 12:52:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER    PROCEDURE [dbo].[GetGuestAppBuilder] 
(
	@BuilderId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
		SELECT [Id]
		      ,[CustomerId]
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
              ,[TransferServices]
              ,[IsActive]
              ,[IsWork]
			  ,
		      JSON_QUERY(
							(
								  SELECT [Id]
                                        ,[ScreenName]
                                        ,[JsonData]
                                        ,[RefrenceId]
                                        ,[IsActive] 
								  FROM ScreenDisplayOrderAndStatuses  as DSP
                                  WHERE 
									 [DSP].[RefrenceId] = @BuilderId
                                     AND [DSP].ScreenName = 2
									 AND [DSP].[DeletedAt] IS NULL 
								  FOR JSON PATH
										)
			                   ) as [DisplayOrderForGuestBuilder],
		JSON_QUERY(
                   (
			       SELECT [Id]
							,[CustomerId]
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
							,[Longitude]
							,[Latitude]
							,[IsActive]					
							,[IsPublish],
							JSON_QUERY(
							(
								  SELECT [Id]
                                        ,[ScreenName]
                                        ,[JsonData]
                                        ,[RefrenceId]
                                        ,[IsActive] 
								  FROM ScreenDisplayOrderAndStatuses  as DSP
                                  WHERE 
									 [DSP].[RefrenceId] = [CPI].[Id]
                                     AND [DSP].ScreenName = 1
									 AND [DSP].[DeletedAt] IS NULL 
								  FOR JSON PATH
										)
			                   ) as [DisplayOrderForPropertyInfo]
			              ,JSON_QUERY(
			              (
			                  SELECT [Id]
									,[CustomerPropertyInformationId]
									,[Name]
									,[Icon]
									,[Description]
									,[IsActive]							
									,[IsPublish]
									,JSON_QUERY(
										(
										    SELECT [Id]
												,[CustomerPropertyServiceId]
												,[ServiceImages]
												,[IsActive]										
												,[IsPublish]
										    FROM [dbo].[CustomerPropertyServiceImages] CPSI
										    WHERE [CPSI].[CustomerPropertyServiceId] = [CPS].[Id]
										          AND [CPSI].[DeletedAt] IS NULL
										    FOR JSON PATH
										)
			                        ) as [CustomerPropertyServiceImages]
			                  FROM [dbo].[CustomerPropertyServices] CPS
			                  WHERE [CPS].[CustomerPropertyInformationId] = [CPI].[Id]
			                        AND [CPS].[DeletedAt] IS NULL
			                  FOR JSON PATH
			              )
			                 ) as [CustomerPropertyServices],
						   JSON_QUERY(
							(
								  SELECT [Id]
                                        ,[CustomerPropertyInformationId]
                                        ,[Name]
                                        ,[PhoneNumber]
                                        ,[IsActive]
                                        ,[IsPublish]
										,[DisplayOrder]
								  FROM [dbo].[CustomerPropertyEmergencyNumbers] CEN
								  WHERE CEN.[CustomerPropertyInformationId] = [CPI].[Id]
								        AND [CEN].[DeletedAt] IS NULL
								  FOR JSON PATH
										)
			                ) as [CustomerPropertyEmergencyNo],
							JSON_QUERY(
							(
								  SELECT [Id]
                                        ,[CustomerPropertyInformationId]
                                        ,[PropertyImage]
                                        ,[IsActive]
                                        ,[IsPublish]
								  FROM [dbo].[CustomerPropertyGalleries] CEGL
								  WHERE CEGL.[CustomerPropertyInformationId] = [CPI].[Id]
								        AND [CEGL].[DeletedAt] IS NULL
								  FOR JSON PATH
										)
			                ) as [CustomerPropertyGallery],
							 JSON_QUERY(
			              (
			                  SELECT  [Id]
										,[CustomerPropertyInformationId]
										,[ExtraType]
										,[Name]
										,[IsActive]								
										,[IsPublish]
										,[DisplayOrder]
										,
									JSON_QUERY(
										(
										    SELECT [Id]
													,[CustomerPropertyExtraId]
													,[Description]
													,[Link]
													,[IsActive]											
													,[IsPublish]
										    FROM [dbo].[CustomerPropertyExtraDetails] CPED
										    WHERE CPED.[CustomerPropertyExtraId] = CPE.[Id]
										          AND [CPED].[DeletedAt] IS NULL
										    FOR JSON PATH
										)
			                        ) as [CustomerPropertyExtraDetails]
			                  FROM [dbo].[CustomerPropertyExtras] CPE
			                  WHERE CPE.[CustomerPropertyInformationId] = [CPI].[Id]
			                        AND CPE.[DeletedAt] IS NULL
			                  FOR JSON PATH
			              )
			                 ) as [CustomerPropertyExtras]
			       FROM [dbo].[CustomerPropertyInformations] CPI (NOLOCK)
			       WHERE [CPI].[DeletedAt] IS NULL
					AND [CPI].CustomerGuestAppBuilderId = @BuilderId
			       FOR JSON PATH
				    )
			          ) as [CustomerPropertyinfo],
			JSON_QUERY(
                   (
			       SELECT [Id]
							,[CustomerGuestAppBuilderId]
							,[CustomerId]
							,[CategoryName]
							,[IsActive]							
							,[DisplayOrder]
							,[IsPublish]
							,
			              JSON_QUERY(
			              (
			                  SELECT [Id]
										,[CustomerGuestAppBuilderId]
										,[CustomerId]
										,[CustomerGuestAppBuilderCategoryId]
										,[Badge]
										,[ShortDescription]
										,[LongDescription]
										,[ButtonType]
										,[ButtonText]
										,[ChargeType]
										,[Discount]
										,[Price]
										,[Currency]
										,[IsActive]									
										,[DisplayOrder]
										,[IsPublish]
									, 
									JSON_QUERY(
										(
										    SELECT [Id]
													,[CustomerGuestAppEnhanceYourStayItemId]
													,[ItemsImages]
													,[DisaplayOrder]
													,[IsActive]
													,[IsPublish]
										    FROM [dbo].[CustomerGuestAppEnhanceYourStayItemsImages] CGEII
										    WHERE CGEII.[CustomerGuestAppEnhanceYourStayItemId] = CGEI.[Id]
										          AND CGEII.[DeletedAt] IS NULL
										    FOR JSON PATH
										)
			                        ) as [CustomerGuestAppEnhanceYourStayItemsImages]
									, 
									JSON_QUERY(
										(
										    SELECT [Id]
													,[CustomerGuestAppEnhanceYourStayItemId]
													,[QueType]
													,[Questions]
													,[OptionValues]
													,[IsActive]
													,[IsPublish]
										    FROM [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras] CGEIE
										    WHERE CGEIE.[CustomerGuestAppEnhanceYourStayItemId] = CGEI.[Id]
										          AND CGEIE.[DeletedAt] IS NULL
										    FOR JSON PATH
										)
			                        ) as [CustomerGuestAppEnhanceYourStayCategoryItemsExtras]
			                  FROM [dbo].[CustomerGuestAppEnhanceYourStayItems] CGEI
			                  WHERE CGEI.[CustomerGuestAppBuilderCategoryId] = CGEC.[Id]
			                        AND CGEI.[DeletedAt] IS NULL
			                  FOR JSON PATH
			              )
			                 ) as [CustomerGuestAppEnhanceYourStayItems]						  
			       FROM [dbo].[CustomerGuestAppEnhanceYourStayCategories] CGEC (NOLOCK)
			       WHERE CGEC.[DeletedAt] IS NULL
					AND CGEC.CustomerGuestAppBuilderId = @BuilderId
			       FOR JSON PATH
				    )
			          ) as [CustomerGuestAppEnhanceYourStayCategories]
					  ,
				JSON_QUERY(
					(
						SELECT
						    [CC].[Id],
						    [CC].[CustomerGuestAppBuilderId],
						    [CC].[CustomerId],
						    [CC].[CategoryName],
						    [CC].[IsActive],
						    [CC].[DisplayOrder],
						    [CC].[IsPublish],
						    [CC].[JsonData],
						    JSON_QUERY((
						        SELECT
						            [CI].[Id],
						            [CI].[CustomerId],
						            [CI].[CustomerGuestAppBuilderId],
						            [CI].[CustomerGuestAppConciergeCategoryId],
						            [CI].[Name],
						            [CI].[ItemsMonth],
						            [CI].[ItemsDay],
						            [CI].[ItemsMinute],
						            [CI].[ItemsHour],
						            [CI].[QuantityBar],
						            [CI].[ItemLocation],
						            [CI].[Comment],
						            [CI].[IsPriceEnable],
						            [CI].[Price],
						            [CI].[Currency],
						            [CI].[IsActive],
						            [CI].[DisplayOrder],
						            [CI].[IsPublish]
						        FROM [dbo].[CustomerGuestAppConciergeItems] AS [CI]
						        WHERE [CI].[CustomerGuestAppConciergeCategoryId] = [CC].[Id]
								AND [CI].[DeletedAt] IS NULL
 						        FOR JSON PATH
						    )) AS [Conciergeitem]
						FROM [dbo].[CustomerGuestAppConciergeCategories] AS [CC]
						WHERE [CC].[DeletedAt] IS NULL
						AND [CC].[CustomerGuestAppBuilderId] = @BuilderId
						FOR JSON PATH
					)
						) as [CustomerGuestAppConciergeCategories],
				JSON_QUERY(
					(
					SELECT
						    [RCC].[Id],
						    [RCC].[CustomerGuestAppBuilderId],
						    [RCC].[CustomerId],
						    [RCC].[CategoryName],
						    [RCC].[IsActive],
						    [RCC].[DisplayOrder],
						    [RCC].[IsPublish],
						    JSON_QUERY((
						        SELECT
						            [RCI].[Id],
						            [RCI].[CustomerId],
						            [RCI].[CustomerGuestAppBuilderId],
						            [RCI].[CustomerGuestAppReceptionCategoryId],
						            [RCI].[Name],
						            [RCI].[ItemsMonth],
						            [RCI].[ItemsDay],
						            [RCI].[ItemsMinute],
						            [RCI].[ItemsHour],
						            [RCI].[QuantityBar],
						            [RCI].[ItemLocation],
						            [RCI].[Comment],
						            [RCI].[IsPriceEnable],
						            [RCI].[Price],
						            [RCI].[Currency],
						            [RCI].[IsActive],
						            [RCI].[DisplayOrder],
						            [RCI].[IsPublish]
						        FROM [dbo].[CustomerGuestAppReceptionItems] AS [RCI]
						        WHERE [RCI].[CustomerGuestAppReceptionCategoryId] = [RCC].[Id]
								AND [RCI].[DeletedAt] IS NULL
						        FOR JSON PATH
						    )) AS [ReceptionItem]
						FROM [dbo].[CustomerGuestAppReceptionCategories] AS [RCC]
						WHERE [RCC].[DeletedAt] IS NULL
						AND [RCC].[CustomerGuestAppBuilderId] = @BuilderId
						FOR JSON PATH

					)) as [CustomerGuestAppReceptionCategories],
				JSON_QUERY(
					(
						SELECT
						    [HC].[Id],
						    [HC].[CustomerGuestAppBuilderId],
						    [HC].[CustomerId],
						    [HC].[CategoryName],
						    [HC].[IsActive],
						    [HC].[DisplayOrder],
						    [HC].[IsPublish],
						    JSON_QUERY((
						        SELECT
						            [HI].[Id],
						            [HI].[CustomerId],
						            [HI].[CustomerGuestAppBuilderId],
						            [HI].[CustomerGuestAppHousekeepingCategoryId],
						            [HI].[Name],
						            [HI].[ItemsMonth],
						            [HI].[ItemsDay],
						            [HI].[ItemsMinute],
						            [HI].[ItemsHour],
						            [HI].[QuantityBar],
						            [HI].[ItemLocation],
						            [HI].[Comment],
						            [HI].[IsPriceEnable],
						            [HI].[Price],
						            [HI].[Currency],
						            [HI].[IsActive],
						            [HI].[DisplayOrder],
						            [HI].[IsPublish]
						        FROM [dbo].[CustomerGuestAppHousekeepingItems] AS [HI]
						        WHERE [HI].[CustomerGuestAppHousekeepingCategoryId] = [HC].[Id]
								AND [HI].[DeletedAt] IS NULL
						        FOR JSON PATH
						    )) AS [HouseItem]
						FROM [dbo].[CustomerGuestAppHousekeepingCategories] AS [HC]
						WHERE [HC].[DeletedAt] IS NULL
						AND [HC].[CustomerGuestAppBuilderId] = @BuilderId
						FOR JSON PATH
					)) as [CustomerGuestAppHousekeepingCategories],
					JSON_QUERY(
					(
						SELECT
						    [RC].[Id],
						    [RC].[CustomerGuestAppBuilderId],
						    [RC].[CustomerId],
						    [RC].[CategoryName],
						    [RC].[IsActive],
						    [RC].[DisplayOrder],
						    [RC].[IsPublish],
						    JSON_QUERY((
						        SELECT
						            [RI].[Id],
						            [RI].[CustomerId],
						            [RI].[CustomerGuestAppBuilderId],
						            [RI].[CustomerGuestAppRoomServiceCategoryId],
						            [RI].[Name],
						            [RI].[ItemsMonth],
						            [RI].[ItemsDay],
						            [RI].[ItemsMinute],
						            [RI].[ItemsHour],
						            [RI].[QuantityBar],
						            [RI].[ItemLocation],
						            [RI].[Comment],
						            [RI].[IsPriceEnable],
						            [RI].[Price],
						            [RI].[Currency],
						            [RI].[IsActive],
						            [RI].[DisplayOrder],
						            [RI].[IsPublish]
						        FROM [dbo].[CustomerGuestAppRoomServiceItems] AS [RI]
						        WHERE [RI].[CustomerGuestAppRoomServiceCategoryId] = [RC].[Id]
								AND [RI].[DeletedAt] IS NULL
						        FOR JSON PATH
						    )) AS [RoomItem]
						FROM [dbo].[CustomerGuestAppRoomServiceCategories] AS [RC]
						WHERE [RC].[DeletedAt] IS NULL
						AND [RC].[CustomerGuestAppBuilderId] = @BuilderId
						FOR JSON PATH
					)) as [CustomerGuestAppRoomServiceCategories]
				From [dbo].[CustomerGuestAppBuilders] CGA (NOLOCK)
				WHERE [CGA].[DeletedAt] IS NULL
				AND [CGA].[Id] = @BuilderId
				 FOR JSON PATH
        ) 
END
");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

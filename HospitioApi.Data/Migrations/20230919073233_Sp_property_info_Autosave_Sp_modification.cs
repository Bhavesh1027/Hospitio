using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Sp_property_info_Autosave_Sp_modification : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region AddProprty

            migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[AddPropertyInfo] --  1,1
(
	@AppBuilderId int = 0,
	@CustomerId int  = 0,
	@UserType INT = 0
)
AS BEGIN
	SET NOCOUNT ON;
	Declare @CustomerAppBuliderId int
	Declare @DisplayOrder int 
	Declare @PropertyId int
	DECLARE @RoomName NVARCHAR(100)
	Declare @ISDELETE BIT = 0;

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

	WITH Property_Information AS (
		SELECT
			CI.Id,
			CI.CustomerId,
			JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
			JSON_VALUE(JsonData, '$.WifiUsername') AS [WifiUsername],
			JSON_VALUE(JsonData, '$.WifiPassword') AS [WifiPassword],
			JSON_VALUE(JsonData, '$.Overview') AS [Overview],
			JSON_VALUE(JsonData, '$.CheckInPolicy') AS [CheckInPolicy],
			JSON_VALUE(JsonData, '$.TermsAndConditions') AS [TermsAndConditions],
			JSON_VALUE(JsonData, '$.Street') AS [Street],
			JSON_VALUE(JsonData, '$.StreetNumber') AS [StreetNumber],
			JSON_VALUE(JsonData, '$.City') AS [City],
			JSON_VALUE(JsonData, '$.Postalcode') AS [Postalcode],
			JSON_VALUE(JsonData, '$.Country') AS [Country],
			@RoomName AS RoomName
		FROM CustomerPropertyInformations CI
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)
		
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
			@RoomName AS RoomName
		FROM CustomerPropertyInformations CI
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	), 
	Property_Extra AS (
		SELECT [Id],
			[CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.ExtraType') AS [ExtraType],
			JSON_VALUE(JsonData, '$.Name') AS [Name],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
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
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
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
			@ISDELETE As [IsDeleted]
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
		  ,@RoomName AS RoomName
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
		FROM Emergency_Numbers CEN With (NOLOCK) where CEN.CustomerPropertyInformationId=CI.Id 
		AND CEN.IsDeleted != 1
		FOR JSON PATH
		)) as [CustomerPropertyInfoEmergencyNumbersOuts]
		,JSON_QUERY(( 
		SELECT CE.[Id]
		  ,CE.[CustomerPropertyInformationId]
		  ,CE.[ExtraType]
		  ,CE.[Name]
		  ,CE.[IsDeleted]
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
		FOR JSON PATH
		)) as [CustomerPropertyInfoExtrasOuts]
		FROM Property_Information CI With (NOLOCK)
		Where CI.CustomerId = @CustomerId
		AND CI.CustomerGuestAppBuilderId = @AppBuilderId
		FOR JSON PATH )
	END
END");

			#endregion

			#region GetCustomerPropertyExtras

			migrationBuilder.Sql(@"CREATE OR ALTER  PROCEDURE [dbo].[GetCustomerPropertyExtras] 
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
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyExtras CE 
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			

		UNION ALL

		SELECT CE.[Id]
		  ,CE.[CustomerPropertyInformationId]
		  ,CE.[ExtraType]
		  ,CE.[Name]
		  ,@ISDELETE As [IsDeleted]
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
            JSON_VALUE(JsonData, '$.Link') AS [Link],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyExtraDetails CED
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			

		UNION ALL

		SELECT  
			CED.[Id],
			CED.[CustomerPropertyExtraId],
			CED.[Description],
			CED.[Link],
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
		FROM Property_Extra CE With (NOLOCK) where CE.CustomerPropertyInformationId = @CustomerPropertyInformationId
		AND CE.IsDeleted != 1
		FOR JSON PATH
	) AS [CustomerPropertyInfoExtrasOuts]
    
END");
			#endregion

			#region GetCustomerPropertyServicesForAPPBuilder
			migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPropertyServicesForAPPBuilder]
(
    @CustomerPropertyInformationId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @ISDELETE BIT = 0;

    WITH Property_Services AS (
        SELECT 
            CS.[Id],
            CS.[CustomerPropertyInformationId],
            JSON_VALUE(JsonData, '$.Name') AS [Name],
            JSON_VALUE(JsonData, '$.Icon') AS [Icon],
            JSON_VALUE(JsonData, '$.Description') AS [Description],
            JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
        FROM CustomerPropertyServices CS
        WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL

        UNION ALL

        SELECT 
            CS.[Id],
            CS.[CustomerPropertyInformationId],
            CS.[Name],
            CS.[Icon],
            CS.[Description],
            @ISDELETE AS [IsDeleted]
        FROM CustomerPropertyServices CS
        WHERE [DeletedAt] IS NULL
            AND JsonData IS NULL
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

        UNION ALL

        SELECT  
            CSI.[Id],
            CSI.[CustomerPropertyServiceId],
            CSI.[ServiceImages],
            @ISDELETE AS [IsDeleted]
        FROM CustomerPropertyServiceImages CSI
        WHERE [DeletedAt] IS NULL
            AND JsonData IS NULL
    )

    SELECT
        (
            SELECT 
                CS.[Id],
                CS.[CustomerPropertyInformationId],
                CS.[Name],
                CS.[Icon],
                CS.[Description],
                CS.[IsDeleted],
                JSON_QUERY(
				(
                    SELECT  
                        CSI.[Id],
                        CSI.[CustomerPropertyServiceId],
                        CSI.[ServiceImages],
                        CSI.[IsDeleted]
                    FROM Property_Service_Images CSI WITH (NOLOCK)
                    WHERE CSI.CustomerPropertyServiceId = CS.Id
                    AND CSI.IsDeleted != 1
                    FOR JSON PATH
                )
				) AS [CustomerPropertyInfoServiceImagesOuts]
            FROM Property_Services CS WITH (NOLOCK) 
            WHERE CustomerPropertyInformationId = @CustomerPropertyInformationId 
            AND CS.IsDeleted != 1
            FOR JSON PATH
        ) AS [CustomerPropertyInfoServicesOuts]
		OPTION (RECOMPILE);
END");
			#endregion

			#region GetCustomersPropertiesInfoById
			migrationBuilder.Sql(@"CREATE OR ALTER  PROCEDURE [dbo].[GetCustomersPropertiesInfoById] 
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
			JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
			JSON_VALUE(JsonData, '$.WifiUsername') AS [WifiUsername],
			JSON_VALUE(JsonData, '$.WifiPassword') AS [WifiPassword],
			JSON_VALUE(JsonData, '$.Overview') AS [Overview],
			JSON_VALUE(JsonData, '$.CheckInPolicy') AS [CheckInPolicy],
			JSON_VALUE(JsonData, '$.TermsAndConditions') AS [TermsAndConditions],
			JSON_VALUE(JsonData, '$.Street') AS [Street],
			JSON_VALUE(JsonData, '$.StreetNumber') AS [StreetNumber],
			JSON_VALUE(JsonData, '$.City') AS [City],
			JSON_VALUE(JsonData, '$.Postalcode') AS [Postalcode],
			JSON_VALUE(JsonData, '$.Country') AS [Country],
			JSON_VALUE(JsonData, '$.IsActive') AS [IsActive]
		FROM CustomerPropertyInformations CI
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)
			AND Id = @Id
		
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
			CI.[IsActive]
		FROM CustomerPropertyInformations CI
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
			AND Id = @Id
END  
");
			#endregion

			#region GetCustomerPropertyEmergencyNumbers
			migrationBuilder.Sql(@"CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerPropertyEmergencyNumbers]
(
	@PropertyId INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON


	SELECT 
			[Id],
			JSON_VALUE(JsonData, '$.CustomerPropertyInformationId') AS [CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.Name') AS [Name],
			JSON_VALUE(JsonData, '$.PhoneCountry') AS [PhoneCountry],
			JSON_VALUE(JsonData, '$.PhoneNumber') AS [PhoneNumber]
		FROM CustomerPropertyEmergencyNumbers CEN
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND CustomerPropertyInformationId = @PropertyId

		UNION ALL

		SELECT 
			CEN.[Id],
			CEN.[CustomerPropertyInformationId],
			CEN.[Name],
			CEN.[PhoneCountry],
			CEN.[PhoneNumber]		  
		FROM CustomerPropertyEmergencyNumbers CEN
		WHERE [DeletedAt] IS NULL
			AND JsonData IS NULL
			AND CustomerPropertyInformationId = @PropertyId
END
");
			#endregion

			#region GetCustomersEnhanceYourStayCategoryItemById
			migrationBuilder.Sql(@"CREATE OR ALTER   PROC [dbo].[GetCustomersEnhanceYourStayCategoryItemById]
(
	@Id INT = 0,
	@UserType INT = 0
)
AS
BEGIN
	SET NOCOUNT ON;
    SET XACT_ABORT ON;
	Declare @ISDELETE BIT = 0;

	WITH CustomerEnhanceYourStayItems AS
    (
        SELECT
            [Id],
            [CustomerId],
            JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
			JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderCategoryId') AS [CustomerGuestAppBuilderCategoryId],
            JSON_VALUE(JsonData, '$.Badge') AS [Badge],
            JSON_VALUE(JsonData, '$.ShortDescription') AS [ShortDescription],
            JSON_VALUE(JsonData, '$.LongDescription') AS [LongDescription],
            JSON_VALUE(JsonData, '$.ButtonType') AS [ButtonType],
            JSON_VALUE(JsonData, '$.ButtonText') AS [ButtonText],
            JSON_VALUE(JsonData, '$.ChargeType') AS [ChargeType],
            JSON_VALUE(JsonData, '$.Discount') AS [Discount],
            JSON_VALUE(JsonData, '$.Price') AS [Price],
            JSON_VALUE(JsonData, '$.Currency') AS [Currency],
            JSON_VALUE(JsonData, '$.IsActive') AS [IsActive],
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
        FROM [dbo].[CustomerGuestAppEnhanceYourStayItems] items (NOLOCK)
		WHERE [items].[DeletedAt] IS NULL
		AND [Id] = @Id
        AND ISJSON(JsonData) = 1
		AND (@UserType = 2)

        UNION ALL 

        SELECT
            [Id],
            [CustomerId],
            [CustomerGuestAppBuilderId],
			[CustomerGuestAppBuilderCategoryId],
            [Badge],
            [ShortDescription],
            [LongDescription],
            [ButtonType],
            [ButtonText],
            [ChargeType],
            [Discount],
            [Price],
            [Currency],
            [IsActive],
            [DisplayOrder],
			@ISDELETE AS [IsDeleted]
        FROM [dbo].[CustomerGuestAppEnhanceYourStayItems] items (NOLOCK)
		WHERE [items].[DeletedAt] IS NULL
		AND [Id] = @Id
		AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
    ), 
	Customer_EnhanceYourStay_Item_Image AS
    (
		SELECT
				ItemImage1.[Id],
				JSON_VALUE(ItemImage1.JsonData, '$.CustomerGuestAppEnhanceYourStayItemId') AS [CustomerGuestAppEnhanceYourStayItemId],
				JSON_VALUE(ItemImage1.JsonData, '$.ItemsImages') AS  [ItemsImages],
				JSON_VALUE(ItemImage1.JsonData, '$.DisaplayOrder') AS [DisaplayOrder],
				JSON_VALUE(ItemImage1.JsonData, '$.IsActive') AS [IsActive],
				JSON_VALUE(ItemImage1.JsonData, '$.IsDeleted') AS [IsDeleted]
			FROM [dbo].[CustomerGuestAppEnhanceYourStayItemsImages] ItemImage1 (NOLOCK)
			WHERE ISJSON(ItemImage1.JsonData) = 1
				AND [CustomerGuestAppEnhanceYourStayItemId] = @Id
				AND ItemImage1.[DeletedAt] IS NULL
				AND (@UserType = 2)

			UNION ALL

			SELECT
				ItemImage.[Id],
				ItemImage.[CustomerGuestAppEnhanceYourStayItemId],
				ItemImage.[ItemsImages],
				ItemImage.[DisaplayOrder],
				ItemImage.[IsActive],
				@ISDELETE AS [IsDeleted]
			FROM [dbo].[CustomerGuestAppEnhanceYourStayItemsImages] ItemImage(NOLOCK)
			WHERE ItemImage.[DeletedAt] IS NULL
				AND [CustomerGuestAppEnhanceYourStayItemId] = @Id
				AND ((@UserType = 2 AND ItemImage.JsonData IS NULL) OR (@UserType = 3 AND ItemImage.IsPublish = 1))  
	),
	Customer_EnhanceYourStay_Item_Extra AS
    (
		SELECT
				ItemExtra1.[Id],
				JSON_VALUE(ItemExtra1.JsonData, '$.QueType') AS  [QueType],
				JSON_VALUE(ItemExtra1.JsonData, '$.Questions') AS [Questions],
				JSON_VALUE(ItemExtra1.JsonData, '$.OptionValues') AS [OptionValues],
				JSON_VALUE(ItemExtra1.JsonData, '$.IsDeleted') AS [IsDeleted]
			FROM [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras] ItemExtra1 (NOLOCK)
			WHERE ISJSON(ItemExtra1.JsonData) = 1
				AND ItemExtra1.[DeletedAt] IS NULL
				AND ItemExtra1.[CustomerGuestAppEnhanceYourStayItemId] = @Id
				AND (@UserType = 2)

			UNION ALL

			SELECT
				 ItemExtra.[Id],
                 ItemExtra.[QueType],
                 ItemExtra.[Questions],
                 ItemExtra.[OptionValues],
				 @ISDELETE AS [IsDeleted]
			FROM [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras] ItemExtra (NOLOCK)
			WHERE ItemExtra.[DeletedAt] IS NULL
				AND ItemExtra.[CustomerGuestAppEnhanceYourStayItemId] = @Id
				AND ((@UserType = 2 AND ItemExtra.JsonData IS NULL) OR (@UserType = 3 AND ItemExtra.IsPublish = 1))  
	)
	SELECT (
                SELECT
                    cci.[Id],
                    cci.[CustomerId],
                    cci.[CustomerGuestAppBuilderId],
                    cci.[CustomerGuestAppBuilderCategoryId],
                    cci.[Badge],
                    cci.[ShortDescription],
                    cci.[LongDescription],
                    cci.[ButtonType],
                    cci.[ButtonText],
                    cci.[ChargeType],
                    cci.[Discount],
                    cci.[Price],
                    cci.[Currency],
                    cci.[IsActive],
                    cci.[DisplayOrder]
					,
					(
						Select 
							CEII.[Id],
							CEII.[CustomerGuestAppEnhanceYourStayItemId],
							CEII.[ItemsImages],
							CEII.[DisaplayOrder],
							CEII.[IsActive]
							FROM Customer_EnhanceYourStay_Item_Image CEII
							WHERE CEII.[CustomerGuestAppEnhanceYourStayItemId] = cci.[Id]
							AND CEII.IsDeleted != 1
							ORDER BY CEII.[DisaplayOrder] ASC
							FOR JSON PATH
 					) AS [ItemsImages]
					,
					(
						Select 
							  CEIE.[Id],
                              CEIE.[QueType],
                              CEIE.[Questions],
                              CEIE.[OptionValues]
							FROM Customer_EnhanceYourStay_Item_Extra CEIE
							Where CEIE.IsDeleted != 1
							FOR JSON PATH
 					) AS [CustomerEnhanceYourStayCategoryItemsExtra]
                FROM CustomerEnhanceYourStayItems cci
				Where cci.IsDeleted != 1
				Order by cci.[DisplayOrder] ASC
                FOR JSON PATH
            ) AS [CustomerEnhanceYourStayItemByIdOut]
        
		OPTION (RECOMPILE);
END");

			#endregion

			

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

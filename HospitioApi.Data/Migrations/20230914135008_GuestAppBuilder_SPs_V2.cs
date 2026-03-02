using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GuestAppBuilder_SPs_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerConciergeWithRelationSP

            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerConciergeWithRelationSP]
(
    @AppBuilderId INT = 0,
    @UserType INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

	Declare @ISDELETE BIT = 0;

    WITH Customer_Concierge_Results AS
    (

        SELECT
            [Id],
            JSON_VALUE(JsonData, '$.CustomerId') AS [CustomerId],
            JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
            JSON_VALUE(JsonData, '$.CategoryName') AS [CategoryName],
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder],
            JSON_VALUE(JsonData, '$.IsActive') AS [IsActive],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
        FROM [dbo].[CustomerGuestAppConciergeCategories] categories1 (NOLOCK)
        WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
            AND [CustomerGuestAppBuilderId] = @AppBuilderId
			AND (@UserType = 2)
			--AND JSON_VALUE(JsonData, '$.IsDeleted') = 0

        UNION ALL

        SELECT
            [Id],
            [CustomerId],
            [CustomerGuestAppBuilderId],
            [CategoryName],
            [DisplayOrder],
            [IsActive],
			@ISDELETE As [IsDeleted]
        FROM [dbo].[CustomerGuestAppConciergeCategories] categories (NOLOCK)
        WHERE [DeletedAt] IS NULL
            AND [CustomerGuestAppBuilderId] = @AppBuilderId
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1))  
    ),
    CustomerConciergeItems AS
    (
        SELECT
            [Id],
            [CustomerId],
            JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
            [CustomerGuestAppConciergeCategoryId],
            JSON_VALUE(JsonData, '$.Name') AS [Name],
            JSON_VALUE(JsonData, '$.ItemsMonth') AS [ItemsMonth],
            JSON_VALUE(JsonData, '$.ItemsDay') AS [ItemsDay],
            JSON_VALUE(JsonData, '$.ItemsMinute') AS [ItemsMinute],
            JSON_VALUE(JsonData, '$.ItemsHour') AS [ItemsHour],
            JSON_VALUE(JsonData, '$.QuantityBar') AS [QuantityBar],
            JSON_VALUE(JsonData, '$.ItemLocation') AS [ItemLocation],
            JSON_VALUE(JsonData, '$.Comment') AS [Comment],
            JSON_VALUE(JsonData, '$.IsPriceEnable') AS [IsPriceEnable],
            JSON_VALUE(JsonData, '$.Price') AS [Price],
            JSON_VALUE(JsonData, '$.Currency') AS [Currency],
            JSON_VALUE(JsonData, '$.IsActive') AS [IsActive],
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
        FROM [dbo].[CustomerGuestAppConciergeItems] items (NOLOCK)
		WHERE [items].[DeletedAt] IS NULL
		--AND [CustomerGuestAppBuilderId] = @AppBuilderId
        AND ISJSON(JsonData) = 1
		AND (@UserType = 2)

        UNION ALL 

        SELECT
            [Id],
            [CustomerId],
            [CustomerGuestAppBuilderId],
            [CustomerGuestAppConciergeCategoryId],
            [Name],
            [ItemsMonth],
            [ItemsDay],
            [ItemsMinute],
            [ItemsHour],
            [QuantityBar],
            [ItemLocation],
            [Comment],
            [IsPriceEnable],
            [Price],
            [Currency],
            [IsActive],
            [DisplayOrder],
			@ISDELETE As [IsDeleted]
        FROM [dbo].[CustomerGuestAppConciergeItems] items (NOLOCK)
		WHERE [items].[DeletedAt] IS NULL
		AND [CustomerGuestAppBuilderId] = @AppBuilderId
		AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
    )
    SELECT (
        SELECT
            ccr.[Id],
            ccr.[CustomerId],
            ccr.[CustomerGuestAppBuilderId],
            ccr.[CategoryName],
            ccr.[DisplayOrder],
            ccr.[IsActive],
			ccr.[IsDeleted],
            (
                SELECT
                    cci.[Id],
                    cci.[CustomerId],
                    cci.[CustomerGuestAppBuilderId],
                    cci.[CustomerGuestAppConciergeCategoryId],
                    cci.[Name],
                    cci.[ItemsMonth],
                    cci.[ItemsDay],
                    cci.[ItemsMinute],
                    cci.[ItemsHour],
                    cci.[QuantityBar],
                    cci.[ItemLocation],
                    cci.[Comment],
                    cci.[IsPriceEnable],
                    cci.[Price],
                    cci.[Currency],
                    cci.[IsActive],
                    cci.[DisplayOrder],
					cci.[IsDeleted]
                FROM CustomerConciergeItems cci 
                WHERE cci.[CustomerGuestAppConciergeCategoryId] = ccr.[Id]
				AND cci.IsDeleted != 1
				Order by cci.[DisplayOrder] ASC
                FOR JSON PATH
            ) AS [CustomerConciergeItems]
        FROM Customer_Concierge_Results ccr
		Where ccr.IsDeleted != 1
		Order by ccr.[DisplayOrder] ASC
		FOR JSON PATH
            ) AS [CustomerConciergeWithRelationOut]
		OPTION (RECOMPILE);

            
END

");

            #endregion

            #region GetCustomerRoomServiceWithRelationSP

            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerRoomServiceWithRelationSP] 
(
    @AppBuilderId INT = 0,
	@UserType INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

	Declare @ISDELETE BIT = 0;

	WITH Customer_Roomservice_Results AS
    (
        SELECT
            [Id],
            JSON_VALUE(JsonData, '$.CustomerId') AS [CustomerId],
            JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
            JSON_VALUE(JsonData, '$.CategoryName') AS [CategoryName],
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder],
            JSON_VALUE(JsonData, '$.IsActive') AS [IsActive],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
        FROM [dbo].[CustomerGuestAppRoomServiceCategories] categories
        WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
            AND [CustomerGuestAppBuilderId] = @AppBuilderId
			AND (@UserType = 2)

        UNION ALL

        SELECT
            [Id],
            [CustomerId],
            [CustomerGuestAppBuilderId],
            [CategoryName],
            [DisplayOrder],
            [IsActive],
			@ISDELETE AS [IsDeleted]
        FROM [dbo].[CustomerGuestAppRoomServiceCategories] categories
        WHERE [DeletedAt] IS NULL
            AND [CustomerGuestAppBuilderId] = @AppBuilderId
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1))  
    ),
    CustomerRoomServiceItems AS
    (
        SELECT
            [Id],
            JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
            [CustomerGuestAppRoomServiceCategoryId],
            JSON_VALUE(JsonData, '$.Name') AS [Name],
            JSON_VALUE(JsonData, '$.ItemsMonth') AS [ItemsMonth],
            JSON_VALUE(JsonData, '$.ItemsDay') AS [ItemsDay],
            JSON_VALUE(JsonData, '$.ItemsMinute') AS [ItemsMinute],
            JSON_VALUE(JsonData, '$.ItemsHour') AS [ItemsHour],
            JSON_VALUE(JsonData, '$.QuantityBar') AS [QuantityBar],
            JSON_VALUE(JsonData, '$.ItemLocation') AS [ItemLocation],
            JSON_VALUE(JsonData, '$.Comment') AS [Comment],
            JSON_VALUE(JsonData, '$.IsPriceEnable') AS [IsPriceEnable],
            JSON_VALUE(JsonData, '$.Price') AS [Price],
            JSON_VALUE(JsonData, '$.Currency') AS [Currency],
            JSON_VALUE(JsonData, '$.IsActive') AS [IsActive],
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
        FROM [dbo].[CustomerGuestAppRoomServiceItems] items (NOLOCK)
		WHERE [items].[DeletedAt] IS NULL
		AND [CustomerGuestAppBuilderId] = @AppBuilderId
        AND ISJSON(JsonData) = 1
		AND (@UserType = 2)

        UNION ALL 

        SELECT
            [Id],
            [CustomerGuestAppBuilderId],
            [CustomerGuestAppRoomServiceCategoryId],
            [Name],
            [ItemsMonth],
            [ItemsDay],
            [ItemsMinute],
            [ItemsHour],
            [QuantityBar],
            [ItemLocation],
            [Comment],
            [IsPriceEnable],
            [Price],
            [Currency],
            [IsActive],
            [DisplayOrder],
			@ISDELETE AS [IsDeleted]
        FROM [dbo].[CustomerGuestAppRoomServiceItems] items (NOLOCK)
		WHERE [items].[DeletedAt] IS NULL
		AND [CustomerGuestAppBuilderId] = @AppBuilderId
		AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
    )

    SELECT (
        SELECT
            category.[Id],
            category.[CustomerId],
            category.[CustomerGuestAppBuilderId],
            category.[CategoryName],
            category.[DisplayOrder],
            category.[IsActive],
			category.[IsDeleted],
            (
                SELECT
                    items.[Id],
                    items.[CustomerGuestAppBuilderId],
                    items.[CustomerGuestAppRoomServiceCategoryId],
                    items.[Name],
                    items.[ItemsMonth],
                    items.[ItemsDay],
                    items.[ItemsMinute],
                    items.[ItemsHour],
                    items.[QuantityBar],
                    items.[ItemLocation],
                    items.[Comment],
                    items.[IsPriceEnable],
                    items.[Price],
                    items.[Currency],
                    items.[IsActive],
                    items.[DisplayOrder],
					items.[IsDeleted]
                FROM CustomerRoomserviceItems items
                WHERE items.[CustomerGuestAppRoomServiceCategoryId] = category.[Id]
				AND items.[IsDeleted] != 1
				Order by items.[DisplayOrder] ASC
                FOR JSON PATH
            ) AS [CustomerRoomServiceItems]
        FROM Customer_Roomservice_Results category
		Where category.[IsDeleted] != 1
		Order by category.[DisplayOrder] ASC
		FOR JSON PATH
            ) AS [CustomerRoomServiceWithRelationOut]
		OPTION (RECOMPILE);

END");
            #endregion

            #region  GetCustomerHouseKeepingWithRelationSP
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomerHouseKeepingWithRelationSP] 
(
    @AppBuilderId INT = 0,
	@UserType INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	Declare @ISDELETE BIT = 0;

	WITH Customer_Housekeeping_Results AS
    (
        SELECT
            [Id],
            JSON_VALUE(JsonData, '$.CustomerId') AS [CustomerId],
            JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
            JSON_VALUE(JsonData, '$.CategoryName') AS [CategoryName],
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder],
            JSON_VALUE(JsonData, '$.IsActive') AS [IsActive],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
        FROM [dbo].[CustomerGuestAppHousekeepingCategories] categories1
        WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
            AND [CustomerGuestAppBuilderId] = @AppBuilderId
			AND (@UserType = 2)

        UNION ALL

        SELECT
            [Id],
            [CustomerId],
            [CustomerGuestAppBuilderId],
            [CategoryName],
            [DisplayOrder],
            [IsActive],
			@ISDELETE As [IsDeleted]
        FROM [dbo].[CustomerGuestAppHousekeepingCategories] categories
        WHERE [DeletedAt] IS NULL
            AND [CustomerGuestAppBuilderId] = @AppBuilderId
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1))  
    ),
    CustomerHouseKeepingItems AS
    (
        SELECT
            [Id],
            [CustomerId],
            JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
            [CustomerGuestAppHousekeepingCategoryId],
            JSON_VALUE(JsonData, '$.Name') AS [Name],
            JSON_VALUE(JsonData, '$.ItemsMonth') AS [ItemsMonth],
            JSON_VALUE(JsonData, '$.ItemsDay') AS [ItemsDay],
            JSON_VALUE(JsonData, '$.ItemsMinute') AS [ItemsMinute],
            JSON_VALUE(JsonData, '$.ItemsHour') AS [ItemsHour],
            JSON_VALUE(JsonData, '$.QuantityBar') AS [QuantityBar],
            JSON_VALUE(JsonData, '$.ItemLocation') AS [ItemLocation],
            JSON_VALUE(JsonData, '$.Comment') AS [Comment],
            JSON_VALUE(JsonData, '$.IsPriceEnable') AS [IsPriceEnable],
            JSON_VALUE(JsonData, '$.Price') AS [Price],
            JSON_VALUE(JsonData, '$.Currency') AS [Currency],
            JSON_VALUE(JsonData, '$.IsActive') AS [IsActive],
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
        FROM [dbo].[CustomerGuestAppHousekeepingItems] items (NOLOCK)
		WHERE [items].[DeletedAt] IS NULL
		AND [CustomerGuestAppBuilderId] = @AppBuilderId
        AND ISJSON(JsonData) = 1
		AND (@UserType = 2)

        UNION ALL 

        SELECT
            [Id],
            [CustomerId],
            [CustomerGuestAppBuilderId],
            [CustomerGuestAppHousekeepingCategoryId],
            [Name],
            [ItemsMonth],
            [ItemsDay],
            [ItemsMinute],
            [ItemsHour],
            [QuantityBar],
            [ItemLocation],
            [Comment],
            [IsPriceEnable],
            [Price],
            [Currency],
            [IsActive],
            [DisplayOrder],
			@ISDELETE As [IsDeleted]
        FROM [dbo].[CustomerGuestAppHousekeepingItems] items (NOLOCK)
		WHERE [items].[DeletedAt] IS NULL
		AND [CustomerGuestAppBuilderId] = @AppBuilderId
		AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
    )
    SELECT (
        SELECT
            category.[Id],
            category.[CustomerId],
            category.[CustomerGuestAppBuilderId],
            category.[CategoryName],
            category.[DisplayOrder],
            category.[IsActive],
			category.[IsDeleted],
            (
                SELECT
                    items.[Id],
                    items.[CustomerId],
                    items.[CustomerGuestAppBuilderId],
                    items.[CustomerGuestAppHousekeepingCategoryId],
                    items.[Name],
                    items.[ItemsMonth],
                    items.[ItemsDay],
                    items.[ItemsMinute],
                    items.[ItemsHour],
                    items.[QuantityBar],
                    items.[ItemLocation],
                    items.[Comment],
                    items.[IsPriceEnable],
                    items.[Price],
                    items.[Currency],
                    items.[IsActive],
                    items.[DisplayOrder],
					items.[IsDeleted]
                FROM CustomerHousekeepingItems items
                WHERE items.[CustomerGuestAppHousekeepingCategoryId] = category.[Id]
				AND items.[IsDeleted] != 1
				Order by items.[DisplayOrder] ASC
                FOR JSON PATH
            ) AS [CustomerHouseKeepingItems]
        FROM Customer_Housekeeping_Results category
		Where category.[IsDeleted] != 1
		Order by category.[DisplayOrder] ASC
		FOR JSON PATH
            ) AS [CustomerHouseKeepingWithRelationOut]
		OPTION (RECOMPILE);

END");
            #endregion

            #region  GetCustomerReceptionWithRelationSP

            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerReceptionWithRelationSP]
(
    @AppBuilderId INT = 0,
	@UserType INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	Declare @ISDELETE BIT = 0;

	WITH Customer_Reception_Results AS
    (
        SELECT
            [Id],
            JSON_VALUE(JsonData, '$.CustomerId') AS [CustomerId],
            JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
            JSON_VALUE(JsonData, '$.CategoryName') AS [CategoryName],
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder],
            JSON_VALUE(JsonData, '$.IsActive') AS [IsActive],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
        FROM [dbo].[CustomerGuestAppReceptionCategories] categories
        WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
            AND [CustomerGuestAppBuilderId] = @AppBuilderId
			AND (@UserType = 2)

        UNION ALL

        SELECT
            [Id],
            [CustomerId],
            [CustomerGuestAppBuilderId],
            [CategoryName],
            [DisplayOrder],
            [IsActive],
			@ISDELETE As [IsDeleted]
        FROM [dbo].[CustomerGuestAppReceptionCategories] categories
        WHERE [DeletedAt] IS NULL
            AND [CustomerGuestAppBuilderId] = @AppBuilderId
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1))  
    ),
    CustomerReceptionItems AS
    (
        SELECT
            [Id],
            [CustomerId],
            JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
            [CustomerGuestAppReceptionCategoryId],
            JSON_VALUE(JsonData, '$.Name') AS [Name],
            JSON_VALUE(JsonData, '$.ItemsMonth') AS [ItemsMonth],
            JSON_VALUE(JsonData, '$.ItemsDay') AS [ItemsDay],
            JSON_VALUE(JsonData, '$.ItemsMinute') AS [ItemsMinute],
            JSON_VALUE(JsonData, '$.ItemsHour') AS [ItemsHour],
            JSON_VALUE(JsonData, '$.QuantityBar') AS [QuantityBar],
            JSON_VALUE(JsonData, '$.ItemLocation') AS [ItemLocation],
            JSON_VALUE(JsonData, '$.Comment') AS [Comment],
            JSON_VALUE(JsonData, '$.IsPriceEnable') AS [IsPriceEnable],
            JSON_VALUE(JsonData, '$.Price') AS [Price],
            JSON_VALUE(JsonData, '$.Currency') AS [Currency],
            JSON_VALUE(JsonData, '$.IsActive') AS [IsActive],
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
        FROM [dbo].[CustomerGuestAppReceptionItems] items (NOLOCK)
		WHERE [items].[DeletedAt] IS NULL
		AND [CustomerGuestAppBuilderId] = @AppBuilderId
        AND ISJSON(JsonData) = 1
		AND (@UserType = 2)

        UNION ALL 

        SELECT
            [Id],
            [CustomerId],
            [CustomerGuestAppBuilderId],
            [CustomerGuestAppReceptionCategoryId],
            [Name],
            [ItemsMonth],
            [ItemsDay],
            [ItemsMinute],
            [ItemsHour],
            [QuantityBar],
            [ItemLocation],
            [Comment],
            [IsPriceEnable],
            [Price],
            [Currency],
            [IsActive],
            [DisplayOrder],
			@ISDELETE As [IsDeleted]
        FROM [dbo].[CustomerGuestAppReceptionItems] items (NOLOCK)
		WHERE [items].[DeletedAt] IS NULL
		AND [CustomerGuestAppBuilderId] = @AppBuilderId
		AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
    )

    SELECT (
        SELECT
            category.[Id],
            category.[CustomerId],
            category.[CustomerGuestAppBuilderId],
            category.[CategoryName],
            category.[DisplayOrder],
            category.[IsActive],
			category.[IsDeleted],
            (
                SELECT
                    items.[Id],
                    items.[CustomerId],
                    items.[CustomerGuestAppBuilderId],
                    items.[CustomerGuestAppReceptionCategoryId],
                    items.[Name],
                    items.[ItemsMonth],
                    items.[ItemsDay],
                    items.[ItemsMinute],
                    items.[ItemsHour],
                    items.[QuantityBar],
                    items.[ItemLocation],
                    items.[Comment],
                    items.[IsPriceEnable],
                    items.[Price],
                    items.[Currency],
                    items.[IsActive],
                    items.[DisplayOrder],
					items.[IsDeleted]
                FROM CustomerReceptionItems items
                WHERE items.[CustomerGuestAppReceptionCategoryId] = category.[Id]
				AND items.IsDeleted != 1
				Order by items.[DisplayOrder] ASC
                FOR JSON PATH
            ) AS [CustomerReceptionItems]
        FROM Customer_Reception_Results category
		Where category.IsDeleted != 1
		Order by category.[DisplayOrder] ASC
		FOR JSON PATH
            ) AS [CustomerReceptionWithRelationOut]
		OPTION (RECOMPILE);

END");

            #endregion

            #region GetCustomersEnhanceYourStay

            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomersEnhanceYourStay]
(
	@BuilderId INT = 0,
	@UserType INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	Declare @ISDELETE BIT = 0;

	 WITH Customer_EnhanceYourStay_Results AS
    (
		SELECT
				JSON_VALUE(JsonData, '$.CategoryId') AS [Id],
				JSON_VALUE(JsonData, '$.CustomerId') AS [CustomerId],
				JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
				JSON_VALUE(JsonData, '$.Name') AS [CategoryName],
				JSON_VALUE(JsonData, '$.CategoryDisplayOrder') AS [DisplayOrder],
				JSON_VALUE(JsonData, '$.IsActive') AS [IsActive],
				JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
			FROM [dbo].[CustomerGuestAppEnhanceYourStayCategories] categories1
			WHERE ISJSON(JsonData) = 1
				AND [DeletedAt] IS NULL
				AND [CustomerGuestAppBuilderId] = @BuilderId
				AND (@UserType = 2)

			UNION ALL

			SELECT
				[Id],
				[CustomerId],
				[CustomerGuestAppBuilderId],
				[CategoryName],
				[DisplayOrder],
				[IsActive],
				@ISDELETE As [IsDeleted]
			FROM [dbo].[CustomerGuestAppEnhanceYourStayCategories] categories
			WHERE [DeletedAt] IS NULL
				AND [CustomerGuestAppBuilderId] = @BuilderId
				AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1))  
	),
	CustomerEnhanceYourStayItems AS
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
		AND [CustomerGuestAppBuilderId] = @BuilderId
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
			@ISDELETE As [IsDeleted]
        FROM [dbo].[CustomerGuestAppEnhanceYourStayItems] items (NOLOCK)
		WHERE [items].[DeletedAt] IS NULL
		AND [CustomerGuestAppBuilderId] = @BuilderId
		AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
    ), 
	Customer_EnhanceYourStay_Item_Extra AS
    (
		SELECT
				ItemImage1.[Id],
				JSON_VALUE(ItemImage1.JsonData, '$.CustomerGuestAppEnhanceYourStayItemId') AS [CustomerGuestAppEnhanceYourStayItemId],
				JSON_VALUE(ItemImage1.JsonData, '$.ItemsImages') AS  [ItemsImages],
				JSON_VALUE(ItemImage1.JsonData, '$.DisaplayOrder') AS [DisaplayOrder],
				JSON_VALUE(ItemImage1.JsonData, '$.IsActive') AS [IsActive],
				JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
			FROM [dbo].[CustomerGuestAppEnhanceYourStayItemsImages] ItemImage1
			WHERE ISJSON(ItemImage1.JsonData) = 1
				AND ItemImage1.[DeletedAt] IS NULL
				AND (@UserType = 2)

			UNION ALL

			SELECT
				ItemImage.[Id],
				ItemImage.[CustomerGuestAppEnhanceYourStayItemId],
				ItemImage.[ItemsImages],
				ItemImage.[DisaplayOrder],
				ItemImage.[IsActive],
				@ISDELETE As [IsDeleted]
			FROM [dbo].[CustomerGuestAppEnhanceYourStayItemsImages] ItemImage
			WHERE ItemImage.[DeletedAt] IS NULL
				AND ((@UserType = 2 AND ItemImage.JsonData IS NULL) OR (@UserType = 3 AND ItemImage.IsPublish = 1))  
	)
	SELECT (
        SELECT
            ccr.[Id],
            ccr.[CustomerId],
            ccr.[CustomerGuestAppBuilderId],
            ccr.[CategoryName],
            ccr.[DisplayOrder],
            ccr.[IsActive],
			ccr.[IsDeleted],
            (
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
                    cci.[DisplayOrder],
					cci.[IsDeleted],
					(
						Select 
							CEII.[Id],
							CEII.[CustomerGuestAppEnhanceYourStayItemId],
							CEII.[ItemsImages],
							CEII.[DisaplayOrder],
							CEII.[IsActive],
							CEII.[IsDeleted]
							FROM Customer_EnhanceYourStay_Item_Extra CEII
							WHERE CEII.[CustomerGuestAppEnhanceYourStayItemId] = cci.[Id]
							AND CEII.IsDeleted != 1
							ORDER BY CEII.[DisaplayOrder] ASC
							FOR JSON PATH
 					) AS [customerGuestAppEnhanceYourStayItemsImages]
                FROM CustomerEnhanceYourStayItems cci
                WHERE cci.[CustomerGuestAppBuilderCategoryId] = ccr.[Id]
				AND cci.IsDeleted != 1
				Order by cci.[DisplayOrder] ASC
                FOR JSON PATH
            ) AS [customerGuestAppEnhanceYourStayItems]
        FROM Customer_EnhanceYourStay_Results ccr
		Where ccr.IsDeleted != 1
		Order by ccr.[DisplayOrder] ASC
		FOR JSON PATH
            ) AS [CustomersEnhanceYourStayCategoriesOut]
		OPTION (RECOMPILE);
END");
            #endregion

            #region GetCustomersEnhanceYourStayCategoryItemById

            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomersEnhanceYourStayCategoryItemById]
(
	@Id INT = 0,
	@UserType INT = 0
)
AS
BEGIN
	SET NOCOUNT ON;
    SET XACT_ABORT ON;

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
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder]
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
            [DisplayOrder]
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
				JSON_VALUE(ItemImage1.JsonData, '$.IsActive') AS [IsActive]
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
				ItemImage.[IsActive]
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
				JSON_VALUE(ItemExtra1.JsonData, '$.OptionValues') AS [OptionValues]
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
                 ItemExtra.[OptionValues]
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
							FOR JSON PATH
 					) AS [CustomerEnhanceYourStayCategoryItemsExtra]
                FROM CustomerEnhanceYourStayItems cci
				Order by cci.[DisplayOrder] ASC
                FOR JSON PATH
            ) AS [CustomerEnhanceYourStayItemByIdOut]
        
		OPTION (RECOMPILE);
END
");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

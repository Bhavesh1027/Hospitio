using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetCustomersEnhanceYourStay_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomersEnhanceYourStay]
(
	@BuilderId INT = 0,
	@UserType INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

	 WITH Customer_EnhanceYourStay_Results AS
    (
		SELECT
				JSON_VALUE(JsonData, '$.CategoryId') AS [Id],
				JSON_VALUE(JsonData, '$.CustomerId') AS [CustomerId],
				JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
				JSON_VALUE(JsonData, '$.Name') AS [CategoryName],
				JSON_VALUE(JsonData, '$.CategoryDisplayOrder') AS [DisplayOrder],
				JSON_VALUE(JsonData, '$.IsActive') AS [IsActive]
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
				[IsActive]
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
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder]
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
            [DisplayOrder]
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
				JSON_VALUE(ItemImage1.JsonData, '$.IsActive') AS [IsActive]
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
				ItemImage.[IsActive]
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
                    cci.[DisplayOrder]
					,
					(
						Select 
							CEII.[Id],
							CEII.[CustomerGuestAppEnhanceYourStayItemId],
							CEII.[ItemsImages],
							CEII.[DisaplayOrder],
							CEII.[IsActive]
							FROM Customer_EnhanceYourStay_Item_Extra CEII
							WHERE CEII.[CustomerGuestAppEnhanceYourStayItemId] = cci.[Id]
							ORDER BY CEII.[DisaplayOrder] ASC
							FOR JSON PATH
 					) AS [customerGuestAppEnhanceYourStayItemsImages]
                FROM CustomerEnhanceYourStayItems cci
                WHERE cci.[CustomerGuestAppBuilderCategoryId] = ccr.[Id]
				Order by cci.[DisplayOrder] ASC
                FOR JSON PATH
            ) AS [customerGuestAppEnhanceYourStayItems]
        FROM Customer_EnhanceYourStay_Results ccr
		Order by ccr.[DisplayOrder] ASC
		FOR JSON PATH
            ) AS [CustomersEnhanceYourStayCategoriesOut]
		OPTION (RECOMPILE);
END");

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
				JSON_VALUE(ItemExtra1.JsonData, '$.CustomerGuestAppEnhanceYourStayItemId') AS [CustomerGuestAppEnhanceYourStayItemId],
				JSON_VALUE(ItemExtra1.JsonData, '$.QueType') AS  [QueType],
				JSON_VALUE(ItemExtra1.JsonData, '$.Questions') AS [Questions],
				JSON_VALUE(ItemExtra1.JsonData, '$.OptionValues') AS [OptionValues],
				JSON_VALUE(ItemExtra1.JsonData, '$.IsActive') AS [IsActive]
			FROM [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras] ItemExtra1 (NOLOCK)
			WHERE ISJSON(ItemExtra1.JsonData) = 1
				AND ItemExtra1.[DeletedAt] IS NULL
				AND (@UserType = 2)

			UNION ALL

			SELECT
				 ItemExtra.[Id],
                 ItemExtra.[CustomerGuestAppEnhanceYourStayItemId],
                 ItemExtra.[QueType],
                 ItemExtra.[Questions],
                 ItemExtra.[OptionValues],
                 ItemExtra.[IsActive]
			FROM [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras] ItemExtra (NOLOCK)
			WHERE ItemExtra.[DeletedAt] IS NULL
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
                              CEIE.[CustomerGuestAppEnhanceYourStayItemId],
                              CEIE.[QueType],
                              CEIE.[Questions],
                              CEIE.[OptionValues],
                              CEIE.[IsActive]
							FROM Customer_EnhanceYourStay_Item_Extra CEIE
							WHERE CEIE.[CustomerGuestAppEnhanceYourStayItemId] = cci.[Id]
							FOR JSON PATH
 					) AS [CustomerEnhanceYourStayCategoryItemsExtra]
                FROM CustomerEnhanceYourStayItems cci
				Order by cci.[DisplayOrder] ASC
                FOR JSON PATH
            ) AS [CustomerEnhanceYourStayItemByIdOut]
        
		OPTION (RECOMPILE);
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

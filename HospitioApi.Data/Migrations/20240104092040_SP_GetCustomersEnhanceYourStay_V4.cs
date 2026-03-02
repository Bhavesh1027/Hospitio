using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetCustomersEnhanceYourStay_V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomersEnhanceYourStay] -- 74,2
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
				AND ((@UserType = 2 AND ItemImage.JsonData IS NULL) OR (@UserType = 3))  
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

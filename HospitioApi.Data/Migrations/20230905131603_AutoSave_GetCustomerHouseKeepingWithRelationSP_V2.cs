using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AutoSave_GetCustomerHouseKeepingWithRelationSP_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomerHouseKeepingWithRelationSP] 
(
    @AppBuilderId INT = 0,
	@UserType INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

	WITH Customer_Housekeeping_Results AS
    (
        SELECT
            [Id],
            JSON_VALUE(JsonData, '$.CustomerId') AS [CustomerId],
            JSON_VALUE(JsonData, '$.CustomerGuestAppBuilderId') AS [CustomerGuestAppBuilderId],
            JSON_VALUE(JsonData, '$.CategoryName') AS [CategoryName],
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder],
            JSON_VALUE(JsonData, '$.IsActive') AS [IsActive]
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
            [IsActive]
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
            JSON_VALUE(JsonData, '$.DisplayOrder') AS [DisplayOrder]
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
            [DisplayOrder]
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
                    items.[DisplayOrder]
                FROM CustomerHousekeepingItems items
                WHERE items.[CustomerGuestAppHousekeepingCategoryId] = category.[Id]
				Order by items.[DisplayOrder] ASC
                FOR JSON PATH
            ) AS [CustomerHouseKeepingItems]
        FROM Customer_Housekeeping_Results category
		Order by category.[DisplayOrder] ASC
		FOR JSON PATH
            ) AS [CustomerHouseKeepingWithRelationOut]
		OPTION (RECOMPILE);

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

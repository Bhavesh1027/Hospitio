using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModificationGetCustomerConciergeWithRelationSP_V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER   PROC [dbo].[GetCustomerConciergeWithRelationSP]
(
    @AppBuilderId INT = 0,
	@UserType INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    WITH Customer_Concierge_Results
    AS (SELECT
            (
                SELECT [Id],
                       [CustomerId],
                       [CustomerGuestAppBuilderId],
                       [CategoryName],
                       [DisplayOrder],
                       [IsActive],
                       (
                           SELECT [Id],
                                  [CustomerId],
                                  [CustomerGuestAppBuilderId],
                                  [CustomerGuestAppConciergeCategoryId],
                                  [CategoryName],
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
                           FROM [dbo].[CustomerGuestAppConciergeItems] items (NOLOCK)
                           WHERE [items].[CustomerGuestAppConciergeCategoryId] = [categories].[Id]
                                 AND [items].[DeletedAt] IS NULL
								 AND (
									  @UserType = 2
									  OR (
										 @UserType = 3
										 AND ([IsPublish] = 1)
										 )
								  )
                           ORDER BY [DisplayOrder] ASC
                           FOR JSON PATH
                       ) AS [CustomerConciergeItems]
                FROM [dbo].[CustomerGuestAppConciergeCategories] categories (NOLOCK)
                WHERE [DeletedAt] IS NULL
                      AND [CustomerGuestAppBuilderId] = @AppBuilderId
					  AND (
							@UserType = 2
							OR (
								@UserType = 3
								AND ([IsPublish] = 1)
								)
						)
                ORDER BY [DisplayOrder] ASC
                FOR JSON PATH
            ) AS [CustomerConciergeWithRelationOut]
       )
    SELECT * FROM Customer_Concierge_Results
    OPTION (RECOMPILE)
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

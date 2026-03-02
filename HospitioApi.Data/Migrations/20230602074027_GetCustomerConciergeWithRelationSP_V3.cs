using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerConciergeWithRelationSP_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerConciergeWithRelationSP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerConciergeWithRelationSP]    Script Date: 02-06-2023 13:07:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROC [dbo].[GetCustomerConciergeWithRelationSP] 
(
	@CustomerId Int=0,
	@AppBuilderId int =0
)
AS BEGIN
	SET NOCOUNT ON;
	
	; WITH Customer_Concierge_Results AS
    (
		SELECT 
		( 
			SELECT [Id],[CustomerId],[CustomerGuestAppBuilderId],[CategoryName],[DisplayOrder],[IsActive],
				(SELECT [Id]
					,[CustomerId]
					,[CustomerGuestAppBuilderId]
					,[CustomerGuestAppConciergeCategoryId]
					,[CategoryName]
					,[Name]
					,[ItemsMonth]
					,[ItemsDay]
					,[ItemsMinute]
					,[ItemsHour]
					,[QuantityBar]
					,[ItemLocation]
					,[Comment]
					,[IsPriceEnable]
					,[Price]
					,[Currency]
					,[IsActive]
					,[DisplayOrder]
				FROM [dbo].[CustomerGuestAppConciergeItems] items
				WHERE items.[CustomerGuestAppConciergeCategoryId] = categories.Id
				AND items.DeletedAt is null 
				ORDER BY DisplayOrder ASC
				FOR JSON PATH) AS CustomerConciergeItems
			FROM [dbo].[CustomerGuestAppConciergeCategories] categories
			WHERE DeletedAt is null 
			AND CustomerId = @CustomerId
			AND CustomerGuestAppBuilderId = @AppBuilderId
			ORDER BY DisplayOrder ASC

		FOR JSON PATH) AS CustomerConciergeWithRelationOut 
    )

	SELECT *
	FROM Customer_Concierge_Results
	OPTION (RECOMPILE)
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

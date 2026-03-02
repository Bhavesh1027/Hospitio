using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerHouseKeepingWithRelationSP_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			#region GetCustomerHouseKeepingWithRelationSP

			migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerHouseKeepingWithRelationSP]    Script Date: 02-06-2023 12:22:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomerHouseKeepingWithRelationSP]
(
	@CustomerId Int=0,
	@AppBuilderId int = 0
)
AS BEGIN
	SET NOCOUNT ON;
	
	; WITH Customer_House_keeping_Results AS
    (
		SELECT ( 
            SELECT [Id],[CustomerId],[CustomerGuestAppBuilderId],[CategoryName],[DisplayOrder],[IsActive],
				(SELECT [Id]
				  ,[CustomerId]
				  ,[CustomerGuestAppBuilderId]
				  ,[CustomerGuestAppHousekeepingCategoryId]
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
                FROM [dbo].[CustomerGuestAppHousekeepingItems] items
                WHERE items.CustomerGuestAppHousekeepingCategoryId = categories.Id
				AND items.DeletedAt is null
				ORDER BY items.DisplayOrder ASC
                FOR JSON PATH) AS CustomerHouseKeepingItems
		FROM [dbo].[CustomerGuestAppHousekeepingCategories] categories
		WHERE DeletedAt is null AND categories.CustomerId = @CustomerId 
		AND categories.CustomerGuestAppBuilderId = @AppBuilderId
		ORDER BY DisplayOrder ASC
		FOR JSON PATH) AS CustomerHouseKeepingWithRelationOut
	)

	SELECT *
	FROM Customer_House_keeping_Results
	OPTION (RECOMPILE)
	
END
");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerReceptionWithRelationSP_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			#region  GetCustomerReceptionWithRelationSP

			migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerReceptionWithRelationSP]    Script Date: 01-06-2023 15:12:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomerReceptionWithRelationSP]
(
@CustomerId int = 0,
@AppBuilderId int = 0
)
AS BEGIN
	SET NOCOUNT ON;
	
	; WITH Customer_Reception_Results AS
    (
		SELECT 
		( 
			SELECT [Id],[CustomerId],[CustomerGuestAppBuilderId],[CategoryName],[DisplayOrder],[IsActive],
				(SELECT [Id]
					,[CustomerId]
					,[CustomerGuestAppBuilderId]
					,[CustomerGuestAppReceptionCategoryId]
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
				FROM [dbo].[CustomerGuestAppReceptionItems] items
				WHERE items.CustomerGuestAppReceptionCategoryId = categories.Id
				AND items.DeletedAt is null 
				ORDER BY DisplayOrder ASC
				FOR JSON PATH) AS CustomerReceptionItems
			FROM [dbo].[CustomerGuestAppReceptionCategories] categories
			WHERE categories.DeletedAt is null 
			AND categories.CustomerId = @CustomerId
			AND categories.CustomerGuestAppBuilderId = @AppBuilderId
			ORDER BY DisplayOrder ASC
		FOR JSON PATH) AS CustomerReceptionWithRelationOut 
    )

	SELECT *
	FROM Customer_Reception_Results
	OPTION (RECOMPILE)
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

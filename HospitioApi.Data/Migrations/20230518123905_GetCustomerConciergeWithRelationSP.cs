using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerConciergeWithRelationSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerConciergeWithRelationSP]    Script Date: 24-05-2023 11:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROC [dbo].[GetCustomerConciergeWithRelationSP] --'',1,2,'CategoryName','ASC',1
(
    @SearchValue NVARCHAR(50) = null,
    @PageNo Int=1,
	@PageSize Int=10, --NoOf Record To Get
	@SortColumn NVARCHAR(20) = 'DisplayOrder',
    @SortOrder NVARCHAR(5) = 'ASC',
	@CustomerId Int=1
)
AS BEGIN
	SET NOCOUNT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
	
	; WITH Customer_Concierge_Results AS
    (
		SELECT 
		( 
			SELECT [Id],[CustomerId],[CustomerGuestAppBuilderId],[CategoryName],[DisplayOrder],
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
					,[PickupLocation]
					,[DestinationLocation]
					,[Comment]
					,[IsPriceEnable]
					,[Price]
					,[Currency]
					,[IsActive]
					,[DisplayOrder]
				FROM [dbo].[CustomerGuestAppConciergeItems] items
				WHERE items.[CustomerGuestAppConciergeCategoryId] = categories.Id
				ORDER BY DisplayOrder ASC
				FOR JSON PATH) AS CustomerConciergeItems
			FROM [dbo].[CustomerGuestAppConciergeCategories] categories
			WHERE DeletedAt is null AND CustomerId = @CustomerId AND (
			CategoryName LIKE '%' + @SearchValue + '%' )

			ORDER BY
			CASE WHEN (@SortColumn = 'DisplayOrder' AND @SortOrder='ASC')
			THEN DisplayOrder
			END ASC,

			CASE WHEN (@SortColumn = 'DisplayOrder' AND @SortOrder='DESC')
			THEN DisplayOrder
			END DESC,

			CASE WHEN (@SortColumn = 'CategoryName' AND @SortOrder='ASC')
			THEN CategoryName
			END ASC,

			CASE WHEN (@SortColumn = 'CategoryName' AND @SortOrder='DESC')
			THEN CategoryName
			END DESC
       
			OFFSET @PageSize * (@PageNo - 1) ROWS
			FETCH NEXT @PageSize ROWS ONLY

		FOR JSON PATH) AS CustomerConciergeWithRelationOut 
    )

	SELECT *
	FROM Customer_Concierge_Results
	OPTION (RECOMPILE)
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

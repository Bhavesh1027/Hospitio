using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerReceptionWithRelationSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

GO
/****** Object:  StoredProcedure [dbo].[GetCustomerReceptionWithRelationSP]    Script Date: 23-05-2023 16:58:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomerReceptionWithRelationSP] --'',1,2,'CategoryName','ASC',1
(
    @SearchValue NVARCHAR(50) = null,
    @PageNo Int=1,
	@PageSize Int=10, --NoOf Record To Get
	@SortColumn NVARCHAR(20) = 'CategoryName',
    @SortOrder NVARCHAR(5) = 'ASC',
	@CustomerId Int=1
)
AS BEGIN
	SET NOCOUNT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
	
	; WITH Customer_Reception_Results AS
    (
		SELECT 
		( 
			SELECT [Id],[CustomerId],[CustomerGuestAppBuilderId],[CategoryName],
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
					,[PickupLocation]
					,[DestinationLocation]
					,[Comment]
					,[IsPriceEnable]
					,[Price]
					,[Currency]
					,[IsActive]
				FROM [dbo].[CustomerGuestAppReceptionItems] items
				WHERE items.CustomerGuestAppReceptionCategoryId = categories.Id				
				FOR JSON PATH) AS CustomerReceptionItems
			FROM [dbo].[CustomerGuestAppReceptionCategories] categories
			WHERE DeletedAt is null AND CustomerId = @CustomerId AND (
			CategoryName LIKE '%' + @SearchValue + '%' )

			ORDER BY

	        CASE WHEN (@SortColumn = 'CategoryName' AND @SortOrder='ASC')
			THEN CategoryName
			END ASC,

			CASE WHEN (@SortColumn = 'CategoryName' AND @SortOrder='DESC')
			THEN CategoryName
			END DESC
			OFFSET @PageSize * (@PageNo - 1) ROWS
			FETCH NEXT @PageSize ROWS ONLY

		FOR JSON PATH) AS CustomerReceptionWithRelationOut 
    )

	SELECT *
	FROM Customer_Reception_Results
	OPTION (RECOMPILE)
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerRoomServiceSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerRoomServiceWithRelationSP]    Script Date: 31-05-2023 14:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomerRoomServiceWithRelationSP] --'',1,2,'CategoryName','ASC',1
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
	
	; WITH Customer_RoomService_Results AS
    (
		SELECT 
		( 
			SELECT [Id],[CustomerId],[CustomerGuestAppBuilderId],[CategoryName],[DisplayOrder],
				(SELECT [Id]
					,[CustomerId]
					,[CustomerGuestAppBuilderId]
					,[CustomerGuestAppRoomServiceCategoryId]
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
				FROM [dbo].[CustomerGuestAppRoomServiceItems] items
				WHERE items.[CustomerGuestAppRoomServiceCategoryId] = categories.Id
				ORDER BY DisplayOrder ASC
				FOR JSON PATH) AS CustomerRoomServiceItems
			FROM [dbo].[CustomerGuestAppRoomServiceCategories] categories
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

		FOR JSON PATH) AS CustomerRoomServiceWithRelationOut 
    )

	SELECT *
	FROM Customer_RoomService_Results
	OPTION (RECOMPILE)
END

");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

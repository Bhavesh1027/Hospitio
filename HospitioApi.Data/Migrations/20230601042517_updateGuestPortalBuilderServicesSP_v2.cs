using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class updateGuestPortalBuilderServicesSP_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerReceptionWithRelationSP]    Script Date: 01-06-2023 09:55:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetCustomerReceptionWithRelationSP] --'',1,2,'CategoryName','ASC',1
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
	
	; WITH Customer_Reception_Results AS
    (
		SELECT 
		( 
			SELECT [Id],[CustomerId],[CustomerGuestAppBuilderId],[CategoryName],[DisplayOrder],
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
				ORDER BY DisplayOrder ASC
				FOR JSON PATH) AS CustomerReceptionItems
			FROM [dbo].[CustomerGuestAppReceptionCategories] categories
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

		FOR JSON PATH) AS CustomerReceptionWithRelationOut 
    )

	SELECT *
	FROM Customer_Reception_Results
	OPTION (RECOMPILE)
END
");

			migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerHouseKeepingWithRelationSP]    Script Date: 01-06-2023 09:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROC [dbo].[GetCustomerHouseKeepingWithRelationSP] --'',1,2,'CategoryName','ASC',1
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
	
	; WITH Customer_House_keeping_Results AS
    (
		SELECT ( 
            SELECT [Id],[CustomerId],[CustomerGuestAppBuilderId],[CategoryName],[DisplayOrder],
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
				ORDER BY DisplayOrder ASC
                FOR JSON PATH) AS CustomerHouseKeepingItems
		FROM [dbo].[CustomerGuestAppHousekeepingCategories] categories
		WHERE DeletedAt is null AND CustomerId = @CustomerId AND (CategoryName LIKE '%' + @SearchValue + '%') 

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
		FOR JSON PATH) AS CustomerHouseKeepingWithRelationOut
	)

	SELECT *
	FROM Customer_House_keeping_Results
	OPTION (RECOMPILE)
	
END
");
			migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerRoomServiceWithRelationSP]    Script Date: 01-06-2023 09:52:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetCustomerRoomServiceWithRelationSP] --'',1,2,'CategoryName','ASC',1
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
					,[ItemLocation]
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

			migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerConciergeWithRelationSP]    Script Date: 01-06-2023 09:58:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROC [dbo].[GetCustomerConciergeWithRelationSP] --'',1,2,'CategoryName','ASC',1
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
					,[ItemLocation]
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
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

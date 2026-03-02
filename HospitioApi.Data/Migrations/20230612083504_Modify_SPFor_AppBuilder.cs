using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_SPFor_AppBuilder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region  GetCustomerConciergeWithRelationSP
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerConciergeWithRelationSP]    Script Date: 12-06-2023 14:03:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomerConciergeWithRelationSP] 
(
	--@CustomerId Int=0,
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
			--AND CustomerId = @CustomerId
			AND CustomerGuestAppBuilderId = @AppBuilderId
			ORDER BY DisplayOrder ASC

		FOR JSON PATH) AS CustomerConciergeWithRelationOut 
    )

	SELECT *
	FROM Customer_Concierge_Results
	OPTION (RECOMPILE)
END
");
            #endregion

            #region GetCustomerHouseKeepingWithRelationSP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerHouseKeepingWithRelationSP]    Script Date: 12-06-2023 14:07:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetCustomerHouseKeepingWithRelationSP]
(
	--@CustomerId Int=0,
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
		WHERE DeletedAt is null 
		--AND categories.CustomerId = @CustomerId 
		AND categories.CustomerGuestAppBuilderId = @AppBuilderId
		FOR JSON PATH) AS CustomerHouseKeepingWithRelationOut
	)

	SELECT *
	FROM Customer_House_keeping_Results
	OPTION (RECOMPILE)
	
END

");
            #endregion

			# region GetCustomerReceptionWithRelationSP
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerReceptionWithRelationSP]    Script Date: 12-06-2023 14:08:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetCustomerReceptionWithRelationSP] 
(
--@CustomerId int = 0,
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
			--AND categories.CustomerId = @CustomerId
			AND categories.CustomerGuestAppBuilderId = @AppBuilderId
		FOR JSON PATH) AS CustomerReceptionWithRelationOut 
    )

	SELECT *
	FROM Customer_Reception_Results
	OPTION (RECOMPILE)
END
");
            #endregion

            #region GetCustomerRoomServiceWithRelationSP
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerRoomServiceWithRelationSP]    Script Date: 12-06-2023 14:10:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetCustomerRoomServiceWithRelationSP] --'',1,2,'CategoryName','ASC',1
(
	--@CustomerId Int=0,
	@AppBuilderId int =0
)
AS BEGIN
	SET NOCOUNT ON;
	
	; WITH Customer_RoomService_Results AS
    (
		SELECT 
		( 
			SELECT [Id],[CustomerId],[CustomerGuestAppBuilderId],[CategoryName],[DisplayOrder],[IsActive],
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
				AND DeletedAt is null 
				ORDER BY DisplayOrder ASC
				FOR JSON PATH) AS CustomerRoomServiceItems
			FROM [dbo].[CustomerGuestAppRoomServiceCategories] categories
			WHERE DeletedAt is null 
			--AND CustomerId = @CustomerId
			AND CustomerGuestAppBuilderId = @AppBuilderId
			ORDER BY DisplayOrder ASC
		FOR JSON PATH) AS CustomerRoomServiceWithRelationOut 
    )

	SELECT *
	FROM Customer_RoomService_Results
	OPTION (RECOMPILE)
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

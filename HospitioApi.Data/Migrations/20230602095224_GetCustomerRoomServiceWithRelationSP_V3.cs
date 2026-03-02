using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerRoomServiceWithRelationSP_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerRoomServiceWithRelationSP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerRoomServiceWithRelationSP]    Script Date: 02-06-2023 15:15:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomerRoomServiceWithRelationSP] --'',1,2,'CategoryName','ASC',1
(
	@CustomerId Int=0,
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
			AND CustomerId = @CustomerId
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

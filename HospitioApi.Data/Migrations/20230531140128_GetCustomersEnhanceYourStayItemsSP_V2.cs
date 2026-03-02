using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomersEnhanceYourStayItemsSP_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomersEnhanceYourStayCategoryItemById]    Script Date: 31-05-2023 19:31:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Or ALTER   PROC [dbo].[GetCustomersEnhanceYourStayCategoryItemById] 
    @Id Int=1	
AS 
	SELECT (
		SELECT [Id]
		  ,[CustomerGuestAppBuilderId]
		  ,[CustomerId]
		  ,[CustomerGuestAppBuilderCategoryId]
		  ,[Badge]
		  ,[ShortDescription]
		  ,[LongDescription]
		  ,[ButtonType]
		  ,[ButtonText]
		  ,[ChargeType]
		  ,[Discount]
		  ,[Price]
		  ,[Currency]
		  ,[DisplayOrder]
		  ,[IsActive],
			(SELECT [Id]
				,[CustomerGuestAppEnhanceYourStayItemId]
				,[ItemsImages]
				,[DisaplayOrder]
				,[IsActive]
				,[CreatedAt]
				,[UpdateAt]
				,[DeletedAt]
				,[CreatedBy]
			FROM [dbo].[CustomerGuestAppEnhanceYourStayItemsImages] images
			WHERE  DeletedAt is null AND images.CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItems.Id 
			FOR JSON PATH) AS ItemsImages,
			(SELECT [Id]
					,[CustomerGuestAppEnhanceYourStayItemId]
					,[QueType]
					,[Questions]
					,[OptionValues]
					,[IsActive]
					,[CreatedAt]
					,[UpdateAt]
					,[DeletedAt]
					,[CreatedBy]
				FROM [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras] extraDetails
				WHERE  DeletedAt is null AND extraDetails.CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItems.Id 
				FOR JSON PATH) AS CustomerEnhanceYourStayCategoryItemsExtra
		FROM   [dbo].[CustomerGuestAppEnhanceYourStayItems]  AS CustomerGuestAppEnhanceYourStayItems
		WHERE DeletedAt is null AND Id = @Id 
		FOR JSON PATH) AS CustomerEnhanceYourStayItemByIdOut
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

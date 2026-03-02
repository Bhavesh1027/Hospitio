using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_GetCustomerRoomNames_v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerRoomNames]    Script Date: 10/05/2023 7:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER          PROCEDURE [dbo].[GetCustomerRoomNames] --20
(
	@CustomerId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON


    SELECT [R].[Id], 
           [R].[Name], 
           [BT].BizType,
           CASE
               WHEN ( 
                    NOT EXISTS (SELECT 1 FROM CustomerGuestAppConciergeCategories WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null)
                        AND NOT  EXISTS (SELECT 1 FROM CustomerGuestAppEnhanceYourStayCategories WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null )
                        AND NOT  EXISTS (SELECT 1 FROM CustomerGuestAppHousekeepingCategories WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null)
                        AND NOT  EXISTS (SELECT 1 FROM CustomerGuestAppReceptionCategories WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null )
                        AND NOT  EXISTS (SELECT 1 FROM CustomerGuestAppRoomServiceCategories WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null)
                        AND NOT  EXISTS (SELECT 1 FROM CustomerPropertyEmergencyNumbers WHERE CustomerPropertyInformationId = [CP].[Id] and DeletedAt  is null)
				        AND ( EXISTS (SELECT 1 FROM CustomerPropertyInformations WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null and IsPublish IS NULL and JsonData IS null)
						       OR NOT  EXISTS (SELECT 1 FROM CustomerPropertyInformations WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null))
                        AND NOT  EXISTS (SELECT 1 FROM CustomerPropertyExtras WHERE CustomerPropertyInformationId = [CP].[Id] and DeletedAt  is null )
                        AND NOT  EXISTS (SELECT 1 FROM CustomerPropertyGalleries WHERE CustomerPropertyInformationId = [CP].[Id] and DeletedAt  is null )
                        AND NOT  EXISTS (SELECT 1 FROM CustomerPropertyServices WHERE CustomerPropertyInformationId = [CP].[Id] and DeletedAt  is null )
                    )
               THEN 3
               WHEN (EXISTS (SELECT 1 FROM CustomerGuestAppConciergeCategories category WHERE  category.CustomerGuestAppBuilderId= [G].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) ) AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerGuestAppEnhanceYourStayCategories category WHERE category.CustomerGuestAppBuilderId= [G].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerGuestAppHousekeepingCategories category WHERE category.CustomerGuestAppBuilderId= [G].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerGuestAppReceptionCategories category WHERE category.CustomerGuestAppBuilderId= [G].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerGuestAppRoomServiceCategories category WHERE category.CustomerGuestAppBuilderId= [G].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerPropertyInformations category WHERE category.CustomerGuestAppBuilderId= [G].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerPropertyEmergencyNumbers category WHERE category.CustomerPropertyInformationId= [CP].[Id] AND((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerPropertyExtras category WHERE category.CustomerPropertyInformationId= [CP].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerPropertyGalleries category WHERE category.CustomerPropertyInformationId= [CP].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerPropertyServices category WHERE category.CustomerPropertyInformationId= [CP].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null ))
               THEN 2
               ELSE 1
           END AS IsWork
    FROM [dbo].[CustomerRoomNames] [R] (NOLOCK)
    INNER JOIN [dbo].[Customers] [C]
    ON [R].[CustomerId] = [C].[Id]
    LEFT JOIN [dbo].[CustomerGuestAppBuilders] [G] (NOLOCK)
    ON [G].CustomerRoomNameId = [R].Id
    LEFT JOIN [dbo].[BusinessTypes] [BT]
    ON [C].BusinessTypeId = [BT].Id
	LEFT JOIN [dbo].[CustomerPropertyInformations] [CP]
	ON [CP].CustomerGuestAppBuilderId = [G].[Id]
    WHERE [R].[DeletedAt] IS NULL
    AND ([G].[CustomerId] = @CustomerId OR [C].[Id] = @CustomerId)
	AND [G].[DeletedAt] IS NULL
	AND [CP].[DeletedAt] IS NULL
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

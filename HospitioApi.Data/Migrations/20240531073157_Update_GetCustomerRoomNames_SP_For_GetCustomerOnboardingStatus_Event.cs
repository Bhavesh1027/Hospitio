using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Update_GetCustomerRoomNames_SP_For_GetCustomerOnboardingStatus_Event : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomerRoomNames
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerRoomNames]    Script Date: 31/05/2024 11:11:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[GetCustomerRoomNames]
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
                    NOT EXISTS (SELECT 1 FROM CustomerGuestAppConciergeCategories (NOLOCK) WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null)
                        AND NOT  EXISTS (SELECT 1 FROM CustomerGuestAppEnhanceYourStayCategories (NOLOCK) WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null )
                        AND NOT  EXISTS (SELECT 1 FROM CustomerGuestAppHousekeepingCategories (NOLOCK) WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null)
                        AND NOT  EXISTS (SELECT 1 FROM CustomerGuestAppReceptionCategories (NOLOCK) WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null )
                        AND NOT  EXISTS (SELECT 1 FROM CustomerGuestAppRoomServiceCategories (NOLOCK) WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null)
                        AND NOT  EXISTS (SELECT 1 FROM CustomerPropertyEmergencyNumbers (NOLOCK) WHERE CustomerPropertyInformationId = [CP].[Id] and DeletedAt  is null)
				        AND ( EXISTS (SELECT 1 FROM CustomerPropertyInformations (NOLOCK) WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null and IsPublish IS NULL and JsonData IS null)
						       OR NOT  EXISTS (SELECT 1 FROM CustomerPropertyInformations (NOLOCK) WHERE CustomerGuestAppBuilderId = [G].[Id] and DeletedAt  is null))
                        AND NOT  EXISTS (SELECT 1 FROM CustomerPropertyExtras (NOLOCK) WHERE CustomerPropertyInformationId = [CP].[Id] and DeletedAt  is null )
                        AND NOT  EXISTS (SELECT 1 FROM CustomerPropertyGalleries (NOLOCK) WHERE CustomerPropertyInformationId = [CP].[Id] and DeletedAt  is null )
                        AND NOT  EXISTS (SELECT 1 FROM CustomerPropertyServices (NOLOCK) WHERE CustomerPropertyInformationId = [CP].[Id] and DeletedAt  is null )
                    )
               THEN 3
               WHEN (EXISTS (SELECT 1 FROM CustomerGuestAppConciergeCategories category (NOLOCK) WHERE  category.CustomerGuestAppBuilderId= [G].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) ) AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerGuestAppEnhanceYourStayCategories category (NOLOCK) WHERE category.CustomerGuestAppBuilderId= [G].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerGuestAppHousekeepingCategories category (NOLOCK) WHERE category.CustomerGuestAppBuilderId= [G].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerGuestAppReceptionCategories category (NOLOCK) WHERE category.CustomerGuestAppBuilderId= [G].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerGuestAppRoomServiceCategories category (NOLOCK) WHERE category.CustomerGuestAppBuilderId= [G].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerPropertyInformations category (NOLOCK) WHERE category.CustomerGuestAppBuilderId= [G].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerPropertyEmergencyNumbers category (NOLOCK) WHERE category.CustomerPropertyInformationId= [CP].[Id] AND((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerPropertyExtras category (NOLOCK) WHERE category.CustomerPropertyInformationId= [CP].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerPropertyGalleries category (NOLOCK) WHERE category.CustomerPropertyInformationId= [CP].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null )
					 or EXISTS (SELECT 1 FROM CustomerPropertyServices category (NOLOCK) WHERE category.CustomerPropertyInformationId= [CP].[Id] AND ((JsonData IS NULL AND IsPublish = 0)or (JsonData is not null) )AND category.DeletedAt is null ))
               THEN 2
               ELSE 1
           END AS IsWork,
		   [C].[NoOfRooms]
    FROM [dbo].[CustomerRoomNames] [R] (NOLOCK)
    INNER JOIN [dbo].[Customers] [C] (NOLOCK)
    ON [R].[CustomerId] = [C].[Id]
    LEFT JOIN [dbo].[CustomerGuestAppBuilders] [G] (NOLOCK)
    ON [G].CustomerRoomNameId = [R].Id
    LEFT JOIN [dbo].[BusinessTypes] [BT] (NOLOCK)
    ON [C].BusinessTypeId = [BT].Id
	LEFT JOIN [dbo].[CustomerPropertyInformations] [CP] (NOLOCK)
	ON [CP].CustomerGuestAppBuilderId = [G].[Id]
	   AND [CP].[DeletedAt] IS NULL
    WHERE [R].[DeletedAt] IS NULL
    AND ([G].[CustomerId] = @CustomerId OR [C].[Id] = @CustomerId)
	AND [G].[DeletedAt] IS NULL
END
");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

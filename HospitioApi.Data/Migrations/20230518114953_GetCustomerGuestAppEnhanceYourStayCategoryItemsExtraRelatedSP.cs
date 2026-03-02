using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerGuestAppEnhanceYourStayCategoryItemsExtraRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuestAppEnhanceYourStayItemsExtraByStayId]    Script Date: 18-05-2023 17:27:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROC [dbo].[GetCustomerGuestAppEnhanceYourStayItemsExtraByStayId]
    @CustomerGuestAppEnhanceYourStayItemId Int=1
	
	
AS 
SET NOCOUNT ON 
SET XACT_ABORT ON  

	SELECT dbo.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.Id
      ,dbo.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.CustomerGuestAppEnhanceYourStayItemId
      ,dbo.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.QueType
      ,dbo.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.Questions
      ,dbo.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.OptionValues
      ,dbo.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.IsActive
	FROM   [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras] WITH (NOLOCK)
	WHERE dbo.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.CustomerGuestAppEnhanceYourStayItemId = @CustomerGuestAppEnhanceYourStayItemId and dbo.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.DeletedAt is null


                "
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

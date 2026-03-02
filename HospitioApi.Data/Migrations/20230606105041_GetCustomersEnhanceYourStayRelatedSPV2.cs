using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomersEnhanceYourStayRelatedSPV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomersEnhanceYourStay]    Script Date: 06-06-2023 16:18:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   Procedure [dbo].[GetCustomersEnhanceYourStay]  --1
(
@CustomerId int=0
)
AS
SET NOCOUNT ON 
SET XACT_ABORT ON  
 
SELECT 
( 
SELECT m.[Id], m.[CustomerGuestAppBuilderId], m.[CustomerId], m.[CategoryName], m.[IsActive],m.[DisplayOrder]
,JSON_QUERY(( 
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
      ,[IsActive]
	  ,[DisplayOrder]
	  ,JSON_QUERY(( 
SELECT [Id]
      ,[CustomerGuestAppEnhanceYourStayItemId]
      ,[ItemsImages]
      ,[DisaplayOrder]
      ,[IsActive]
FROM CustomerGuestAppEnhanceYourStayItemsImages where CustomerGuestAppEnhanceYourStayItemId=n.Id and DeletedAt is null 
ORDER BY
    --CASE WHEN DisplayOrder IS NULL THEN 1 ELSE 0 END,
    DisplayOrder ASC
FOR JSON PATH
)) as [customerGuestAppEnhanceYourStayItemsImages]
FROM CustomerGuestAppEnhanceYourStayItems n where CustomerGuestAppBuilderCategoryId=m.Id and DeletedAt is null 
ORDER BY
    CASE WHEN DisplayOrder IS NULL THEN 1 ELSE 0 END,
    DisplayOrder ASC
FOR JSON PATH
)) as [customerGuestAppEnhanceYourStayItems]
FROM CustomerGuestAppEnhanceYourStayCategories m where m.DeletedAt is null and m.CustomerId = @CustomerId 
 ORDER BY
    CASE WHEN m.DisplayOrder IS NULL THEN 1 ELSE 0 END,
    m.DisplayOrder ASC
FOR JSON PATH )
                "
          );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

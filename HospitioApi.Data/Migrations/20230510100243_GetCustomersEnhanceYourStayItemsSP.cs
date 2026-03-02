using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomersEnhanceYourStayItemsSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomersEnhanceYourStayItems]    Script Date: 17-05-2023 12:26:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomersEnhanceYourStayItems] --'',1,2,'ShortDescription','ASC',1
(
    @SearchValue NVARCHAR(50) = null,
    @PageNo Int=1,
	@PageSize Int=10, --NoOf Record To Get
	@SortColumn NVARCHAR(20) = 'ShortDescription',
    @SortOrder NVARCHAR(5) = 'ASC',
	@CustomerId Int=1
)
AS BEGIN
	SET NOCOUNT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
	
	; WITH Enhance_Stay_Items_Results AS
    (
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
		  ,COUNT(*) OVER() as FilteredCount
	  FROM CustomerGuestAppEnhanceYourStayItems

        WHERE DeletedAt is null AND CustomerId = @CustomerId AND (
					Badge LIKE '%' + @SearchValue + '%' OR
					ShortDescription LIKE '%' + @SearchValue + '%' OR
					LongDescription LIKE '%' + @SearchValue + '%' OR
					ButtonType LIKE '%' + @SearchValue + '%' OR
					ButtonText LIKE '%' + @SearchValue + '%' OR
					ChargeType LIKE '%' + @SearchValue + '%' OR
					Discount LIKE '%' + @SearchValue + '%' OR
					Price LIKE '%' + @SearchValue + '%' OR
					Currency LIKE '%' + @SearchValue + '%' OR
					IsActive LIKE '%' + @SearchValue + '%') 

		ORDER BY
		CASE WHEN (@SortColumn = 'Badge' AND @SortOrder='ASC')
        THEN Badge
        END ASC,
       
		CASE WHEN (@SortColumn = 'Badge' AND @SortOrder='DESC')
        THEN Badge
        END DESC,

		CASE WHEN (@SortColumn = 'ShortDescription' AND @SortOrder='ASC')
        THEN ShortDescription
        END ASC,
       
		CASE WHEN (@SortColumn = 'ShortDescription' AND @SortOrder='DESC')
        THEN ShortDescription
        END DESC,

		CASE WHEN (@SortColumn = 'LongDescription' AND @SortOrder='ASC')
        THEN LongDescription
        END ASC,
       
		CASE WHEN (@SortColumn = 'LongDescription' AND @SortOrder='DESC')
        THEN LongDescription
        END DESC,

		CASE WHEN (@SortColumn = 'ButtonType' AND @SortOrder='ASC')
        THEN ButtonType
        END ASC,
       
		CASE WHEN (@SortColumn = 'ButtonType' AND @SortOrder='DESC')
        THEN ButtonType
        END DESC,

		CASE WHEN (@SortColumn = 'ChargeType' AND @SortOrder='ASC')
        THEN ChargeType
        END ASC,
       
		CASE WHEN (@SortColumn = 'ChargeType' AND @SortOrder='DESC')
        THEN ChargeType
        END DESC,

		CASE WHEN (@SortColumn = 'Discount' AND @SortOrder='ASC')
        THEN Discount
        END ASC,
       
		CASE WHEN (@SortColumn = 'Discount' AND @SortOrder='DESC')
        THEN Discount
        END DESC,

		CASE WHEN (@SortColumn = 'Price' AND @SortOrder='ASC')
        THEN Price
        END ASC,
       
		CASE WHEN (@SortColumn = 'Price' AND @SortOrder='DESC')
        THEN Price
        END DESC,

		CASE WHEN (@SortColumn = 'Currency' AND @SortOrder='ASC')
        THEN Currency
        END ASC,
       
		CASE WHEN (@SortColumn = 'Currency' AND @SortOrder='DESC')
        THEN Currency
        END DESC,

		CASE WHEN (@SortColumn = 'IsActive' AND @SortOrder='ASC')
        THEN IsActive
        END ASC,
       
		CASE WHEN (@SortColumn = 'IsActive' AND @SortOrder='DESC')
        THEN IsActive
        END DESC

		OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )

	select [Id]
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
	  ,FilteredCount 
	from Enhance_Stay_Items_Results
	OPTION (RECOMPILE)
	
END
            ");

            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomersEnhanceYourStayCategoryItemById]    Script Date: 17-05-2023 12:36:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomersEnhanceYourStayCategoryItemById] 
    @Id Int=1	
AS 
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
      
	FROM   [dbo].[CustomerGuestAppEnhanceYourStayItems] 
	WHERE DeletedAt is null AND Id = @Id");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

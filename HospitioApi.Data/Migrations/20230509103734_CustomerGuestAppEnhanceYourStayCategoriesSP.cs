using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CustomerGuestAppEnhanceYourStayCategoriesSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomersEnhanceYourStayCategories]    Script Date: 17-05-2023 11:26:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomersEnhanceYourStayCategories] --'',1,2,'CategoryName','ASC',1
(
    @SearchValue NVARCHAR(50) = null,
    @PageNo Int=1,
	@PageSize Int=10, --NoOf Record To Get
	@SortColumn NVARCHAR(20) = 'CategoryName',
    @SortOrder NVARCHAR(5) = 'ASC',
	@CustomerId Int=1
)
AS BEGIN
	SET NOCOUNT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
	
	; WITH Enhance_Stay_Categories_Results AS
    (
        SELECT [Id]
      ,[CustomerId]
      ,[CustomerGuestAppBuilderId]
      ,[CategoryName]
	  ,COUNT(*) OVER() as FilteredCount
	  FROM CustomerGuestAppEnhanceYourStayCategories

        WHERE DeletedAt is null AND CustomerId = @CustomerId AND (
                CategoryName LIKE '%' + @SearchValue + '%')

		ORDER BY
		CASE WHEN (@SortColumn = 'CategoryName' AND @SortOrder='ASC')
        THEN CategoryName
        END ASC,
       
		CASE WHEN (@SortColumn = 'CategoryName' AND @SortOrder='DESC')
        THEN CategoryName
        END DESC
       
		OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )

	select [Id]
      ,[CustomerId]
      ,[CustomerGuestAppBuilderId]
      ,[CategoryName]
	  ,FilteredCount 
	from Enhance_Stay_Categories_Results
	OPTION (RECOMPILE)
	
END
            ");

            migrationBuilder.Sql(@" 
GO
/****** Object:  StoredProcedure [dbo].[GetCustomersEnhanceYourStayCategoryById]    Script Date: 17-05-2023 12:11:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomersEnhanceYourStayCategoryById] 
    @Id Int=1
AS 
	SELECT [Id]
      ,[CustomerId]
      ,[CustomerGuestAppBuilderId]
      ,[CategoryName]
      
	FROM   [dbo].[CustomerGuestAppEnhanceYourStayCategories] 
	WHERE  DeletedAt is null AND Id = @Id");

            migrationBuilder.Sql(@"

GO
/****** Object:  StoredProcedure [dbo].[GetCustomersEnhanceYourStayCategoriesWithRelation]    Script Date: 09-05-2023 17:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomersEnhanceYourStayCategoriesWithRelation] --'',1,2,'CategoryName','ASC',1
(
    @SearchValue NVARCHAR(50) = null,
    @PageNo Int=1,
	@PageSize Int=10, --NoOf Record To Get
	@SortColumn NVARCHAR(20) = 'CategoryName',
    @SortOrder NVARCHAR(5) = 'ASC',
	@CustomerId Int=1
)
AS BEGIN
	SET NOCOUNT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))

	 ; WITH Enhance_Stay_Results AS
	 (
	 SELECT ( 
		SELECT [Id],[CustomerId],[CustomerGuestAppBuilderId],[CategoryName],
			(SELECT [Id]
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
        FROM [dbo].[CustomerGuestAppEnhanceYourStayItems] items
        WHERE DeletedAt is null AND  items.CustomerGuestAppBuilderCategoryId = categories.Id 
        FOR JSON PATH) AS CustomerGuestAppEnhanceYourStayItems
	FROM [dbo].[CustomerGuestAppEnhanceYourStayCategories] categories
    WHERE DeletedAt is null AND CustomerId = @CustomerId AND (
                CategoryName LIKE '%' + @SearchValue + '%')

		ORDER BY
		CASE WHEN (@SortColumn = 'CategoryName' AND @SortOrder='ASC')
        THEN CategoryName
        END ASC,

		CASE WHEN (@SortColumn = 'CategoryName' AND @SortOrder='DESC')
        THEN CategoryName
        END DESC
       
		OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
		FOR JSON PATH) AS CustomerEnhanceYourStayCategoriesWithRelationOut
	)
	
	select *
	from Enhance_Stay_Results
	OPTION (RECOMPILE)
	
END
");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

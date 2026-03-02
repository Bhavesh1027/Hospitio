using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetProducts_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetProducts]    Script Date: 15-05-2023 17:39:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetProducts]
(
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS BEGIN
    SET NOCOUNT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))

    ; WITH Product_Results AS
    (
		SELECT dbo.Products.Id,dbo.Products.Name,dbo.Products.CreatedAt,(dbo.Users.FirstName + SPACE(1) + dbo.Users.LastName) as [CreatedBy] , dbo.Products.IsActive
		,COUNT(*) OVER() as TotalCount
		FROM dbo.Products 
		INNER JOIN
		dbo.Users ON dbo.Products.CreatedBy = dbo.Users.Id

		WHERE dbo.Products.DeletedAt is null and (
		    dbo.Products.Name LIKE '%' + @SearchValue + '%' OR	
			dbo.Products.CreatedAt LIKE '%' + @SearchValue + '%' OR
			dbo.Users.FirstName LIKE '%' + @SearchValue + '%' OR
			dbo.Users.LastName LIKE '%' + @SearchValue + '%' 
		)

		ORDER BY
		CASE WHEN (@SortColumn = 'Name' AND @SortOrder='ASC')
        THEN Name
        END ASC,
        
		CASE WHEN (@SortColumn = 'Name' AND @SortOrder='DESC')
        THEN Name
        END DESC,
		
		CASE WHEN (@SortColumn = 'CreatedAt' AND @SortOrder='ASC')
        THEN Name
        END ASC,
        
		CASE WHEN (@SortColumn = 'CreatedAt' AND @SortOrder='DESC')
        THEN Name
        END DESC,

		CASE WHEN (@SortColumn = 'FirstName' AND @SortOrder='ASC')
        THEN Name
        END ASC,
        
		CASE WHEN (@SortColumn = 'FirstName' AND @SortOrder='DESC')
        THEN Name
        END DESC,
		
		CASE WHEN (@SortColumn = 'IsActive' AND @SortOrder='ASC')
        THEN Name
        END ASC,
        
		CASE WHEN (@SortColumn = 'IsActive' AND @SortOrder='DESC')
        THEN Name
        END DESC
            
		OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )

	select Id,Name,CreatedAt,[CreatedBy],IsActive,TotalCount
	from Product_Results
	OPTION (RECOMPILE)
    
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

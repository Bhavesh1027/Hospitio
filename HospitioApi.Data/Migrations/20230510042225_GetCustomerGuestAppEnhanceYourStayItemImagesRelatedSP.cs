using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerGuestAppEnhanceYourStayItemImagesRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomersGuestAppEnhanceYourStayItemImages]    Script Date: 10-05-2023 09:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetCustomersGuestAppEnhanceYourStayItemImages] --'','',1,2,'Name','ASC',1
(
	@SearchColumn NVARCHAR(50) = null,
    @SearchValue NVARCHAR(50) = null,
    @PageNo Int=1,
	@PageSize Int=10, --NoOf Record To Get
	@SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC',
	@CustomerGuestAppEnhanceYourStayItemId Int=1
)
AS BEGIN
	SET NOCOUNT ON;

    SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
	
	; WITH CTE_Results AS
    (
        SELECT [Id]
      ,[CustomerGuestAppEnhanceYourStayItemId]
      ,[ItemsImages]
      ,[DisaplayOrder]
      ,[IsActive]
	  ,COUNT(*) OVER() as FilteredCount
	  FROM CustomerGuestAppEnhanceYourStayItemsImages WITH (NOLOCK)

        WHERE DeletedAt is null AND CustomerGuestAppEnhanceYourStayItemId = @CustomerGuestAppEnhanceYourStayItemId AND @SearchColumn= '' OR  (
                CASE @SearchColumn
                    WHEN 'ItemsImages' THEN ItemsImages
                END
            ) LIKE '%' + @SearchValue + '%'

		ORDER BY
		CASE WHEN (@SortColumn = '')
        THEN DisaplayOrder
        END ASC,

		CASE WHEN (@SortColumn = 'ItemsImages' AND @SortOrder='ASC')
        THEN ItemsImages
        END ASC,
        
		CASE WHEN (@SortColumn = 'ItemsImages' AND @SortOrder='DESC')
        THEN ItemsImages
        END DESC

		OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )

	select [Id]
      ,[CustomerGuestAppEnhanceYourStayItemId]
      ,[ItemsImages]
      ,[DisaplayOrder]
      ,[IsActive]
	  ,FilteredCount 
	from CTE_Results
	OPTION (RECOMPILE)
	--With Total Rows
    --,CTE_TotalRows AS
    --(
    --    select count(ID) as TotalRows from Modules
    --    WHERE @SearchColumn= '' OR  (
    --            CASE @SearchColumn
    --                WHEN 'Name' THEN Name
    --                --WHEN 'ModuleType' THEN ModuleType
    --            END
    --        ) LIKE '%' + @SearchValue + '%'
    --)
    --Select TotalRows, t.Id, t.Name, t.ModuleType,t.IsActive from dbo.Modules as t, CTE_TotalRows
    --WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.ID = t.ID)

    
END

                "
             );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

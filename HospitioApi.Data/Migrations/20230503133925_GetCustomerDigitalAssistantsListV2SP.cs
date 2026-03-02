using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerDigitalAssistantsListV2SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerDigitalAssistantsListV2]    Script Date: 04-05-2023 12:10:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetCustomerDigitalAssistantsListV2] --'','',1,2,'Name','ASC'
(
    @SearchColumn NVARCHAR(50) = null,
    @SearchValue NVARCHAR(50) = null,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC',
	@CustomerId INT = 1
)
AS BEGIN
    SET NOCOUNT ON;

    SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))

    ; WITH CTE_Results AS
    (
        SELECT [Id],[CustomerId],[Name],[Details],[Icon],[IsActive]
		,COUNT(*) OVER() as FilteredCount
		FROM CustomerDigitalAssistants WITH (NOLOCK)

        WHERE DeletedAt is null AND CustomerId = @CustomerId AND @SearchColumn= '' OR  (
                CASE @SearchColumn
                    WHEN 'Name' THEN Name
                    WHEN 'Details' THEN Details
					WHEN 'Icon' THEN Icon
                END
            ) LIKE '%' + @SearchValue + '%'

		ORDER BY
		CASE WHEN (@SortColumn = 'Name' AND @SortOrder='ASC')
        THEN Name
        END ASC,
        
		CASE WHEN (@SortColumn = 'Name' AND @SortOrder='DESC')
        THEN Name
        END DESC,
        
		CASE WHEN (@SortColumn = 'Details' AND @SortOrder='ASC')
        THEN Details
        END ASC,
        
		CASE WHEN (@SortColumn = 'Details' AND @SortOrder='DESC')
        THEN Details
        END DESC,

		CASE WHEN (@SortColumn = 'Icon' AND @SortOrder='ASC')
        THEN Icon
        END ASC,
        
		CASE WHEN (@SortColumn = 'Icon' AND @SortOrder='DESC')
        THEN Icon
        END DESC
            
		OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )

	select [Id],[CustomerId],[Name],[Details],[Icon],[IsActive],FilteredCount 
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

            // Get Customer Digital Assistant by Id
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetDigitalAssistantsById]    Script Date: 15-05-2023 17:30:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetDigitalAssistantsById] 
    @Id Int=1
	
	
AS 

	SELECT [Id]
      ,[CustomerId]
      ,[Name]
      ,[Details]
      ,[Icon]
	FROM   [dbo].[CustomerDigitalAssistants] WITH (NOLOCK)
	WHERE  Id = @Id and DeletedAt is null
                                ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

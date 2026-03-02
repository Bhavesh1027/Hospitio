using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SampleDapperStoreProcedureWithNestedObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetModulesListV2_sample]    Script Date: 27-04-2023 16:47:18 ******/
DROP PROCEDURE IF EXISTS [dbo].[GetModulesListV2_sample]
GO
/****** Object:  StoredProcedure [dbo].[GetModules_sample]    Script Date: 27-04-2023 16:47:18 ******/
DROP PROCEDURE IF EXISTS [dbo].[GetModules_sample]
GO
/****** Object:  StoredProcedure [dbo].[GetModuleJson_sample]    Script Date: 27-04-2023 16:47:18 ******/
DROP PROCEDURE IF EXISTS [dbo].[GetModuleJson_sample]
GO
/****** Object:  StoredProcedure [dbo].[GetModuleJson_sample]    Script Date: 27-04-2023 16:47:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetModuleJson_sample]
AS

 
SELECT 
( 
SELECT m.[Id], m.[Name], m.[ModuleType], m.[IsActive], m.[CreatedAt], m.[UpdateAt], m.[DeletedAt], m.[CreatedBy]
,JSON_QUERY(( 
SELECT [Id], [ModuleId], [Name], [IsActive], [CreatedAt], [UpdateAt], [DeletedAt], [CreatedBy] 
FROM ModuleServices where ModuleId=m.Id
FOR JSON PATH
)) as [ModuleServices]
FROM Modules m 
FOR JSON PATH )
						

 
GO
/****** Object:  StoredProcedure [dbo].[GetModules_sample]    Script Date: 27-04-2023 16:47:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetModules_sample] 
    @PageNo Int=1,
	@PageSize Int=10 --NoOf Record To Get
	
	
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	 
	SELECT [Id], [Name], [ModuleType], [IsActive], [CreatedAt], [UpdateAt], [DeletedAt], [CreatedBy],
	COUNT(*) OVER() AS TotalCount 
	FROM   [dbo].[Modules] WITH (NOLOCK)
	WHERE  DeletedAt is null
	order by Name
	OffSet @PageSize * (@PageNo-1) Rows
	Fetch Next @PageSize Rows Only
	 
GO
/****** Object:  StoredProcedure [dbo].[GetModulesListV2_sample]    Script Date: 27-04-2023 16:47:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetModulesListV2_sample] --'','',1,2,'Name','ASC'
(
    @SearchColumn NVARCHAR(50) = NULL,
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS BEGIN
    SET NOCOUNT ON;

    SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))

    ; WITH CTE_Results AS
    (
        SELECT [Id], [Name], [ModuleType], [IsActive] 
		,COUNT(*) OVER() as FilteredCount

		FROM Modules WITH (NOLOCK)

        WHERE @SearchColumn= '' OR  (
                CASE @SearchColumn
                    WHEN 'Name' THEN Name
                    --WHEN 'ModuleType' THEN ModuleType
                END
            ) LIKE '%' + @SearchValue + '%'

		ORDER BY
		CASE WHEN (@SortColumn = 'Name' AND @SortOrder='ASC')
        THEN Name
        END ASC,
        
		CASE WHEN (@SortColumn = 'Name' AND @SortOrder='DESC')
        THEN Name
        END DESC
        
		--CASE WHEN (@SortColumn = 'City' AND @SortOrder='ASC')
  --      THEN City
  --      END ASC,
        
		--CASE WHEN (@SortColumn = 'City' AND @SortOrder='DESC')
  --      THEN City
  --      END DESC
            
		OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )

	select [Id], [Name], [ModuleType], [IsActive],FilteredCount 
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
GO

                ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

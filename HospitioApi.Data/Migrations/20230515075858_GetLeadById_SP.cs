using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetLeadById_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetLeadById]    Script Date: 15-05-2023 13:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create OR ALTER Procedure [dbo].[GetLeadById]
@Id int =0
AS

	SET NOCOUNT ON 
	SET XACT_ABORT ON  
select [Id]
      ,[FirstName]
      ,[LastName]
      ,[Email]
      ,[Comment]
      ,[PhoneCountry]
      ,[PhoneNumber]
      ,[ContactFor]
      ,[IsActive]
	  ,[Company]
	  From Leads
	  Where DeletedAt is null 
	  And Id = @Id
");
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetLeads]    Script Date: 15-05-2023 09:41:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetLeads]
(
    @SearchValue NVARCHAR(50) = null,
    @PageNo Int=1,
	@PageSize Int=10, --NoOf Record To Get
	@SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS BEGIN
	SET NOCOUNT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
	
	; WITH CTE_Results AS
    (
        SELECT [Id]
      ,ISNULL([FirstName],'') + ' ' + ISNULL([LastName],'') AS Name
      ,[Email]
	  ,[Comment]
      ,[PhoneNumber]
      ,[ContactFor]
      ,[IsActive]
      ,[CreatedAt]
      ,[UpdateAt]
      ,[Company]
	  ,COUNT(*) OVER() as FilteredCount
	  FROM [Leads]  WITH (NOLOCK)

        WHERE DeletedAt is null AND (
                FirstName LIKE '%' + @SearchValue + '%' OR 
                LastName LIKE '%' + @SearchValue + '%' OR
				Company LIKE '%' + @SearchValue + '%' OR
				PhoneNumber LIKE '%' + @SearchValue + '%' OR
				Email LIKE '%' + @SearchValue + '%'
            )

		ORDER BY
		CASE WHEN @SortColumn = 'Name' AND @SortOrder = 'ASC' 
        THEN ISNULL(FirstName, '') + ' ' + ISNULL(LastName, '')
		END ASC,
    
	    CASE WHEN @SortColumn = 'Name' AND @SortOrder = 'DESC'
        THEN ISNULL(FirstName, '') + ' ' + ISNULL(LastName, '')
        END DESC,

		CASE WHEN (@SortColumn = 'Company' AND @SortOrder='ASC')
        THEN Company
        END ASC,
        
		CASE WHEN (@SortColumn = 'Company' AND @SortOrder='DESC')
        THEN Company
        END DESC,

		CASE WHEN (@SortColumn = 'PhoneNumber' AND @SortOrder='ASC')
        THEN PhoneNumber
        END ASC,
        
		CASE WHEN (@SortColumn = 'PhoneNumber' AND @SortOrder='DESC')
        THEN PhoneNumber
        END DESC,

		CASE WHEN (@SortColumn = 'Email' AND @SortOrder='ASC')
        THEN Email
        END ASC,
        
		CASE WHEN (@SortColumn = 'Email' AND @SortOrder='DESC')
        THEN Email
        END DESC

		OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )

	SELECT [Id]
      ,[Name]
      ,[Email]
	  ,[Comment]
      ,[PhoneNumber]
      ,[ContactFor]
      ,[IsActive]
      ,[CreatedAt]
      ,[UpdateAt]
      ,[Company]
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
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

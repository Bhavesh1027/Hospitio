using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerPropertyInfoRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomersPropertiesInfo]    Script Date: 08-05-2023 09:49:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomersPropertiesInfo] --'','',1,2,'Name','ASC',1
(
	@SearchColumn NVARCHAR(50) = null,
    @SearchValue NVARCHAR(50) = null,
    @PageNo Int=1,
	@PageSize Int=10, --NoOf Record To Get
	@SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC',
	@CustomerId Int=1
)
AS BEGIN
	SET NOCOUNT ON;

    SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
	
	; WITH CTE_Results AS
    (
        SELECT [Id]
      ,[CustomerId]
      ,[CustomerGuestAppBuilderId]
      ,[WifiUsername]
      ,[WifiPassword]
      ,[Overview]
      ,[CheckInPolicy]
      ,[TermsAndConditions]
      ,[Street]
      ,[StreetNumber]
      ,[City]
      ,[Postalcode]
      ,[Country]
      ,[IsActive]
	  ,COUNT(*) OVER() as FilteredCount
	  FROM CustomerPropertyInformations WITH (NOLOCK)

        WHERE DeletedAt is null AND CustomerId = @CustomerId AND @SearchColumn= '' OR  (
                CASE @SearchColumn
                    WHEN 'WifiUsername' THEN WifiUsername
                    WHEN 'WifiPassword' THEN WifiPassword
					WHEN 'Overview' THEN Overview
					WHEN 'CheckInPolicy' THEN CheckInPolicy
                    WHEN 'TermsAndConditions' THEN TermsAndConditions
					WHEN 'Street' THEN Street
					WHEN 'StreetNumber' THEN StreetNumber
                    WHEN 'City' THEN City
					WHEN 'Postalcode' THEN Postalcode
					WHEN 'Country' THEN Country
                END
            ) LIKE '%' + @SearchValue + '%'

		ORDER BY
		CASE WHEN (@SortColumn = 'WifiUsername' AND @SortOrder='ASC')
        THEN WifiUsername
        END ASC,
        
		CASE WHEN (@SortColumn = 'WifiUsername' AND @SortOrder='DESC')
        THEN WifiUsername
        END DESC,

		CASE WHEN (@SortColumn = 'WifiPassword' AND @SortOrder='ASC')
        THEN WifiPassword
        END ASC,
        
		CASE WHEN (@SortColumn = 'WifiPassword' AND @SortOrder='DESC')
        THEN WifiPassword
        END DESC,

		CASE WHEN (@SortColumn = 'Overview' AND @SortOrder='ASC')
        THEN Overview
        END ASC,
        
		CASE WHEN (@SortColumn = 'Overview' AND @SortOrder='DESC')
        THEN Overview
        END DESC,

		CASE WHEN (@SortColumn = 'CheckInPolicy' AND @SortOrder='ASC')
        THEN CheckInPolicy
        END ASC,
        
		CASE WHEN (@SortColumn = 'CheckInPolicy' AND @SortOrder='DESC')
        THEN CheckInPolicy
        END DESC,

		CASE WHEN (@SortColumn = 'TermsAndConditions' AND @SortOrder='ASC')
        THEN TermsAndConditions
        END ASC,
        
		CASE WHEN (@SortColumn = 'TermsAndConditions' AND @SortOrder='DESC')
        THEN TermsAndConditions
        END DESC,

		CASE WHEN (@SortColumn = 'Street' AND @SortOrder='ASC')
        THEN Street
        END ASC,
        
		CASE WHEN (@SortColumn = 'Street' AND @SortOrder='DESC')
        THEN Street
        END DESC,

		CASE WHEN (@SortColumn = 'StreetNumber' AND @SortOrder='ASC')
        THEN StreetNumber
        END ASC,
        
		CASE WHEN (@SortColumn = 'StreetNumber' AND @SortOrder='DESC')
        THEN StreetNumber
        END DESC,

		CASE WHEN (@SortColumn = 'City' AND @SortOrder='ASC')
        THEN City
        END ASC,
        
		CASE WHEN (@SortColumn = 'City' AND @SortOrder='DESC')
        THEN City
        END DESC

		OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )

	select [Id]
      ,[CustomerId]
      ,[CustomerGuestAppBuilderId]
      ,[WifiUsername]
      ,[WifiPassword]
      ,[Overview]
      ,[CheckInPolicy]
      ,[TermsAndConditions]
      ,[Street]
      ,[StreetNumber]
      ,[City]
      ,[Postalcode]
      ,[Country]
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

            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomersPropertiesInfoById]    Script Date: 15-05-2023 17:31:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetCustomersPropertiesInfoById] 
    @Id Int=1
	
	
AS 

	SELECT [Id]
      ,[CustomerId]
      ,[CustomerGuestAppBuilderId]
      ,[WifiUsername]
      ,[WifiPassword]
      ,[Overview]
      ,[CheckInPolicy]
      ,[TermsAndConditions]
      ,[Street]
      ,[StreetNumber]
      ,[City]
      ,[Postalcode]
      ,[Country]
      ,[IsActive]
	FROM   [dbo].[CustomerPropertyInformations] WITH (NOLOCK)
	WHERE  Id = @Id and DeletedAt is null
                                ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

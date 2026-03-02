using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddCustomerPropertyServiceSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Get Customer Property Services
            migrationBuilder.Sql(@"
                                    CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPropertyServices]
                                    (
                                     	@CustomerPropertyInformationId int = 0,
                                         @SearchColumn NVARCHAR(50) = '',
                                         @SearchValue NVARCHAR(50) = '',
                                         @PageNo INT = 1,
                                         @PageSize INT = 10,
                                         @SortColumn NVARCHAR(20) = 'Name',
                                         @SortOrder NVARCHAR(5) = 'ASC'
                                     )
                                     AS
                                     BEGIN
                                     
                                     SET NOCOUNT ON;
                                     
                                         SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
                                         SET @SearchValue = LTRIM(RTRIM(@SearchValue))
                                     
                                         ; WITH CustomerPropertyInformations_Results AS
                                         (
                                             SELECT [Id],[CustomerPropertyInformationId],[Name],[Icon],[Description],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy]
                                     		,COUNT(*) OVER() as FilteredCount
                                     
                                     		FROM [dbo].[CustomerPropertyServices] WITH (NOLOCK)
                                     
                                             WHERE DeletedAt is null AND CustomerPropertyInformationId = @CustomerPropertyInformationId AND @SearchColumn= '' OR  (
                                                     CASE @SearchColumn
                                                         WHEN 'Name' THEN Name
                                                     END
                                                 ) LIKE '%' + @SearchValue + '%'
                                     
                                     		ORDER BY
                                     		CASE WHEN (@SortColumn = 'Name' AND @SortOrder='ASC')
                                             THEN Name
                                             END ASC,
                                             
                                     		CASE WHEN (@SortColumn = 'Name' AND @SortOrder='DESC')
                                             THEN Name
                                             END DESC
                                     
                                     		OFFSET @PageSize * (@PageNo - 1) ROWS
                                             FETCH NEXT @PageSize ROWS ONLY
                                         )
                                     
                                         select [Id],[CustomerPropertyInformationId],[Name],[Icon],[Description],[IsActive],[CreatedAt],[UpdateAt],[DeletedAt],[CreatedBy],FilteredCount 
                                     	from CustomerPropertyInformations_Results
                                     	OPTION (RECOMPILE)
                                     END
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

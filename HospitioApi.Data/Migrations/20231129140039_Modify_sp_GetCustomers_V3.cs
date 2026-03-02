using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_sp_GetCustomers_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
/****** Object:  StoredProcedure [dbo].[GetCustomers]    Script Date: 29-11-2023 19:18:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER       PROCEDURE [dbo].[GetCustomers] -- '',1,10,'','',''
(
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'CreatedAt',
    @SortOrder NVARCHAR(5) = 'ASC',
    @AlphabetsStartsWith NVARCHAR(50) = NULL
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
    SET @AlphabetsStartsWith = LTRIM(RTRIM(@AlphabetsStartsWith));
    WITH Customer_Results
    AS (SELECT [dbo].[Customers].[Id],
               [dbo].[CustomerGuestsCheckInFormBuilders].[Logo] AS [ProfilePicture],
               [dbo].[Customers].[BusinessName],
               [dbo].[BusinessTypes].[BizType],
               [dbo].[Products].[Name] AS [ServicePackName],
               COUNT(*) OVER () AS [FilteredCount],
			   CU.[FirstName],
			   CU.[LastName]
        FROM [dbo].[Customers] (NOLOCK)
            INNER JOIN [dbo].[BusinessTypes] (NOLOCK)
                ON [dbo].[Customers].[BusinessTypeId] = [dbo].[BusinessTypes].[Id]
            LEFT OUTER JOIN [dbo].[Products] (NOLOCK)
                ON [dbo].[Customers].[ProductId] = [dbo].[Products].[Id]
            LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] (NOLOCK)
                ON [dbo].[Customers].[Id] = [dbo].[CustomerGuestsCheckInFormBuilders].[CustomerId]
				OUTER APPLY (
            SELECT TOP 1 [FirstName],[LastName]
            FROM [dbo].[CustomerUsers] CU
            WHERE CU.[CustomerId] = [dbo].[Customers].[Id]
              AND CU.[DeletedAt] IS NULL
            --ORDER BY CU.[CreatedAt] DESC
        ) CU
        WHERE [dbo].[Customers].[DeletedAt] IS NULL
              AND (
                      [dbo].[BusinessTypes].[BizType] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[Products].[Name] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[Customers].[BusinessName] LIKE '%' + @SearchValue + '%'
					  OR CU.[FirstName] LIKE '%' + @SearchValue + '%'
                  )
              AND (
                      @AlphabetsStartsWith IS NULL
                      OR EXISTS
        (
            SELECT 1 FROM STRING_SPLIT(@AlphabetsStartsWith, ',') AS s
        )
                  )
        ORDER BY CASE
                     WHEN @SortColumn = 'BusinessName'
                          AND @SortOrder = 'ASC' THEN
                         [dbo].[Customers].[BusinessName]
                 END ASC,
                 CASE
                     WHEN @SortColumn = 'BusinessName'
                          AND @SortOrder = 'DESC' THEN
                         [dbo].[Customers].[BusinessName]
                 END DESC,
				 CASE
                     WHEN @SortColumn = 'CreatedAt'
                          AND @SortOrder = 'ASC' THEN
                         [dbo].[Customers].[CreatedAt]
                 END DESC,
				  CASE
                     WHEN @SortColumn = 'CreatedAt'
                          AND @SortOrder = 'DESC' THEN
                         [dbo].[Customers].[CreatedAt]
                 END ASC
				 OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [ProfilePicture],
           [BusinessName],
           [BizType],
           [ServicePackName],
           [FilteredCount],
		   [FirstName],
		   [LastName]
    FROM Customer_Results
    OPTION (RECOMPILE)
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

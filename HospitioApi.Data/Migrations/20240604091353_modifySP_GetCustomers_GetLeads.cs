using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class modifySP_GetCustomers_GetLeads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomers
            migrationBuilder.Sql(@"/****** Object:  StoredProcedure [dbo].[GetCustomers]    Script Date: 04-Jun-24 1:52:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER         PROCEDURE [dbo].[GetCustomers] -- '',1,10,'','',''
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
			WHERE LTRIM(RTRIM([FirstName])) LIKE LTRIM(RTRIM(s.value)) + '%'
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
END");
            #endregion

            #region GetLeads
            migrationBuilder.Sql(@"/****** Object:  StoredProcedure [dbo].[GetLeads]    Script Date: 04-Jun-24 12:22:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetLeads] 
(
    @SearchValue NVARCHAR(50) = '',
    @PageNo INT = 1,
    @PageSize INT = 10, --NoOf Record To Get
    @SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC',
    @AlphabetsStartsWith NVARCHAR(50) = ''
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
    SET @AlphabetsStartsWith = LTRIM(RTRIM(@AlphabetsStartsWith));
    WITH CTE_Results
    AS (SELECT [Id],
               ISNULL([FirstName], '') + SPACE(1) + ISNULL([LastName], '') AS [Name],
               [Email],
               [Comment],
               [PhoneNumber],
               [ContactFor],
               [IsActive],
               [CreatedAt],
               [UpdateAt],
               [Company],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[Leads] WITH (NOLOCK)
        WHERE [DeletedAt] IS NULL
              AND (
                      [FirstName] LIKE '%' + @SearchValue + '%'
                      OR [LastName] LIKE '%' + @SearchValue + '%'
                      OR [Company] LIKE '%' + @SearchValue + '%'
                      OR [PhoneNumber] LIKE '%' + @SearchValue + '%'
                      OR [Email] LIKE '%' + @SearchValue + '%'
                  )
              AND (
                      @AlphabetsStartsWith IS NULL
                      OR EXISTS
        (
            SELECT 1
            FROM STRING_SPLIT(@AlphabetsStartsWith, ',') AS s
            WHERE LTRIM(RTRIM([FirstName])) LIKE LTRIM(RTRIM(s.value)) + '%'
        )
                  )
        ORDER BY CASE
                     WHEN @SortColumn = 'Name'
                          AND @SortOrder = 'ASC' THEN
                         ISNULL(FirstName, '') + ' ' + ISNULL(LastName, '')
                 END ASC,
                 CASE
                     WHEN @SortColumn = 'Name'
                          AND @SortOrder = 'DESC' THEN
                         ISNULL(FirstName, '') + ' ' + ISNULL(LastName, '')
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Company'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Company
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Company'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Company
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'PhoneNumber'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         PhoneNumber
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'PhoneNumber'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         PhoneNumber
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Email'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Email
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Email'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Email
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [Name],
           [Email],
           [Comment],
           [PhoneNumber],
           [ContactFor],
           [IsActive],
           [CreatedAt],
           [UpdateAt],
           [Company],
           [FilteredCount]
    FROM CTE_Results
    OPTION (RECOMPILE)

END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

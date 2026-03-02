using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomersRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomers]    Script Date: 31-05-2023 15:34:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetCustomers] -- '',1,2,'','','a,c'
(
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'BusinessName',
    @SortOrder NVARCHAR(5) = 'ASC',
    @AlphabetsStartsWith NVARCHAR(50) = NULL
)
AS BEGIN
    SET NOCOUNT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
    SET @AlphabetsStartsWith = LTRIM(RTRIM(@AlphabetsStartsWith))

    ;WITH Customer_Results AS
    (
        SELECT
            dbo.Customers.Id,
            dbo.CustomerUsers.Title,
            dbo.CustomerUsers.FirstName,
            dbo.CustomerUsers.LastName,
            dbo.CustomerUsers.ProfilePicture,
            dbo.Customers.BusinessName,
            dbo.BusinessTypes.BizType,
            dbo.Products.Name AS ""ServicePackName"",
            COUNT(*) OVER() AS FilteredCount
        FROM
            dbo.Customers
        INNER JOIN dbo.BusinessTypes ON dbo.Customers.BusinessTypeId = dbo.BusinessTypes.Id
        LEFT OUTER JOIN dbo.Products ON dbo.Customers.ProductId = dbo.Products.Id
        LEFT OUTER JOIN dbo.CustomerUsers ON dbo.Customers.Id = dbo.CustomerUsers.CustomerId
        WHERE
            dbo.Customers.DeletedAt IS NULL
            AND (
                dbo.BusinessTypes.BizType LIKE '%' + @SearchValue + '%'
                OR dbo.Products.Name LIKE '%' + @SearchValue + '%'
                OR dbo.CustomerUsers.LastName LIKE '%' + @SearchValue + '%'
            )
            AND (
                @AlphabetsStartsWith IS NULL
                OR EXISTS (
                    SELECT 1
                    FROM STRING_SPLIT(@AlphabetsStartsWith, ',') AS s
                    WHERE dbo.CustomerUsers.FirstName LIKE LTRIM(RTRIM(s.value)) + '%'
                )
            )
        ORDER BY
            CASE
                WHEN @SortColumn = 'BusinessName' AND @SortOrder = 'ASC' THEN dbo.Customers.BusinessName
            END ASC,
            CASE
                WHEN @SortColumn = 'BusinessName' AND @SortOrder = 'DESC' THEN dbo.Customers.BusinessName
            END DESC
        OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )
    SELECT
        Id,
        FirstName,
        LastName,
        ProfilePicture,
        BusinessName,
        Title,
        BizType,
        'Name' AS ""ServicePackName"",
        FilteredCount
    FROM
        Customer_Results
    OPTION (RECOMPILE)
END

                "
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

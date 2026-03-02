using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_getCustomerForNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO

/****** Object: StoredProcedure [dbo].[SP_GetCustomerForNotification] Script Date: 21-08-2023 14:57:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[SP_GetCustomerForNotification] 
(
    @businessTypeId INT,
    @productId INT,
    @country nvarchar(max),
    @city nvarchar(max),
    @postalcode nvarchar(max)
)
AS 
BEGIN
    SELECT [dbo].[Customers].Id AS CustomerId
    FROM [dbo].[Customers]
    WHERE ([dbo].[Customers].BusinessTypeId = @businessTypeId OR ISNULL(@businessTypeId, 0) = 0)
        AND ([dbo].[Customers].ProductId = @productId OR ISNULL(@productId, 0) = 0)
        AND ([dbo].[Customers].Country = @country OR ISNULL(@country, '') = '')
        AND ([dbo].[Customers].City = @city OR ISNULL(@city, '') = '')
        AND ([dbo].[Customers].Postalcode = @postalcode OR ISNULL(@postalcode, '') = '')
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

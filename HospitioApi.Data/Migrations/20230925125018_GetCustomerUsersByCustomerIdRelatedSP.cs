using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerUsersByCustomerIdRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerUsersByCustomerId]    Script Date: 25-09-2023 18:22:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerUsersByCustomerId]
(
	@CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           ISNULL([FirstName], '') + SPACE(1) + ISNULL([LastName], '') AS [Name],
		   [PhoneCountry],
		   [PhoneNumber]
    FROM [dbo].[CustomerUsers] (NOLOCK)
    WHERE [DeletedAt] IS NULL AND [IsActive] = 1
          AND [CustomerId] = @CustomerId
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

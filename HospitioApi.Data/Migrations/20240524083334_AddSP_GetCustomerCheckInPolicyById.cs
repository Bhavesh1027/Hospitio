using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddSP_GetCustomerCheckInPolicyById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER         PROCEDURE [dbo].[GetCustomerCheckInPolicyById] 
(
	@CustomerId int = NULL
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [M].[CheckInPolicy],
		   [M].[CheckOutPolicy]
    FROM [dbo].[Customers] M (NOLOCK)
    WHERE [M].[DeletedAt] IS NULL
          AND [M].[Id] = @CustomerId
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

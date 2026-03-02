using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerStaffAlertsByCustomerIdRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerStaffAlertsByCustomerId]    Script Date: 26-09-2023 13:44:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerStaffAlertsByCustomerId] 
(
	@CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerId],
           [Name],
           [Platfrom],
           [PhoneCountry],
           [PhoneNumber],
           [WaitTimeInMintes],
           [IsActive],
		   [Msg],
		   [CustomerUserId]
    FROM [dbo].[CustomerStaffAlerts] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [CustomerId] = @CustomerId

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

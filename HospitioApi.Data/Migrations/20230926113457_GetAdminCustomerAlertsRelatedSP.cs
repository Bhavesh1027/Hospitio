using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetAdminCustomerAlertsRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetAdminCustomerAlerts] 
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

    SELECT [Id],
           [Msg],
           [MsgWaitTimeInMinutes],
           [IsActive]
    FROM [dbo].[AdminCustomerAlerts] (NOLOCK)
    WHERE [DeletedAt] IS NULL
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

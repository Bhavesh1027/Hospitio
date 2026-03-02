using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetAdminStaffAlertsRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetAdminStaffAlerts] 
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
           [Platfrom],
           [PhoneCountry],
           [PhoneNumber],
           [WaitTimeInMintes],
           [IsActive],
		   [Msg],
		   [UserId]
    FROM [dbo].[AdminStaffAlerts] (NOLOCK)
    WHERE [DeletedAt] IS NULL

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

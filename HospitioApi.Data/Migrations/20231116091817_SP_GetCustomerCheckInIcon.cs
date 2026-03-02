using Microsoft.EntityFrameworkCore.Migrations;
using HospitioApi.Data.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetCustomerCheckInIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROCEDURE[dbo].[GetCustomerCheckInIcon]-- 1
(
@CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

 SELECT
 [CG].[JsonData]
 FROM[dbo].[CustomerGuestsCheckInFormBuilders](NOLOCK) AS CG
 WHERE CG.[CustomerId] = @CustomerId AND
 CG.[DeletedAt] IS NULL
 END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

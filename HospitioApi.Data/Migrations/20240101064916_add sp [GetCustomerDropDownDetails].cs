using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class addspGetCustomerDropDownDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"/****** Object:  StoredProcedure [dbo].[GetCustomerDropDownDetails]    Script Date: 01-01-2024 12:15:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROCEDURE [dbo].[GetCustomerDropDownDetails]

AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

    SELECT Id,
			BusinessName as CustomerName 
	FROM Customers 
	where DeletedAt is null;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

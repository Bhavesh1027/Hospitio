using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class addspinmigrationSP_GetCustomerCustomerUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"/****** Object:  StoredProcedure [dbo].[SP_GetCustomer&CustomerUsers]    Script Date: 22-11-2023 11:11:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SP_GetCustomer&CustomerUsers]
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    select [dbo].[Customers].[Id] As Id ,[dbo].[CustomerUsers].Id As CustomerUserId from 
	[dbo].[Customers] 
	INNER JOIN [dbo].[CustomerUsers]
	ON [dbo].[Customers].Id = [dbo].[CustomerUsers].CustomerId
    
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

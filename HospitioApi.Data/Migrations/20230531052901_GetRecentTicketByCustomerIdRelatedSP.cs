using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetRecentTicketByCustomerIdRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetRecentTicketByCustomerId]    Script Date: 31-05-2023 10:58:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   Procedure [dbo].[GetRecentTicketByCustomerId]
(
	@CustomerId INT = 1
)
AS
SET NOCOUNT ON 
SET XACT_ABORT ON 
select top 5
	   [Id]
      ,[CustomerId]
      ,[Title]
      ,[Details]
      ,[Priority]
      ,[Duedate]
      ,[TicketCategoryId]
      ,[CSAgentId]
      ,[Status]
      ,[CloseDate]
      ,[CreatedFrom]
      ,[IsActive]
      ,[CreatedAt]
from Tickets where DeletedAt is null and Status != 3 and CustomerId = @CustomerId
order by 
CreatedAt DESC
                "
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

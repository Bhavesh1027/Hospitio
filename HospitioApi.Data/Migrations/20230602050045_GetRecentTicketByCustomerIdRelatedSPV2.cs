using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetRecentTicketByCustomerIdRelatedSPV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetRecentTicketByCustomerId]    Script Date: 02-06-2023 10:29:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[GetRecentTicketByCustomerId] 
(
	@CustomerId INT = 1
)
AS
SET NOCOUNT ON 
SET XACT_ABORT ON 

IF @CustomerId <> 0
BEGIN
    SELECT TOP 5
        [Id],
        [CustomerId],
        [Title],
        [Details],
        [Priority],
        [Duedate],
        [TicketCategoryId],
        [CSAgentId],
        [Status],
        [CloseDate],
        [CreatedFrom],
        [IsActive],
        [CreatedAt]
    FROM Tickets 
    WHERE DeletedAt IS NULL 
        AND Status != 3 
        AND CustomerId = @CustomerId
    ORDER BY CreatedAt DESC
END
ELSE
BEGIN
    SELECT TOP 5
        [Id],
        [CustomerId],
        [Title],
        [Details],
        [Priority],
        [Duedate],
        [TicketCategoryId],
        [CSAgentId],
        [Status],
        [CloseDate],
        [CreatedFrom],
        [IsActive],
        [CreatedAt]
    FROM Tickets 
    WHERE DeletedAt IS NULL 
        AND Status != 3
    ORDER BY CreatedAt DESC
END
                "
          );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

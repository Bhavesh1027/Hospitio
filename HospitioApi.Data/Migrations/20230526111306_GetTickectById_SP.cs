using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetTickectById_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region Get Tickect ById

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetTicketById]    Script Date: 26-05-2023 16:13:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetTicketById]
(
	@Id int =1
)
AS
SET NOCOUNT ON 
SET XACT_ABORT ON  
SELECT 
( 
SELECT t.[Id],t.[Title],t.Details,t.CloseDate,t.CreatedFrom,t.CSAgentId,Isnull(U.FirstName,'') + ' ' + Isnull(U.LastName,'') as CSAgentName, t.CustomerId
,Isnull(C.FirstName,'') + ' ' + Isnull(C.LastName,'') as CustomerName, t.Duedate, t.Priority, t.Status, t.CreatedAt
,JSON_QUERY(( 
SELECT TR.Id,TR.Reply,TR.TicketId,TR.CreatedAt
FROM TicketReplies TR where TicketId = t.Id
FOR JSON PATH
)) as [Replies]
FROM Tickets t 
INNER JOIN Users U ON U.Id = t.CSAgentId
INNER JOIN CustomerUsers C ON C.Id = t.CustomerId

Where t.Id = @Id
FOR JSON PATH )
");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
